using JeuCombat.Application.Combat.Actions;
using JeuCombat.Application.Combat.Commands;
using JeuCombat.Application.Combat.Events;
using JeuCombat.Application.Combat.Journal;
using JeuCombat.Application.Combat.Sessions;
using JeuCombat.Application.Combat.States;
using JeuCombat.Domain.Constants;
using JeuCombat.Domain.Entites;
using JeuCombat.Domain.Enums;

namespace JeuCombat.Tests.Application.Combat.Sessions;

public sealed class CombatSessionFullScenarioTests
{
    [Fact]
    public void SessionComplete_PeutAllerJusquaLaVictoire()
    {
        var heros = CreerMage();

        var vagues = new List<Vague>
        {
            CreerVagueFaible(1),
            CreerVagueFaible(2),
            CreerVagueFaible(3)
        };

        var publisher = CreerPublisher(out var journal);
        var session = new CombatSession(heros, vagues, publisher);

        TuerEnnemiCourant(session, publisher);
        session.ExecuterEtatCourant();

        TuerEnnemiCourant(session, publisher);
        session.ExecuterEtatCourant();

        TuerEnnemiCourant(session, publisher);

        Assert.Equal(CombatStateType.Victoire, session.EtatCourant.Type);
        Assert.True(session.EstTerminee);
        Assert.NotEmpty(journal.RecupererDerniersEvenements(20));
    }

    [Fact]
    public void SessionComplete_PeutAllerJusquaLaDefaite()
    {
        var heros = CreerMage();
        heros.RecevoirDegats(75);

        var ennemi = new Ennemi(
            "Brute",
            TypeEnnemi.BossOrc,
            100,
            10,
            0);

        var vague = new Vague(1, new List<Ennemi> { ennemi });
        var publisher = CreerPublisher(out var journal);
        var session = new CombatSession(heros, new List<Vague> { vague }, publisher);

        var command = new AttaquerCommand(
            new AttaqueBasiqueAction(),
            session.Heros,
            session.VagueCourante.Ennemis[0],
            publisher);

        session.ExecuterCommandeJoueur(command);
        session.ExecuterEtatCourant();

        Assert.Equal(CombatStateType.Defaite, session.EtatCourant.Type);
        Assert.True(session.EstTerminee);
        Assert.True(session.Heros.EstVaincu);
        Assert.NotEmpty(journal.RecupererDerniersEvenements(20));
    }

    [Fact]
    public void EntreVagues_LaRestaurationNeDepassePasLesPointsDeVieMaximum()
    {
        var heros = CreerMage();
        heros.RecevoirDegats(5);

        var vagues = new List<Vague>
        {
            CreerVagueFaible(1),
            CreerVagueFaible(2)
        };

        var publisher = CreerPublisher(out _);
        var session = new CombatSession(heros, vagues, publisher);

        TuerEnnemiCourant(session, publisher);
        session.ExecuterEtatCourant();

        Assert.Equal(CombatStateType.TourJoueur, session.EtatCourant.Type);
        Assert.Equal(2, session.NumeroVagueCourante);
        Assert.Equal(CombatRules.MagePointsDeVie, session.Heros.PointsDeVie);
    }

    private static void TuerEnnemiCourant(
        CombatSession session,
        ICombatEventPublisher publisher)
    {
        var command = new AttaquerCommand(
            new AttaqueBasiqueAction(),
            session.Heros,
            session.VagueCourante.Ennemis[0],
            publisher);

        session.ExecuterCommandeJoueur(command);
    }

    private static Heros CreerMage()
    {
        return new Heros(
            "Aria",
            ClasseHero.Mage,
            CombatRules.MagePointsDeVie,
            CombatRules.MageAttaqueBase);
    }

    private static Vague CreerVagueFaible(int numero)
    {
        return new Vague(
            numero,
            new List<Ennemi>
            {
                new("Ennemi faible", TypeEnnemi.Gobelin, 5, 1, 0)
            });
    }

    private static CombatEventPublisher CreerPublisher(out InMemoryCombatJournal journal)
    {
        journal = new InMemoryCombatJournal();

        var publisher = new CombatEventPublisher();
        publisher.AjouterObservateur(journal);

        return publisher;
    }
}
