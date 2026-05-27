using JeuCombat.Domain.Constants;
using JeuCombat.Domain.Entites;
using JeuCombat.Domain.Enums;

namespace JeuCombat.Application.Factories;

public sealed class EnnemiFactory : IEnnemiFactory
{
    public Ennemi Creer(TypeEnnemi type)
    {
        return type switch
        {
            TypeEnnemi.Gobelin => new Ennemi(
                "Gobelin",
                type,
                CombatRules.GobelinPointsDeVie,
                CombatRules.GobelinAttaqueBase,
                CombatRules.GobelinArmure),

            TypeEnnemi.GobelinArcher => new Ennemi(
                "Gobelin archer",
                type,
                CombatRules.GobelinArcherPointsDeVie,
                CombatRules.GobelinArcherAttaqueBase,
                CombatRules.GobelinArcherArmure),

            TypeEnnemi.BossOrc => new Ennemi(
                "Chef orc",
                type,
                CombatRules.BossOrcPointsDeVie,
                CombatRules.BossOrcAttaqueBase,
                CombatRules.BossOrcArmure),

            _ => throw new ArgumentOutOfRangeException(
                nameof(type),
                type,
                "Type d'ennemi inconnu.")
        };
    }
}
