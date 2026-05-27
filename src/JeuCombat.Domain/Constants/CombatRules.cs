namespace JeuCombat.Domain.Constants;

public static class CombatRules
{
    public const int PourcentageRestaurationEntreVagues = 20;
    public const int SoinPointsDeVie = 25;
    public const int NombreMaximumSoins = 2;

    public const int GuerrierPointsDeVie = 120;
    public const int GuerrierAttaqueBase = 18;
    public const int GuerrierCooldownCompetence = 2;
    public const double GuerrierMultiplicateurFrappeLourde = 1.5;

    public const int MagePointsDeVie = 80;
    public const int MageAttaqueBase = 12;
    public const int MageCooldownCompetence = 3;
    public const int MageDegatsEclair = 30;
    public const int MagePourcentageArmureIgnoree = 50;

    public const int VoleurPointsDeVie = 90;
    public const int VoleurAttaqueBase = 14;
    public const int VoleurCooldownCompetence = 2;
    public const int VoleurChanceCritiquePourcentage = 30;
    public const int VoleurMultiplicateurCritique = 2;

    public const int GobelinPointsDeVie = 40;
    public const int GobelinAttaqueBase = 8;
    public const int GobelinArmure = 2;

    public const int GobelinArcherPointsDeVie = 35;
    public const int GobelinArcherAttaqueBase = 10;
    public const int GobelinArcherArmure = 1;

    public const int BossOrcPointsDeVie = 150;
    public const int BossOrcAttaqueBase = 18;
    public const int BossOrcArmure = 5;
}
