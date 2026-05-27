using JeuCombat.Application.Combat.Events;
using JeuCombat.Application.Combat.Journal;

namespace JeuCombat.Tests.Application.Combat.Events;

public sealed class CombatEventPublisherTests
{
    [Fact]
    public void Publier_NotifieLesObservateurs()
    {
        var publisher = new CombatEventPublisher();
        var journal = new InMemoryCombatJournal();

        publisher.AjouterObservateur(journal);

        publisher.Publier(new CombatEvent("Aria attaque Gobelin."));

        var evenements = journal.RecupererDerniersEvenements(10);

        Assert.Single(evenements);
        Assert.Equal("Aria attaque Gobelin.", evenements[0]);
    }

    [Fact]
    public void Publier_NotifiePlusieursObservateurs()
    {
        var publisher = new CombatEventPublisher();
        var premierJournal = new InMemoryCombatJournal();
        var deuxiemeJournal = new InMemoryCombatJournal();

        publisher.AjouterObservateur(premierJournal);
        publisher.AjouterObservateur(deuxiemeJournal);

        publisher.Publier(new CombatEvent("Gobelin est vaincu."));

        Assert.Single(premierJournal.RecupererDerniersEvenements(10));
        Assert.Single(deuxiemeJournal.RecupererDerniersEvenements(10));
    }
}
