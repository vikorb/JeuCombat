using JeuCombat.Application.Factories;
using JeuCombat.Domain.Constants;
using JeuCombat.Domain.Enums;

namespace JeuCombat.Tests.Application.Factories;

public sealed class EnnemiFactoryTests
{
    [Fact]
    public void Creer_Gobelin_CreeUnGobelinAvecLesBonnesStatistiques()
    {
        var factory = new EnnemiFactory();

        var ennemi = factory.Creer(TypeEnnemi.Gobelin);

        Assert.Equal("Gobelin", ennemi.Nom);
        Assert.Equal(TypeEnnemi.Gobelin, ennemi.Type);
        Assert.Equal(CombatRules.GobelinPointsDeVie, ennemi.PointsDeVieMaximum);
        Assert.Equal(CombatRules.GobelinAttaqueBase, ennemi.AttaqueBase);
        Assert.Equal(CombatRules.GobelinArmure, ennemi.Armure);
    }

    [Fact]
    public void Creer_GobelinArcher_CreeUnGobelinArcherAvecLesBonnesStatistiques()
    {
        var factory = new EnnemiFactory();

        var ennemi = factory.Creer(TypeEnnemi.GobelinArcher);

        Assert.Equal("Gobelin archer", ennemi.Nom);
        Assert.Equal(TypeEnnemi.GobelinArcher, ennemi.Type);
        Assert.Equal(CombatRules.GobelinArcherPointsDeVie, ennemi.PointsDeVieMaximum);
        Assert.Equal(CombatRules.GobelinArcherAttaqueBase, ennemi.AttaqueBase);
        Assert.Equal(CombatRules.GobelinArcherArmure, ennemi.Armure);
    }

    [Fact]
    public void Creer_BossOrc_CreeUnBossAvecLesBonnesStatistiques()
    {
        var factory = new EnnemiFactory();

        var ennemi = factory.Creer(TypeEnnemi.BossOrc);

        Assert.Equal("Chef orc", ennemi.Nom);
        Assert.Equal(TypeEnnemi.BossOrc, ennemi.Type);
        Assert.Equal(CombatRules.BossOrcPointsDeVie, ennemi.PointsDeVieMaximum);
        Assert.Equal(CombatRules.BossOrcAttaqueBase, ennemi.AttaqueBase);
        Assert.Equal(CombatRules.BossOrcArmure, ennemi.Armure);
    }

    [Fact]
    public void Creer_TypeInconnu_LeveUneErreur()
    {
        var factory = new EnnemiFactory();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            factory.Creer((TypeEnnemi)999));
    }
}
