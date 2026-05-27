using JeuCombat.Application.Combat.Journal;

namespace JeuCombat.Application.Combat.Commands;

public sealed class AfficherJournalCommand : ICommand
{
    private const int NombreEvenementsAAfficher = 10;

    private readonly ICombatJournal _journal;

    public AfficherJournalCommand(ICombatJournal journal)
    {
        _journal = journal;
    }

    public string Nom => "Afficher le journal";

    public CombatCommandResult Executer()
    {
        var evenements = _journal.RecupererDerniersEvenements(NombreEvenementsAAfficher);

        if (evenements.Count == 0)
        {
            return new CombatCommandResult(
                true,
                false,
                "Aucun événement dans le journal.");
        }

        string message = string.Join(Environment.NewLine, evenements);

        return new CombatCommandResult(true, false, message);
    }
}
