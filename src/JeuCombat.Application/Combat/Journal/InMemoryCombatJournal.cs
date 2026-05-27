using JeuCombat.Application.Combat.Events;

namespace JeuCombat.Application.Combat.Journal;

public sealed class InMemoryCombatJournal : ICombatJournal, ICombatObserver
{
    private readonly List<string> _evenements = new();

    public void Ajouter(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        _evenements.Add(message);
    }

    public void Notifier(CombatEvent evenement)
    {
        Ajouter(evenement.Message);
    }

    public IReadOnlyList<string> RecupererDerniersEvenements(int nombreMaximum)
    {
        if (nombreMaximum <= 0)
        {
            return Array.Empty<string>();
        }

        return _evenements
            .TakeLast(nombreMaximum)
            .ToList()
            .AsReadOnly();
    }
}
