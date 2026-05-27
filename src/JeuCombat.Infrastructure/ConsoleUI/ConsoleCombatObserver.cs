using JeuCombat.Application.Combat.Events;

namespace JeuCombat.Infrastructure.ConsoleUI;

public sealed class ConsoleCombatObserver : ICombatObserver
{
    public void Notifier(CombatEvent evenement)
    {
        ArgumentNullException.ThrowIfNull(evenement);

        if (!string.IsNullOrWhiteSpace(evenement.Message))
        {
            System.Console.WriteLine(evenement.Message);
        }
    }
}
