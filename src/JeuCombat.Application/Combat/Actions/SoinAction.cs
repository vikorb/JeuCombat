using JeuCombat.Domain.Constants;
using JeuCombat.Domain.Entites;

namespace JeuCombat.Application.Combat.Actions;

public sealed class SoinAction : ICombatAction
{
    public string Nom => "Se soigner";

    public bool PeutExecuter(Heros heros, Ennemi? cible)
    {
        return heros.PeutSeSoigner;
    }

    public CombatActionResult Executer(Heros heros, Ennemi? cible)
    {
        if (!PeutExecuter(heros, cible))
        {
            return new CombatActionResult(false, "Le héros ne peut plus se soigner.");
        }

        int pointsDeVieAvantSoin = heros.PointsDeVie;

        heros.UtiliserSoin(CombatRules.SoinPointsDeVie);

        int pointsDeVieSoignes = heros.PointsDeVie - pointsDeVieAvantSoin;
        string message = $"{heros.Nom} récupère {pointsDeVieSoignes} points de vie.";

        return new CombatActionResult(
            true,
            message,
            PointsDeVieSoignes: pointsDeVieSoignes);
    }
}
