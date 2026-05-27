using JeuCombat.Infrastructure.ConsoleUI;

IConsoleInputReader inputReader = new ConsoleInputReader();
IConsoleRenderer renderer = new ConsoleRenderer();

var game = new ConsoleGame(inputReader, renderer);

game.Lancer();
