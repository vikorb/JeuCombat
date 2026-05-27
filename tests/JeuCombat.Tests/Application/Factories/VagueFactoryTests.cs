using JeuCombat.Application.Factories;
using JeuCombat.Domain.Enums;

namespace JeuCombat.Tests.Application.Factories;

public sealed class VagueFactoryTests
{
    [Fact]
    public void CreerToutesLesVagues_RetourneTroisVagues()
    {
        var factory = CreerFactory();

        var vagues = factory.CreerToutesLesVagues();

        Assert.Equal(3, vagues.Count);
        Assert.Equal(1, vagues[0].Numero);
        Assert.Equal(2, vagues[1].Numero);
        Assert.Equal(3, vagues[2].Numero);
    }

    [Fact]
    public void Creer_VagueUn_CreeUneVagueAvecUnGobelin()
    {
        var factory = CreerFactory();

        var vague = factory.Creer(1);

        Assert.Equal(1, vague.Numero);
        Assert.Single(vague.Ennemis);
        Assert.Equal(TypeEnnemi.Gobelin, vague.Ennemis[0].Type);
    }

    [Fact]
    public void Creer_VagueDeux_CreeUneVagueAvecDeuxEnnemis()
    {
        var factory = CreerFactory();

        var vague = factory.Creer(2);

        Assert.Equal(2, vague.Numero);
        Assert.Equal(2, vague.Ennemis.Count);
        Assert.Contains(vague.Ennemis, ennemi => ennemi.Type == TypeEnnemi.Gobelin);
        Assert.Contains(vague.Ennemis, ennemi => ennemi.Type == TypeEnnemi.GobelinArcher);
    }

    [Fact]
    public void Creer_VagueTrois_CreeUneVagueAvecUnBoss()
    {
        var factory = CreerFactory();

        var vague = factory.Creer(3);

        Assert.Equal(3, vague.Numero);
        Assert.Single(vague.Ennemis);
        Assert.Equal(TypeEnnemi.BossOrc, vague.Ennemis[0].Type);
    }

    [Fact]
    public void Creer_NumeroInconnu_LeveUneErreur()
    {
        var factory = CreerFactory();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            factory.Creer(999));
    }

    private static VagueFactory CreerFactory()
    {
        return new VagueFactory(new EnnemiFactory());
    }
}
