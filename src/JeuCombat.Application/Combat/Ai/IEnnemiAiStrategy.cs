using JeuCombat.Domain.Entites;

namespace JeuCombat.Application.Combat.Ai;

public interface IEnnemiAiStrategy
{
    EnnemiActionResult Executer(Ennemi ennemi, Heros cible);
}
