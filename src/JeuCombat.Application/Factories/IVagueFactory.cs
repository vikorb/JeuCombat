using JeuCombat.Domain.Entites;

namespace JeuCombat.Application.Factories;

public interface IVagueFactory
{
    IReadOnlyList<Vague> CreerToutesLesVagues();

    Vague Creer(int numero);
}
