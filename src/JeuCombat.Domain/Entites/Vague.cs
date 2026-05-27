namespace JeuCombat.Domain.Entites;

public sealed class Vague
{
    public Vague(int numero, IReadOnlyList<Ennemi> ennemis)
    {
        if (numero <= 0)
        {
            throw new ArgumentException("Le numéro de vague doit être positif.", nameof(numero));
        }

        if (ennemis.Count == 0)
        {
            throw new ArgumentException("Une vague doit contenir au moins un ennemi.", nameof(ennemis));
        }

        Numero = numero;
        Ennemis = ennemis.ToList().AsReadOnly();
    }

    public int Numero { get; }

    public IReadOnlyList<Ennemi> Ennemis { get; }

    public bool EstTerminee => Ennemis.All(ennemi => ennemi.EstVaincu);
}
