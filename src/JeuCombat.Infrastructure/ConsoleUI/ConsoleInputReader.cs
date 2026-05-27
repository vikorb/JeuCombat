namespace JeuCombat.Infrastructure.ConsoleUI;

public sealed class ConsoleInputReader : IConsoleInputReader
{
    public string LireTexteObligatoire(string message)
    {
        while (true)
        {
            System.Console.Write(message);

            string? valeur = System.Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(valeur))
            {
                return valeur.Trim();
            }

            System.Console.WriteLine("La valeur est obligatoire.");
        }
    }

    public int LireEntier(string message)
    {
        while (true)
        {
            System.Console.Write(message);

            string? valeur = System.Console.ReadLine();

            if (int.TryParse(valeur, out int resultat))
            {
                return resultat;
            }

            System.Console.WriteLine("Veuillez saisir un nombre valide.");
        }
    }

    public void AttendreValidation()
    {
        System.Console.WriteLine();
        System.Console.WriteLine("Appuyez sur Entrée pour continuer...");
        System.Console.ReadLine();
    }
}
