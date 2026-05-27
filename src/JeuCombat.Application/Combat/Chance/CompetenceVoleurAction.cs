using JeuCombat.Application.Combat.Chance;
using JeuCombat.Domain.Constants;
using JeuCombat.Domain.Entites;

namespace JeuCombat.Application.Combat.Actions;

public sealed class CompetenceVoleurAction : ICombatAction
{
    private readonly IChanceProvider _chanceProvider;

    public CompetenceVoleurAction(IChanceProvider chanceProvider)
    {
        _chanceProvider = chanceProvider;
    }

    public string Nom => "Coup critique";

    public bool PeutExecuter(Heros heros, Ennemi? cible)
    {
        return heros.PeutUtiliserCompetence && cible is not null && !cible.EstVaincu;
    }

    public CombatActionResult Executer(Heros heros, Ennemi? cible)
    {
        if (!PeutExecuter(heros, cible))
        {
            return new CombatActionResult(false, "Le coup critique ne peut pas être exécuté.");
        }

        bool critiqueReussi = _chanceProvider.Reussir(CombatRules.VoleurChanceCritiquePourcentage);
        int attaque = critiqueReussi
            ? heros.AttaqueBase * CombatRules.VoleurMultiplicateurCritique
            : heros.AttaqueBase;

        int degats = Math.Max(0, attaque - cible!.Armure);

        cible.RecevoirDegats(degats);
        heros.DefinirCooldownCompetence(CombatRules.VoleurCooldownCompetence);

        string typeCoup = critiqueReussi ? "coup critique réussi" : "coup critique raté";
        string message = cible.EstVaincu
            ? $"{heros.Nom} tente un {typeCoup} sur {cible.Nom} et inflige {degats} dégâts. {cible.Nom} est vaincu."
            : $"{heros.Nom} tente un {typeCoup} sur {cible.Nom} et inflige {degats} dégâts.";

        return new CombatActionResult(true, message, DegatsInfliges: degats);
    }
}
