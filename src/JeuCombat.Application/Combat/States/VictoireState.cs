using JeuCombat.Application.Combat.Sessions;

namespace JeuCombat.Application.Combat.States;

public sealed class VictoireState : ICombatState
{
    public string Nom => "Victoire";

    public CombatStateType Type => CombatStateType.Victoire;

    public void Entrer(CombatSession session)
    {
        ArgumentNullException.ThrowIfNull(session);
    }

    public void Executer(CombatSession session)
    {
        ArgumentNullException.ThrowIfNull(session);
    }
}
