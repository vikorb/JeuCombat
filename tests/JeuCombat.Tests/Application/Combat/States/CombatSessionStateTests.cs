using JeuCombat.Application.Combat.Actions;
using JeuCombat.Application.Combat.Ai;
using JeuCombat.Application.Combat.Commands;
using JeuCombat.Application.Combat.Events;
using JeuCombat.Application.Combat.Journal;
using JeuCombat.Application.Combat.Sessions;
using JeuCombat.Application.Combat.States;
using JeuCombat.Domain.Constants;
using JeuCombat.Domain.Entites;
using JeuCombat.Domain.Enums;

namespace JeuCombat.Tests.Application.Combat.States;

public sealed class CombatSessionStateTests
{
    [Fact]
    public void NouvelleSession_CommenceAuTourDuJoueur()
    {
        var session = CreerSessionAvecUneVague();

        Assert.Equal(CombatStateType.TourJoueur, session.EtatCourant.Type);
        Assert.Equal(1, session.NumeroVagueCourante);
        Assert.False(session.EstTerminee);
    }

    [Fact]
    public void ExecuterCommandeJoueur_QuandEnnemiSurvit_PasseAuTourEnnemi()
    {
        var session = CreerSessionAvecUneVague();
        var publisher = CreerPublisher(out _);

        var command = new AttaquerCommand(
            new AttaqueBasiqueAction(),
            session.Heros,
            session.VagueCourante.Ennemis[0],
            publisher);

        session.ExecuterCommandeJoueur(command);

        Assert.Equal(CombatStateType.TourEnnemi, session.EtatCourant.Type);
    }

    [Fact]
    public void TourEnnemi_UtiliseLaStrategieAiInjectee()
    {
        var heros = new Heros(
            "Aria",
            ClasseHero.Mage,
            CombatRules.MagePointsDeVie,
            CombatRules.MageAttaqueBase);

        var ennemi = new Ennemi(
            "Gobelin",
            TypeEnnemi.Gobelin,
            CombatRules.GobelinPointsDeVie,
            CombatRules.GobelinAttaqueBase,
            CombatRules.GobelinArmure);

        var vague = new Vague(1, new List<Ennemi> { ennemi });
        var strategy = new FakeEnnemiAiStrategy();
        var session = new CombatSession(
            heros,
            new List<Vague> { vague },
            CreerPublisher(out _),
            strategy);

        session.ChangerEtat(new TourEnnemiState());
        session.ExecuterEtatCourant();

        Assert.True(strategy.AEteAppelee);
        Assert.Equal(73, heros.PointsDeVie);
    }

    [Fact]
    public void TourEnnemi_QuandHerosSurvit_RepasseAuTourJoueur()
    {
        var session = CreerSessionAvecUneVague();
        var publisher = CreerPublisher(out _);

        var command = new AttaquerCommand(
            new AttaqueBasiqueAction(),
            session.Heros,
            session.VagueCourante.Ennemis[0],
            publisher);

        session.ExecuterCommandeJoueur(command);
        session.ExecuterEtatCourant();

        Assert.Equal(CombatStateType.TourJoueur, session.EtatCourant.Type);
        Assert.True(session.Heros.PointsDeVie < session.Heros.PointsDeVieMaximum);
    }

    [Fact]
    public void TourEnnemi_QuandHerosMeurt_PasseEnDefaite()
    {
        var heros = new Heros(
            "Aria",
            ClasseHero.Mage,
            CombatRules.MagePointsDeVie,
            CombatRules.MageAttaqueBase);

        heros.RecevoirDegats(75);

        var ennemi = new Ennemi("Brute", TypeEnnemi.BossOrc, 50, 10, 0);
        var vague = new Vague(1, new List<Ennemi> { ennemi });
        var session = new CombatSession(heros, new List<Vague> { vague }, CreerPublisher(out _));

        session.ChangerEtat(new TourEnnemiState());
        session.ExecuterEtatCourant();

        Assert.Equal(CombatStateType.Defaite, session.EtatCourant.Type);
        Assert.True(session.EstTerminee);
    }

    [Fact]
    public void ExecuterCommandeJoueur_QuandVagueTermineeEtCeNestPasLaDerniere_PasseEntreVagues()
    {
        var session = CreerSessionAvecDeuxVaguesEtEnnemiFaible();
        var publisher = CreerPublisher(out _);

        var command = new AttaquerCommand(
            new AttaqueBasiqueAction(),
            session.Heros,
            session.VagueCourante.Ennemis[0],
            publisher);

        session.ExecuterCommandeJoueur(command);

        Assert.Equal(CombatStateType.EntreVagues, session.EtatCourant.Type);
        Assert.False(session.EstTerminee);
    }

