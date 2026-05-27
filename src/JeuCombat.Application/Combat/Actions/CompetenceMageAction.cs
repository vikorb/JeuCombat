using JeuCombat.Domain.Constants;
using JeuCombat.Domain.Entites;

namespace JeuCombat.Application.Combat.Actions;

public sealed class CompetenceMageAction : ICombatAction
{
    public string Nom => "Éclair";

    public bool PeutExecuter(Heros heros, Ennemi? cible)
    {
        return heros.PeutUtiliserCompetence && cible is not null && !cible.EstVaincu;
    }

    public CombatActionResult Executer(Heros heros, Ennemi? cible)
    {
        if (!PeutExecuter(heros, cible))
        {
            return new CombatActionResult(false, "L'éclair ne peut pas être exécuté.");
        }

        int armureIgnoree = (int)Math.Ceiling(cible!.Armure * CombatRules.MagePourcentageArmureIgnoree / 100.0);
        int armureEffective = Math.Max(0, cible.Armure - armureIgnoree);
        int degats = Math.Max(0, CombatRules.MageDegatsEclair - armureEffective);

        cible.RecevoirDegats(degats);
        heros.DefinirCooldownCompetence(CombatRules.MageCooldownCompetence);

        string message = cible.EstVaincu
            ? $"{heros.Nom} lance Éclair sur {cible.Nom} et inflige {degats} dégâts. {cible.Nom} est vaincu."
            : $"{heros.Nom} lance Éclair sur {cible.Nom} et inflige {degats} dégâts.";

        return new CombatActionResult(true, message, DegatsInfliges: degats);
    }
}
