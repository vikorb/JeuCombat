using JeuCombat.Domain.Entites;

namespace JeuCombat.Application.Combat.Actions;

public sealed class AttaqueBasiqueAction : ICombatAction
{
    public string Nom => "Attaque de base";

    public bool PeutExecuter(Heros heros, Ennemi? cible)
    {
        return !heros.EstVaincu && cible is not null && !cible.EstVaincu;
    }

    public CombatActionResult Executer(Heros heros, Ennemi? cible)
    {
        if (!PeutExecuter(heros, cible))
        {
            return new CombatActionResult(false, "L'attaque de base ne peut pas être exécutée.");
        }

        int degats = Math.Max(0, heros.AttaqueBase - cible!.Armure);
        cible.RecevoirDegats(degats);

        string message = cible.EstVaincu
            ? $"{heros.Nom} attaque {cible.Nom} et inflige {degats} dégâts. {cible.Nom} est vaincu."
            : $"{heros.Nom} attaque {cible.Nom} et inflige {degats} dégâts.";

        return new CombatActionResult(true, message, DegatsInfliges: degats);
    }
}
