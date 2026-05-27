namespace JeuCombat.Infrastructure.ConsoleUI;

public interface IConsoleInputReader
{
    string LireTexteObligatoire(string message);

    int LireEntier(string message);

    void AttendreValidation();
}