    [Fact]
    public void EntreVagues_RestaureLesPointsDeVieEtPasseALaVagueSuivante()
    {
        var session = CreerSessionAvecDeuxVaguesEtEnnemiFaible();
        var publisher = CreerPublisher(out var journal);

        session.Heros.RecevoirDegats(50);

        var command = new AttaquerCommand(
            new AttaqueBasiqueAction(),
            session.Heros,
            session.VagueCourante.Ennemis[0],
            publisher);

        session.ExecuterCommandeJoueur(command);
        session.ExecuterEtatCourant();

        Assert.Equal(CombatStateType.TourJoueur, session.EtatCourant.Type);
        Assert.Equal(2, session.NumeroVagueCourante);
        Assert.Equal(46, session.Heros.PointsDeVie);
        Assert.NotEmpty(journal.RecupererDerniersEvenements(10));
    }

    [Fact]
    public void ExecuterCommandeJoueur_QuandDerniereVagueTerminee_PasseEnVictoire()
    {
        var heros = new Heros(
            "Aria",
            ClasseHero.Mage,
            CombatRules.MagePointsDeVie,
            CombatRules.MageAttaqueBase);

        var ennemi = new Ennemi("Ennemi faible", TypeEnnemi.Gobelin, 5, 1, 0);
        var vague = new Vague(1, new List<Ennemi> { ennemi });
        var session = new CombatSession(heros, new List<Vague> { vague }, CreerPublisher(out _));
        var publisher = CreerPublisher(out _);

        var command = new AttaquerCommand(
            new AttaqueBasiqueAction(),
            session.Heros,
            session.VagueCourante.Ennemis[0],
            publisher);

        session.ExecuterCommandeJoueur(command);

        Assert.Equal(CombatStateType.Victoire, session.EtatCourant.Type);
        Assert.True(session.EstTerminee);
    }

    [Fact]
    public void ExecuterCommandeJoueur_QuandCeNestPasLeTourJoueur_RetourneUneErreur()
    {
        var session = CreerSessionAvecUneVague();

        session.ChangerEtat(new TourEnnemiState());

        var command = new AfficherJournalCommand(new InMemoryCombatJournal());

        var resultat = session.ExecuterCommandeJoueur(command);

        Assert.False(resultat.EstReussi);
        Assert.False(resultat.TermineLeTour);
        Assert.Equal("Ce n'est pas le tour du joueur.", resultat.Message);
    }

    private static CombatSession CreerSessionAvecUneVague()
    {
        var heros = new Heros(
            "Aria",
            ClasseHero.Mage,
            CombatRules.MagePointsDeVie,
            CombatRules.MageAttaqueBase);

        var ennemi = new Ennemi(
            "Gobelin",
            TypeEnnemi.Gobelin,
            CombatRules.GobelinPointsDeVie,
            CombatRules.GobelinAttaqueBase,
            CombatRules.GobelinArmure);

        var vague = new Vague(1, new List<Ennemi> { ennemi });

        return new CombatSession(heros, new List<Vague> { vague }, CreerPublisher(out _));
    }

    private static CombatSession CreerSessionAvecDeuxVaguesEtEnnemiFaible()
    {
        var heros = new Heros(
            "Aria",
            ClasseHero.Mage,
            CombatRules.MagePointsDeVie,
            CombatRules.MageAttaqueBase);

        var premiereVague = new Vague(
            1,
            new List<Ennemi>
            {
                new("Ennemi faible", TypeEnnemi.Gobelin, 5, 1, 0)
            });

        var deuxiemeVague = new Vague(
            2,
            new List<Ennemi>
            {
                new("Gobelin", TypeEnnemi.Gobelin, 40, 8, 2)
            });

        return new CombatSession(
            heros,
            new List<Vague> { premiereVague, deuxiemeVague },
            CreerPublisher(out _));
    }

    private static CombatEventPublisher CreerPublisher(out InMemoryCombatJournal journal)
    {
        journal = new InMemoryCombatJournal();

        var publisher = new CombatEventPublisher();

        publisher.AjouterObservateur(journal);

        return publisher;
    }

    private sealed class FakeEnnemiAiStrategy : IEnnemiAiStrategy
    {
        public bool AEteAppelee { get; private set; }

        public EnnemiActionResult Executer(Ennemi ennemi, Heros cible)
        {
            AEteAppelee = true;

            cible.RecevoirDegats(7);

            return new EnnemiActionResult(
                true,
                $"{ennemi.Nom} attaque {cible.Nom} et inflige 7 dégâts.",
                7);
        }
    }
}
