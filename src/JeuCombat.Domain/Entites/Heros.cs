using JeuCombat.Domain.Constants;
using JeuCombat.Domain.Enums;

namespace JeuCombat.Domain.Entites;

public sealed class Heros : Personnage
{
    public Heros(
        string nom,
        ClasseHero classe,
        int pointsDeVieMaximum,
        int attaqueBase)
        : base(nom, pointsDeVieMaximum, attaqueBase)
    {
        Classe = classe;
        SoinsRestants = CombatRules.NombreMaximumSoins;
        CooldownCompetence = 0;
    }

    public ClasseHero Classe { get; }

    public int SoinsRestants { get; private set; }

    public int CooldownCompetence { get; private set; }

    public bool PeutSeSoigner => SoinsRestants > 0 && !EstVaincu;

    public bool PeutUtiliserCompetence => CooldownCompetence == 0 && !EstVaincu;

    public void UtiliserSoin(int pointsDeVie)
    {
        if (!PeutSeSoigner)
        {
            throw new InvalidOperationException("Le héros ne peut plus se soigner.");
        }

        Soigner(pointsDeVie);
        SoinsRestants--;
    }

    public void DefinirCooldownCompetence(int nombreDeTours)
    {
        if (nombreDeTours < 0)
        {
            throw new ArgumentException("Le cooldown ne peut pas être négatif.", nameof(nombreDeTours));
        }

        CooldownCompetence = nombreDeTours;
    }

    public void ReduireCooldownCompetence()
    {
        if (CooldownCompetence > 0)
        {
            CooldownCompetence--;
        }
    }
}
