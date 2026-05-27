using JeuCombat.Application.Combat.Actions;
using JeuCombat.Application.Combat.Commands;
using JeuCombat.Application.Combat.Events;
using JeuCombat.Application.Combat.Journal;
using JeuCombat.Domain.Constants;
using JeuCombat.Domain.Entites;
using JeuCombat.Domain.Enums;

namespace JeuCombat.Tests.Application.Combat.Commands;

public sealed class CommandTests
{
    [Fact]
    public void AttaquerCommand_ExecuteLAttaqueEtPublieUnEvenement()
    {
        var heros = CreerMage();
        var ennemi = CreerGobelin();
        var journal = new InMemoryCombatJournal();
        var publisher = CreerPublisherAvecJournal(journal);

        var command = new AttaquerCommand(
            new AttaqueBasiqueAction(),
            heros,
            ennemi,
            publisher);

        var resultat = command.Executer();

        Assert.True(resultat.EstReussi);
        Assert.True(resultat.TermineLeTour);
        Assert.Equal(30, ennemi.PointsDeVie);
        Assert.Single(journal.RecupererDerniersEvenements(10));
    }

    [Fact]
    public void UtiliserCompetenceCommand_ExecuteLaCompetenceEtPublieUnEvenement()
    {
        var heros = CreerMage();
        var ennemi = CreerGobelin();
        var journal = new InMemoryCombatJournal();
        var publisher = CreerPublisherAvecJournal(journal);

        var command = new UtiliserCompetenceCommand(
            new CompetenceMageAction(),
            heros,
            ennemi,
            publisher);

        var resultat = command.Executer();

        Assert.True(resultat.EstReussi);
        Assert.True(resultat.TermineLeTour);
        Assert.Equal(11, ennemi.PointsDeVie);
        Assert.Equal(CombatRules.MageCooldownCompetence, heros.CooldownCompetence);
        Assert.Single(journal.RecupererDerniersEvenements(10));
    }

    [Fact]
    public void SoignerCommand_SoigneLeHerosEtPublieUnEvenement()
    {
        var heros = CreerMage();
        var journal = new InMemoryCombatJournal();
        var publisher = CreerPublisherAvecJournal(journal);

        heros.RecevoirDegats(30);

        var command = new SoignerCommand(
            new SoinAction(),
            heros,
            publisher);

        var resultat = command.Executer();

        Assert.True(resultat.EstReussi);
        Assert.True(resultat.TermineLeTour);
        Assert.Equal(75, heros.PointsDeVie);
        Assert.Single(journal.RecupererDerniersEvenements(10));
    }

    [Fact]
    public void AfficherJournalCommand_RetourneLesDerniersEvenementsSansTerminerLeTour()
    {
        var journal = new InMemoryCombatJournal();

        journal.Ajouter("Premier événement");
        journal.Ajouter("Deuxième événement");

        var command = new AfficherJournalCommand(journal);

        var resultat = command.Executer();

        Assert.True(resultat.EstReussi);
        Assert.False(resultat.TermineLeTour);
        Assert.Contains("Premier événement", resultat.Message);
        Assert.Contains("Deuxième événement", resultat.Message);
    }

    [Fact]
    public void AfficherJournalCommand_RetourneUnMessageQuandLeJournalEstVide()
    {
        var journal = new InMemoryCombatJournal();
        var command = new AfficherJournalCommand(journal);

        var resultat = command.Executer();

        Assert.True(resultat.EstReussi);
        Assert.False(resultat.TermineLeTour);
        Assert.Equal("Aucun événement dans le journal.", resultat.Message);
    }

    [Fact]
    public void ActionInvoker_ExecuteLaCommandeAssocieeAuChoix()
    {
        var heros = CreerMage();
        var ennemi = CreerGobelin();
        var journal = new InMemoryCombatJournal();
        var publisher = CreerPublisherAvecJournal(journal);

        var invoker = new ActionInvoker(
            new Dictionary<int, ICommand>
            {
                [1] = new AttaquerCommand(
                    new AttaqueBasiqueAction(),
                    heros,
                    ennemi,
                    publisher)
            });

        var resultat = invoker.Executer(1);

        Assert.True(resultat.EstReussi);
        Assert.True(resultat.TermineLeTour);
        Assert.Equal(30, ennemi.PointsDeVie);
    }

    [Fact]
    public void ActionInvoker_RetourneUneErreurQuandLeChoixEstInvalide()
    {
        var invoker = new ActionInvoker(
            new Dictionary<int, ICommand>
            {
                [1] = new AfficherJournalCommand(new InMemoryCombatJournal())
            });

        var resultat = invoker.Executer(99);

        Assert.False(resultat.EstReussi);
        Assert.False(resultat.TermineLeTour);
        Assert.Equal("Choix invalide.", resultat.Message);
    }

    private static CombatEventPublisher CreerPublisherAvecJournal(InMemoryCombatJournal journal)
    {
        var publisher = new CombatEventPublisher();

        publisher.AjouterObservateur(journal);

        return publisher;
    }

    private static Heros CreerMage()
    {
        return new Heros(
            "Aria",
            ClasseHero.Mage,
            CombatRules.MagePointsDeVie,
            CombatRules.MageAttaqueBase);
    }

    private static Ennemi CreerGobelin()
    {
        return new Ennemi(
            "Gobelin",
            TypeEnnemi.Gobelin,
            CombatRules.GobelinPointsDeVie,
            CombatRules.GobelinAttaqueBase,
            CombatRules.GobelinArmure);
    }
}
