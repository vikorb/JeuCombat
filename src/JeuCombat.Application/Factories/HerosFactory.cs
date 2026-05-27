using JeuCombat.Domain.Constants;
using JeuCombat.Domain.Entites;
using JeuCombat.Domain.Enums;

namespace JeuCombat.Application.Factories;

public sealed class HerosFactory : IHerosFactory
{
    public Heros Creer(string nom, ClasseHero classe)
    {
        return classe switch
        {
            ClasseHero.Guerrier => new Heros(
                nom,
                classe,
                CombatRules.GuerrierPointsDeVie,
                CombatRules.GuerrierAttaqueBase),

            ClasseHero.Mage => new Heros(
                nom,
                classe,
                CombatRules.MagePointsDeVie,
                CombatRules.MageAttaqueBase),

            ClasseHero.Voleur => new Heros(
                nom,
                classe,
                CombatRules.VoleurPointsDeVie,
                CombatRules.VoleurAttaqueBase),

            _ => throw new ArgumentOutOfRangeException(
                nameof(classe),
                classe,
                "Classe de héros inconnue.")
        };
    }
}
