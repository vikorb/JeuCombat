using JeuCombat.Application.Combat.Sessions;

namespace JeuCombat.Application.Combat.States;

public interface ICombatState
{
    string Nom { get; }

    CombatStateType Type { get; }

    void Entrer(CombatSession session);

    void Executer(CombatSession session);
}
