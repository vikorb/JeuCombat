namespace JeuCombat.Application.Combat.Events;

public interface ICombatEventPublisher
{
    void AjouterObservateur(ICombatObserver observateur);

    void Publier(CombatEvent evenement);
}
