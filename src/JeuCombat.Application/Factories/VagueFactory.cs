using JeuCombat.Domain.Entites;
using JeuCombat.Domain.Enums;

namespace JeuCombat.Application.Factories;

public sealed class VagueFactory : IVagueFactory
{
    private readonly IEnnemiFactory _ennemiFactory;

    public VagueFactory(IEnnemiFactory ennemiFactory)
    {
        _ennemiFactory = ennemiFactory;
    }

    public IReadOnlyList<Vague> CreerToutesLesVagues()
    {
        return new List<Vague>
        {
            Creer(1),
            Creer(2),
            Creer(3)
        };
    }

    public Vague Creer(int numero)
    {
        return numero switch
        {
            1 => new Vague(
                1,
                new List<Ennemi>
                {
                    _ennemiFactory.Creer(TypeEnnemi.Gobelin)
                }),

            2 => new Vague(
                2,
                new List<Ennemi>
                {
                    _ennemiFactory.Creer(TypeEnnemi.Gobelin),
                    _ennemiFactory.Creer(TypeEnnemi.GobelinArcher)
                }),

            3 => new Vague(
                3,
                new List<Ennemi>
                {
                    _ennemiFactory.Creer(TypeEnnemi.BossOrc)
                }),

            _ => throw new ArgumentOutOfRangeException(
                nameof(numero),
                numero,
                "Numéro de vague inconnu.")
        };
    }
}
