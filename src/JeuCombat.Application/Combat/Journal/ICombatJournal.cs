namespace JeuCombat.Application.Combat.Journal;

public interface ICombatJournal
{
    void Ajouter(string message);

    IReadOnlyList<string> RecupererDerniersEvenements(int nombreMaximum);
}
