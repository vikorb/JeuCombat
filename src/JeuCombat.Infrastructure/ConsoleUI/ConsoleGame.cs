using JeuCombat.Application.Combat.Actions;
using JeuCombat.Application.Combat.Chance;
using JeuCombat.Application.Combat.Commands;
using JeuCombat.Application.Combat.Events;
using JeuCombat.Application.Combat.Journal;
using JeuCombat.Application.Combat.Sessions;
using JeuCombat.Application.Combat.States;
using JeuCombat.Application.Factories;
using JeuCombat.Domain.Entites;
using JeuCombat.Domain.Enums;

namespace JeuCombat.Infrastructure.ConsoleUI;

public sealed class ConsoleGame
{
    private readonly IConsoleInputReader _inputReader;
    private readonly IConsoleRenderer _renderer;
    private readonly IHerosFactory _herosFactory;
    private readonly IEnnemiFactory _ennemiFactory;
    private readonly IVagueFactory _vagueFactory;
    private readonly IChanceProvider _chanceProvider;

    public ConsoleGame(
        IConsoleInputReader inputReader,
        IConsoleRenderer renderer)
    {
        _inputReader = inputReader;
        _renderer = renderer;
        _herosFactory = new HerosFactory();
        _ennemiFactory = new EnnemiFactory();
        _vagueFactory = new VagueFactory(_ennemiFactory);
        _chanceProvider = new RandomChanceProvider();
    }

    public void Lancer()
    {
        _renderer.AfficherAccueil();

        string nomHeros = _inputReader.LireTexteObligatoire("Nom du héros : ");
        ClasseHero classeHero = ChoisirClasseHero();

        var heros = _herosFactory.Creer(nomHeros, classeHero);
        var vagues = _vagueFactory.CreerToutesLesVagues();

        var journal = new InMemoryCombatJournal();
        var eventPublisher = new CombatEventPublisher();

        eventPublisher.AjouterObservateur(journal);
        eventPublisher.AjouterObservateur(new ConsoleCombatObserver());

        var session = new CombatSession(heros, vagues, eventPublisher);

        JouerCombat(session, journal, eventPublisher);

        _renderer.AfficherFin(session);
    }

    private void JouerCombat(
        CombatSession session,
        ICombatJournal journal,
        ICombatEventPublisher eventPublisher)
    {
        while (!session.EstTerminee)
        {
            if (session.EtatCourant.Type == CombatStateType.TourJoueur)
            {
                JouerTourJoueur(session, journal, eventPublisher);
            }

            while (!session.EstTerminee && session.EtatCourant.Type != CombatStateType.TourJoueur)
            {
                session.ExecuterEtatCourant();
            }
        }
    }

    private void JouerTourJoueur(
        CombatSession session,
        ICombatJournal journal,
        ICombatEventPublisher eventPublisher)
    {
        _renderer.AfficherEtatCombat(session);
        _renderer.AfficherMenuActions(session.Heros);

        int choix = _inputReader.LireEntier("Votre choix : ");

        var commande = CreerCommandeDepuisChoix(
            choix,
            session,
            journal,
            eventPublisher);

        if (commande is null)
        {
            _renderer.AfficherErreur("Choix invalide.");
            return;
        }

        var resultat = session.ExecuterCommandeJoueur(commande);

        if (!resultat.EstReussi || !resultat.TermineLeTour)
        {
            _renderer.AfficherMessage(resultat.Message);
        }

        if (!session.EstTerminee)
        {
            _inputReader.AttendreValidation();
        }
    }

    private ICommand? CreerCommandeDepuisChoix(
        int choix,
        CombatSession session,
        ICombatJournal journal,
        ICombatEventPublisher eventPublisher)
    {
        return choix switch
        {
            1 => new AttaquerCommand(
                new AttaqueBasiqueAction(),
                session.Heros,
                ChoisirCible(session),
                eventPublisher),

            2 => CreerCommandeCompetence(session, eventPublisher),

            3 => new SoignerCommand(
                new SoinAction(),
                session.Heros,
                eventPublisher),

            4 => new AfficherJournalCommand(journal),

            _ => null
        };
    }

    private ICommand CreerCommandeCompetence(
        CombatSession session,
        ICombatEventPublisher eventPublisher)
    {
        ICombatAction competence = CreerCompetence(session.Heros);

        return new UtiliserCompetenceCommand(
            competence,
            session.Heros,
            ChoisirCible(session),
            eventPublisher);
    }

    private ICombatAction CreerCompetence(Heros heros)
    {
        return heros.Classe switch
        {
            ClasseHero.Guerrier => new CompetenceGuerrierAction(),
            ClasseHero.Mage => new CompetenceMageAction(),
            ClasseHero.Voleur => new CompetenceVoleurAction(_chanceProvider),
            _ => throw new ArgumentOutOfRangeException(
                nameof(heros),
                heros.Classe,
                "Classe de héros inconnue.")
        };
    }

    private Ennemi ChoisirCible(CombatSession session)
    {
        var ennemisVivants = session.VagueCourante.Ennemis
            .Where(ennemi => !ennemi.EstVaincu)
            .ToList();

        if (ennemisVivants.Count == 1)
        {
            return ennemisVivants[0];
        }

        while (true)
        {
            _renderer.AfficherCibles(ennemisVivants);

            int choix = _inputReader.LireEntier("Cible : ");

            if (choix >= 1 && choix <= ennemisVivants.Count)
            {
                return ennemisVivants[choix - 1];
            }

            _renderer.AfficherErreur("Cible invalide.");
        }
    }

    private ClasseHero ChoisirClasseHero()
    {
        while (true)
        {
            _renderer.AfficherClassesHero();

            int choix = _inputReader.LireEntier("Classe : ");

            if (Enum.IsDefined(typeof(ClasseHero), choix))
            {
                return (ClasseHero)choix;
            }

            _renderer.AfficherErreur("Classe invalide.");
        }
    }
}
