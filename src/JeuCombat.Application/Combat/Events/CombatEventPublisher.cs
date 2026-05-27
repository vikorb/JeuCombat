namespace JeuCombat.Application.Combat.Events;

public sealed class CombatEventPublisher : ICombatEventPublisher
{
    private readonly List<ICombatObserver> _observateurs = new();

    public void AjouterObservateur(ICombatObserver observateur)
    {
        ArgumentNullException.ThrowIfNull(observateur);

        _observateurs.Add(observateur);
    }

    public void Publier(CombatEvent evenement)
    {
        ArgumentNullException.ThrowIfNull(evenement);

        foreach (var observateur in _observateurs)
        {
            observateur.Notifier(evenement);
        }
    }
}
