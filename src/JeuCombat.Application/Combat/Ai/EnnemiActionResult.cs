namespace JeuCombat.Application.Combat.Ai;

public sealed record EnnemiActionResult(
    bool EstReussi,
    string Message,
    int DegatsInfliges = 0);
