namespace JeuCombat.Application.Combat.Commands;

public sealed record CombatCommandResult(
    bool EstReussi,
    bool TermineLeTour,
    string Message);
