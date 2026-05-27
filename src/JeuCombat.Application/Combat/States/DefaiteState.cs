using JeuCombat.Application.Combat.Sessions;

namespace JeuCombat.Application.Combat.States;

public sealed class DefaiteState : ICombatState
{
    public string Nom => "Défaite";

    public CombatStateType Type => CombatStateType.Defaite;

    public void Entrer(CombatSession session)
    {
        ArgumentNullException.ThrowIfNull(session);
    }

    public void Executer(CombatSession session)
    {
        ArgumentNullException.ThrowIfNull(session);
    }
}
