using JeuCombat.Application.Combat.Sessions;

namespace JeuCombat.Application.Combat.States;

public sealed class TourJoueurState : ICombatState
{
    public string Nom => "Tour du joueur";

    public CombatStateType Type => CombatStateType.TourJoueur;

    public void Entrer(CombatSession session)
    {
        ArgumentNullException.ThrowIfNull(session);
    }

    public void Executer(CombatSession session)
    {
        ArgumentNullException.ThrowIfNull(session);
    }
}
