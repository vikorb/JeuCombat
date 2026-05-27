using JeuCombat.Application.Combat.Actions;
using JeuCombat.Application.Combat.Chance;
using JeuCombat.Domain.Constants;
using JeuCombat.Domain.Entites;
using JeuCombat.Domain.Enums;

namespace JeuCombat.Tests.Application.Combat.Actions;

public sealed class CombatActionTests
{
    [Fact]
    public void AttaqueBasique_ReduitLesPointsDeVieEnRespectantArmure()
    {
        var heros = CreerMage();
        var ennemi = CreerGobelin();
        var action = new AttaqueBasiqueAction();

        var resultat = action.Executer(heros, ennemi);

        Assert.True(resultat.EstReussi);
        Assert.Equal(10, resultat.DegatsInfliges);
        Assert.Equal(30, ennemi.PointsDeVie);
    }

    [Fact]
    public void SoinAction_SoigneLeHerosEtConsommeUnSoin()
    {
        var heros = CreerMage();
        var action = new SoinAction();

        heros.RecevoirDegats(30);

        var resultat = action.Executer(heros, null);

        Assert.True(resultat.EstReussi);
        Assert.Equal(30, resultat.PointsDeVieSoignes);
        Assert.Equal(80, heros.PointsDeVie);
        Assert.Equal(1, heros.SoinsRestants);
    }

    [Fact]
    public void CompetenceGuerrier_InfligeDesDegatsEtDeclencheLeCooldown()
    {
        var heros = new Heros(
            "Aria",
            ClasseHero.Guerrier,
            CombatRules.GuerrierPointsDeVie,
            CombatRules.GuerrierAttaqueBase);

        var ennemi = CreerGobelin();
        var action = new CompetenceGuerrierAction();

        var resultat = action.Executer(heros, ennemi);

        Assert.True(resultat.EstReussi);
        Assert.Equal(25, resultat.DegatsInfliges);
        Assert.Equal(15, ennemi.PointsDeVie);
        Assert.Equal(CombatRules.GuerrierCooldownCompetence, heros.CooldownCompetence);
    }

    [Fact]
    public void CompetenceMage_IgnoreUnePartieDeLArmureEtDeclencheLeCooldown()
    {
        var heros = CreerMage();

        var boss = new Ennemi(
            "Chef orc",
            TypeEnnemi.BossOrc,
            CombatRules.BossOrcPointsDeVie,
            CombatRules.BossOrcAttaqueBase,
            CombatRules.BossOrcArmure);

        var action = new CompetenceMageAction();

        var resultat = action.Executer(heros, boss);

        Assert.True(resultat.EstReussi);
        Assert.Equal(29, resultat.DegatsInfliges);
        Assert.Equal(71, boss.PointsDeVie);
        Assert.Equal(CombatRules.MageCooldownCompetence, heros.CooldownCompetence);
    }

    [Fact]
    public void CompetenceVoleur_QuandCritiqueReussit_DoubleLesDegats()
    {
        var heros = CreerVoleur();
        var ennemi = CreerGobelin();
        var action = new CompetenceVoleurAction(new FakeChanceProvider(true));

        var resultat = action.Executer(heros, ennemi);

        Assert.True(resultat.EstReussi);
        Assert.Equal(26, resultat.DegatsInfliges);
        Assert.Equal(14, ennemi.PointsDeVie);
        Assert.Equal(CombatRules.VoleurCooldownCompetence, heros.CooldownCompetence);
    }

    [Fact]
    public void CompetenceVoleur_QuandCritiqueEchoue_InfligeLesDegatsNormaux()
    {
        var heros = CreerVoleur();
        var ennemi = CreerGobelin();
        var action = new CompetenceVoleurAction(new FakeChanceProvider(false));

        var resultat = action.Executer(heros, ennemi);

        Assert.True(resultat.EstReussi);
        Assert.Equal(12, resultat.DegatsInfliges);
        Assert.Equal(28, ennemi.PointsDeVie);
        Assert.Equal(CombatRules.VoleurCooldownCompetence, heros.CooldownCompetence);
    }

    [Fact]
    public void CompetenceEnCooldown_NePeutPasEtreExecutee()
    {
        var heros = CreerMage();
        var ennemi = CreerGobelin();
        var action = new CompetenceMageAction();

        heros.DefinirCooldownCompetence(1);

        var resultat = action.Executer(heros, ennemi);

        Assert.False(resultat.EstReussi);
        Assert.Equal(CombatRules.GobelinPointsDeVie, ennemi.PointsDeVie);
    }

    private static Heros CreerMage()
    {
        return new Heros(
            "Aria",
            ClasseHero.Mage,
            CombatRules.MagePointsDeVie,
            CombatRules.MageAttaqueBase);
    }

    private static Heros CreerVoleur()
    {
        return new Heros(
            "Shade",
            ClasseHero.Voleur,
            CombatRules.VoleurPointsDeVie,
            CombatRules.VoleurAttaqueBase);
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

    private sealed class FakeChanceProvider : IChanceProvider
    {
        private readonly bool _resultat;

        public FakeChanceProvider(bool resultat)
        {
            _resultat = resultat;
        }

        public bool Reussir(int pourcentage)
        {
            return _resultat;
        }
    }
}
