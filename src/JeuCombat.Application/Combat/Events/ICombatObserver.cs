namespace JeuCombat.Application.Combat.Events;

public interface ICombatObserver
{
    void Notifier(CombatEvent evenement);
}
