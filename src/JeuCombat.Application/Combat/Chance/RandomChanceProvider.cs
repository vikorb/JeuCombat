namespace JeuCombat.Application.Combat.Chance;

public sealed class RandomChanceProvider : IChanceProvider
{
    public bool Reussir(int pourcentage)
    {
        if (pourcentage < 0 || pourcentage > 100)
        {
            throw new ArgumentOutOfRangeException(
                nameof(pourcentage),
                pourcentage,
                "Le pourcentage doit être compris entre 0 et 100.");
        }

        return Random.Shared.Next(1, 101) <= pourcentage;
    }
}
