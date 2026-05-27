using JeuCombat.Domain.Constants;
using JeuCombat.Domain.Entites;
using JeuCombat.Domain.Enums;

namespace JeuCombat.Tests.Domain;

public sealed class PersonnageTests
{
    [Fact]
    public void RecevoirDegats_ReduitLesPointsDeVie()
    {
        var heros = new Heros(
            "Aria",
            ClasseHero.Mage,
            CombatRules.MagePointsDeVie,
            CombatRules.MageAttaqueBase);

        heros.RecevoirDegats(20);

        Assert.Equal(60, heros.PointsDeVie);
    }

    [Fact]
    public void RecevoirDegats_NeDescendPasSousZero()
    {
        var heros = new Heros(
            "Aria",
            ClasseHero.Mage,
            CombatRules.MagePointsDeVie,
            CombatRules.MageAttaqueBase);

        heros.RecevoirDegats(999);

        Assert.Equal(0, heros.PointsDeVie);
        Assert.True(heros.EstVaincu);
    }

    [Fact]
    public void Soigner_NeDepassePasLesPointsDeVieMaximum()
    {
        var heros = new Heros(
            "Aria",
            ClasseHero.Mage,
            CombatRules.MagePointsDeVie,
            CombatRules.MageAttaqueBase);

        heros.RecevoirDegats(10);
        heros.Soigner(CombatRules.SoinPointsDeVie);

        Assert.Equal(CombatRules.MagePointsDeVie, heros.PointsDeVie);
    }

    [Fact]
    public void UtiliserSoin_DiminueLeNombreDeSoinsRestants()
    {
        var heros = new Heros(
            "Aria",
            ClasseHero.Mage,
            CombatRules.MagePointsDeVie,
            CombatRules.MageAttaqueBase);

        heros.RecevoirDegats(30);
        heros.UtiliserSoin(CombatRules.SoinPointsDeVie);

        Assert.Equal(1, heros.SoinsRestants);
        Assert.Equal(75, heros.PointsDeVie);
    }

    [Fact]
    public void UtiliserSoin_EchoueQuandAucunSoinRestant()
    {
        var heros = new Heros(
            "Aria",
            ClasseHero.Mage,
            CombatRules.MagePointsDeVie,
            CombatRules.MageAttaqueBase);

        heros.RecevoirDegats(50);
        heros.UtiliserSoin(CombatRules.SoinPointsDeVie);
        heros.UtiliserSoin(CombatRules.SoinPointsDeVie);

        Assert.Throws<InvalidOperationException>(() =>
            heros.UtiliserSoin(CombatRules.SoinPointsDeVie));
    }

    [Fact]
    public void ReduireCooldownCompetence_NeDescendPasSousZero()
    {
        var heros = new Heros(
            "Aria",
            ClasseHero.Mage,
            CombatRules.MagePointsDeVie,
            CombatRules.MageAttaqueBase);

        heros.ReduireCooldownCompetence();

        Assert.Equal(0, heros.CooldownCompetence);
    }

    [Fact]
    public void ReduireCooldownCompetence_ReduitLeCooldown()
    {
        var heros = new Heros(
            "Aria",
            ClasseHero.Mage,
            CombatRules.MagePointsDeVie,
            CombatRules.MageAttaqueBase);

        heros.DefinirCooldownCompetence(3);
        heros.ReduireCooldownCompetence();

        Assert.Equal(2, heros.CooldownCompetence);
    }
}
