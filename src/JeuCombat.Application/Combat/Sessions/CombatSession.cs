using JeuCombat.Application.Combat.Commands;
using JeuCombat.Application.Combat.Events;
using JeuCombat.Application.Combat.States;
using JeuCombat.Domain.Constants;
using JeuCombat.Domain.Entites;

namespace JeuCombat.Application.Combat.Sessions;

public sealed class CombatSession
{
    private readonly ICombatEventPublisher _eventPublisher;
    private int _indexVagueCourante;

    public CombatSession(
        Heros heros,
        IReadOnlyList<Vague> vagues,
        ICombatEventPublisher eventPublisher)
    {
        ArgumentNullException.ThrowIfNull(heros);
        ArgumentNullException.ThrowIfNull(vagues);
        ArgumentNullException.ThrowIfNull(eventPublisher);

        if (vagues.Count == 0)
        {
            throw new ArgumentException("Une session de combat doit contenir au moins une vague.", nameof(vagues));
        }

        Heros = heros;
        Vagues = vagues;
        _eventPublisher = eventPublisher;
        EtatCourant = new TourJoueurState();
        _indexVagueCourante = 0;
    }

    public Heros Heros { get; }

    public IReadOnlyList<Vague> Vagues { get; }

    public ICombatState EtatCourant { get; private set; }

    public Vague VagueCourante => Vagues[_indexVagueCourante];

    public int NumeroVagueCourante => VagueCourante.Numero;

    public bool EstTerminee =>
        EtatCourant.Type is CombatStateType.Victoire or CombatStateType.Defaite;

    public bool EstDerniereVague => _indexVagueCourante == Vagues.Count - 1;

    public CombatCommandResult ExecuterCommandeJoueur(ICommand commande)
    {
        ArgumentNullException.ThrowIfNull(commande);

        if (EtatCourant.Type != CombatStateType.TourJoueur)
        {
            return new CombatCommandResult(
                false,
                false,
                "Ce n'est pas le tour du joueur.");
        }

        var resultat = commande.Executer();

        if (!resultat.EstReussi || !resultat.TermineLeTour)
        {
            return resultat;
        }

        DefinirEtatApresActionJoueur();

        return resultat;
    }

    public void ExecuterEtatCourant()
    {
        EtatCourant.Executer(this);
    }

    public void ChangerEtat(ICombatState nouvelEtat)
    {
        ArgumentNullException.ThrowIfNull(nouvelEtat);

        EtatCourant = nouvelEtat;
        EtatCourant.Entrer(this);
    }

    public void ExecuterTourDesEnnemis()
    {
        foreach (var ennemi in VagueCourante.Ennemis.Where(ennemi => !ennemi.EstVaincu))
        {
            if (Heros.EstVaincu)
            {
                break;
            }

            int degats = ennemi.AttaqueBase;

            Heros.RecevoirDegats(degats);

            _eventPublisher.Publier(new CombatEvent(
                $"{ennemi.Nom} attaque {Heros.Nom} et inflige {degats} dégâts."));

            if (Heros.EstVaincu)
            {
                _eventPublisher.Publier(new CombatEvent($"{Heros.Nom} est vaincu."));
            }
        }

        Heros.ReduireCooldownCompetence();
    }

    public void PasserALaVagueSuivante()
    {
        if (EstDerniereVague)
        {
            ChangerEtat(new VictoireState());
            return;
        }

        int numeroVagueTerminee = VagueCourante.Numero;
        int pointsDeVieAvantSoin = Heros.PointsDeVie;
        int pointsDeVieARestaurer = CalculerPointsDeVieRestauresEntreVagues();

        Heros.Soigner(pointsDeVieARestaurer);

        int pointsDeVieSoignes = Heros.PointsDeVie - pointsDeVieAvantSoin;

        _indexVagueCourante++;

        _eventPublisher.Publier(new CombatEvent(
            $"La vague {numeroVagueTerminee} est terminée."));

        _eventPublisher.Publier(new CombatEvent(
            $"{Heros.Nom} récupère {pointsDeVieSoignes} points de vie avant la vague {VagueCourante.Numero}."));

        ChangerEtat(new TourJoueurState());
    }

    private void DefinirEtatApresActionJoueur()
    {
        if (Heros.EstVaincu)
        {
            ChangerEtat(new DefaiteState());
            return;
        }

        if (VagueCourante.EstTerminee)
        {
            ChangerEtat(EstDerniereVague
                ? new VictoireState()
                : new EntreVaguesState());

            return;
        }

        ChangerEtat(new TourEnnemiState());
    }

    private int CalculerPointsDeVieRestauresEntreVagues()
    {
        return (int)Math.Ceiling(
            Heros.PointsDeVieMaximum * CombatRules.PourcentageRestaurationEntreVagues / 100.0);
    }
}
