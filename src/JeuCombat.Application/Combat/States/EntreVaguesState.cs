using JeuCombat.Application.Combat.Sessions;

namespace JeuCombat.Application.Combat.States;

public sealed class EntreVaguesState : ICombatState
{
    public string Nom => "Entre deux vagues";

    public CombatStateType Type => CombatStateType.EntreVagues;

    public void Entrer(CombatSession session)
    {
        ArgumentNullException.ThrowIfNull(session);
    }

    public void Executer(CombatSession session)
    {
        ArgumentNullException.ThrowIfNull(session);

        session.PasserALaVagueSuivante();
    }
}
