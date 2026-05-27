namespace JeuCombat.Domain.Entites;

public abstract class Personnage
{
    protected Personnage(
        string nom,
        int pointsDeVieMaximum,
        int attaqueBase,
        int armure = 0)
    {
        if (string.IsNullOrWhiteSpace(nom))
        {
            throw new ArgumentException("Le nom du personnage est obligatoire.", nameof(nom));
        }

        if (pointsDeVieMaximum <= 0)
        {
            throw new ArgumentException("Les points de vie maximum doivent être positifs.", nameof(pointsDeVieMaximum));
        }

        if (attaqueBase < 0)
        {
            throw new ArgumentException("L'attaque de base ne peut pas être négative.", nameof(attaqueBase));
        }

        if (armure < 0)
        {
            throw new ArgumentException("L'armure ne peut pas être négative.", nameof(armure));
        }

        Nom = nom;
        PointsDeVieMaximum = pointsDeVieMaximum;
        PointsDeVie = pointsDeVieMaximum;
        AttaqueBase = attaqueBase;
        Armure = armure;
    }

    public string Nom { get; }

    public int PointsDeVieMaximum { get; }

    public int PointsDeVie { get; private set; }

    public int AttaqueBase { get; }

    public int Armure { get; }

    public bool EstVaincu => PointsDeVie <= 0;

    public void RecevoirDegats(int degats)
    {
        if (degats < 0)
        {
            throw new ArgumentException("Les dégâts ne peuvent pas être négatifs.", nameof(degats));
        }

        PointsDeVie = Math.Max(0, PointsDeVie - degats);
    }

    public void Soigner(int pointsDeVie)
    {
        if (pointsDeVie < 0)
        {
            throw new ArgumentException("Le soin ne peut pas être négatif.", nameof(pointsDeVie));
        }

        PointsDeVie = Math.Min(PointsDeVieMaximum, PointsDeVie + pointsDeVie);
    }
}
