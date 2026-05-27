using JeuCombat.Domain.Entites;
using JeuCombat.Domain.Enums;

namespace JeuCombat.Application.Factories;

public interface IEnnemiFactory
{
    Ennemi Creer(TypeEnnemi type);
}
