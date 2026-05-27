namespace JeuCombat.Application.Combat.Actions;

public sealed record CombatActionResult(
    bool EstReussi,
    string Message,
    int DegatsInfliges = 0,
    int PointsDeVieSoignes = 0);
