using JeuCombat.Domain.Enums;

namespace JeuCombat.Domain.Entites;

public sealed class Ennemi : Personnage
{
    public Ennemi(
        string nom,
        TypeEnnemi type,
        int pointsDeVieMaximum,
        int attaqueBase,
        int armure)
        : base(nom, pointsDeVieMaximum, attaqueBase, armure)
    {
        Type = type;
    }

    public TypeEnnemi Type { get; }
}
