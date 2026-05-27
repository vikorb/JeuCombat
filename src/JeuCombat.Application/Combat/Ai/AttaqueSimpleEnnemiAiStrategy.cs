using JeuCombat.Domain.Entites;

namespace JeuCombat.Application.Combat.Ai;

public sealed class AttaqueSimpleEnnemiAiStrategy : IEnnemiAiStrategy
{
    public EnnemiActionResult Executer(Ennemi ennemi, Heros cible)
    {
        ArgumentNullException.ThrowIfNull(ennemi);
        ArgumentNullException.ThrowIfNull(cible);

        if (ennemi.EstVaincu)
        {
            return new EnnemiActionResult(false, $"{ennemi.Nom} ne peut pas attaquer car il est vaincu.");
        }

        if (cible.EstVaincu)
        {
            return new EnnemiActionResult(false, $"{ennemi.Nom} ne peut pas attaquer car {cible.Nom} est déjà vaincu.");
        }

        int degats = ennemi.AttaqueBase;

        cible.RecevoirDegats(degats);

        return new EnnemiActionResult(
            true,
            $"{ennemi.Nom} attaque {cible.Nom} et inflige {degats} dégâts.",
            degats);
    }
}
