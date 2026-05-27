using JeuCombat.Domain.Entites;

namespace JeuCombat.Application.Combat.Actions;

public interface ICombatAction
{
    string Nom { get; }

    bool PeutExecuter(Heros heros, Ennemi? cible);

    CombatActionResult Executer(Heros heros, Ennemi? cible);
}
