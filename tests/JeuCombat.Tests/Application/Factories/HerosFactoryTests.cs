using JeuCombat.Application.Factories;
using JeuCombat.Domain.Constants;
using JeuCombat.Domain.Enums;

namespace JeuCombat.Tests.Application.Factories;

public sealed class HerosFactoryTests
{
    [Fact]
    public void Creer_Guerrier_CreeUnGuerrierAvecLesBonnesStatistiques()
    {
        var factory = new HerosFactory();

        var heros = factory.Creer("Aria", ClasseHero.Guerrier);

        Assert.Equal("Aria", heros.Nom);
        Assert.Equal(ClasseHero.Guerrier, heros.Classe);
        Assert.Equal(CombatRules.GuerrierPointsDeVie, heros.PointsDeVieMaximum);
        Assert.Equal(CombatRules.GuerrierAttaqueBase, heros.AttaqueBase);
    }

    [Fact]
    public void Creer_Mage_CreeUnMageAvecLesBonnesStatistiques()
    {
        var factory = new HerosFactory();

        var heros = factory.Creer("Aria", ClasseHero.Mage);

        Assert.Equal(ClasseHero.Mage, heros.Classe);
        Assert.Equal(CombatRules.MagePointsDeVie, heros.PointsDeVieMaximum);
        Assert.Equal(CombatRules.MageAttaqueBase, heros.AttaqueBase);
    }

    [Fact]
    public void Creer_Voleur_CreeUnVoleurAvecLesBonnesStatistiques()
    {
        var factory = new HerosFactory();

        var heros = factory.Creer("Aria", ClasseHero.Voleur);

        Assert.Equal(ClasseHero.Voleur, heros.Classe);
        Assert.Equal(CombatRules.VoleurPointsDeVie, heros.PointsDeVieMaximum);
        Assert.Equal(CombatRules.VoleurAttaqueBase, heros.AttaqueBase);
    }

    [Fact]
    public void Creer_ClasseInconnue_LeveUneErreur()
    {
        var factory = new HerosFactory();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            factory.Creer("Aria", (ClasseHero)999));
    }
}
