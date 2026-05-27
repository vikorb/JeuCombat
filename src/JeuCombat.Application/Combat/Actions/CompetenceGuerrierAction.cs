using JeuCombat.Domain.Constants;
using JeuCombat.Domain.Entites;

namespace JeuCombat.Application.Combat.Actions;

public sealed class CompetenceGuerrierAction : ICombatAction
{
    public string Nom => "Frappe lourde";

    public bool PeutExecuter(Heros heros, Ennemi? cible)
    {
        return heros.PeutUtiliserCompetence && cible is not null && !cible.EstVaincu;
    }

    public CombatActionResult Executer(Heros heros, Ennemi? cible)
    {
        if (!PeutExecuter(heros, cible))
        {
            return new CombatActionResult(false, "La frappe lourde ne peut pas être exécutée.");
        }

        int attaqueRenforcee = (int)Math.Round(
            heros.AttaqueBase * CombatRules.GuerrierMultiplicateurFrappeLourde);

        int degats = Math.Max(0, attaqueRenforcee - cible!.Armure);

        cible.RecevoirDegats(degats);
        heros.DefinirCooldownCompetence(CombatRules.GuerrierCooldownCompetence);

        string message = cible.EstVaincu
            ? $"{heros.Nom} utilise Frappe lourde sur {cible.Nom} et inflige {degats} dégâts. {cible.Nom} est vaincu."
            : $"{heros.Nom} utilise Frappe lourde sur {cible.Nom} et inflige {degats} dégâts.";

        return new CombatActionResult(true, message, DegatsInfliges: degats);
    }
}
