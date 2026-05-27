namespace JeuCombat.Application.Combat.Commands;

public sealed class ActionInvoker
{
    private readonly IReadOnlyDictionary<int, ICommand> _commandes;

    public ActionInvoker(IReadOnlyDictionary<int, ICommand> commandes)
    {
        if (commandes.Count == 0)
        {
            throw new ArgumentException("Au moins une commande est obligatoire.", nameof(commandes));
        }

        _commandes = commandes;
    }

    public CombatCommandResult Executer(int choix)
    {
        if (!_commandes.TryGetValue(choix, out var commande))
        {
            return new CombatCommandResult(false, false, "Choix invalide.");
        }

        return commande.Executer();
    }
}
