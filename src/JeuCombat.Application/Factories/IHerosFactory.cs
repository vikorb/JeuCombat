using JeuCombat.Domain.Entites;
using JeuCombat.Domain.Enums;

namespace JeuCombat.Application.Factories;

public interface IHerosFactory
{
    Heros Creer(string nom, ClasseHero classe);
}
