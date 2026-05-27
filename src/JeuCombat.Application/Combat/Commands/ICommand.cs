namespace JeuCombat.Application.Combat.Commands;

public interface ICommand
{
    string Nom { get; }

    CombatCommandResult Executer();
}
