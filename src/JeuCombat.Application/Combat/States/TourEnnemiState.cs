using JeuCombat.Application.Combat.Sessions;

namespace JeuCombat.Application.Combat.States;

public sealed class TourEnnemiState : ICombatState
{
    public string Nom => "Tour ennemi";

    public CombatStateType Type => CombatStateType.TourEnnemi;

    public void Entrer(CombatSession session)
    {
        ArgumentNullException.ThrowIfNull(session);
    }

    public void Executer(CombatSession session)
    {
        ArgumentNullException.ThrowIfNull(session);

        session.ExecuterTourDesEnnemis();

        session.ChangerEtat(session.Heros.EstVaincu
            ? new DefaiteState()
            : new TourJoueurState());
    }
}
