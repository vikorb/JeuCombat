using JeuCombat.Application.Combat.Ai;
using JeuCombat.Domain.Constants;
using JeuCombat.Domain.Entites;
using JeuCombat.Domain.Enums;

namespace JeuCombat.Tests.Application.Combat.Ai;

public sealed class AttaqueSimpleEnnemiAiStrategyTests
{
    [Fact]
    public void Executer_QuandEnnemiVivant_InfligeSesDegatsAuHeros()
    {
        var heros = CreerMage();
        var ennemi = CreerGobelin();
        var strategy = new AttaqueSimpleEnnemiAiStrategy();

        var resultat = strategy.Executer(ennemi, heros);

        Assert.True(resultat.EstReussi);
        Assert.Equal(CombatRules.GobelinAttaqueBase, resultat.DegatsInfliges);
        Assert.Equal(72, heros.PointsDeVie);
    }

    [Fact]
    public void Executer_QuandEnnemiVaincu_NInfligePasDeDegats()
    {
        var heros = CreerMage();
        var ennemi = CreerGobelin();
        var strategy = new AttaqueSimpleEnnemiAiStrategy();

        ennemi.RecevoirDegats(999);

        var resultat = strategy.Executer(ennemi, heros);

        Assert.False(resultat.EstReussi);
        Assert.Equal(CombatRules.MagePointsDeVie, heros.PointsDeVie);
    }

    [Fact]
    public void Executer_QuandHerosVaincu_NInfligePasDeDegats()
    {
        var heros = CreerMage();
        var ennemi = CreerGobelin();
        var strategy = new AttaqueSimpleEnnemiAiStrategy();

        heros.RecevoirDegats(999);

        var resultat = strategy.Executer(ennemi, heros);

        Assert.False(resultat.EstReussi);
        Assert.Equal(0, heros.PointsDeVie);
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
