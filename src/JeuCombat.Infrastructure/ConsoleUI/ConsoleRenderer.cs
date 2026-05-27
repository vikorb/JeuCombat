using JeuCombat.Application.Combat.Sessions;
using JeuCombat.Application.Combat.States;
using JeuCombat.Domain.Entites;

namespace JeuCombat.Infrastructure.ConsoleUI;

public sealed class ConsoleRenderer : IConsoleRenderer
{
    public void AfficherAccueil()
    {
        System.Console.WriteLine("══════════════════════════════════════════");
        System.Console.WriteLine("           JEU DE COMBAT CLI");
        System.Console.WriteLine("══════════════════════════════════════════");
        System.Console.WriteLine();
    }

    public void AfficherClassesHero()
    {
        System.Console.WriteLine("Choisissez une classe :");
        System.Console.WriteLine("1. Guerrier");
        System.Console.WriteLine("2. Mage");
        System.Console.WriteLine("3. Voleur");
        System.Console.WriteLine();
    }

    public void AfficherEtatCombat(CombatSession session)
    {
        System.Console.WriteLine();
        System.Console.WriteLine("══════════════════════════════════════════");
        System.Console.WriteLine($"  VAGUE {session.NumeroVagueCourante}/{session.Vagues.Count} — {session.EtatCourant.Nom}");
        System.Console.WriteLine("══════════════════════════════════════════");

        System.Console.WriteLine(
            $"  Héros : {session.Heros.Nom} ({session.Heros.Classe})     PV : {session.Heros.PointsDeVie}/{session.Heros.PointsDeVieMaximum}");

        System.Console.WriteLine($"  Cooldown compétence : {session.Heros.CooldownCompetence} tour(s)");
        System.Console.WriteLine($"  Soins restants : {session.Heros.SoinsRestants}");
        System.Console.WriteLine();

        System.Console.WriteLine("  Ennemis :");

        for (int index = 0; index < session.VagueCourante.Ennemis.Count; index++)
        {
            var ennemi = session.VagueCourante.Ennemis[index];
            string statut = ennemi.EstVaincu ? "vaincu" : $"{ennemi.PointsDeVie}/{ennemi.PointsDeVieMaximum} PV";

            System.Console.WriteLine($"    [{index + 1}] {ennemi.Nom} — {statut}");
        }

        System.Console.WriteLine();
    }

    public void AfficherMenuActions(Heros heros)
    {
        System.Console.WriteLine("  Actions :");
        System.Console.WriteLine("    1. Attaque de base");
        System.Console.WriteLine($"    2. Compétence de classe ({heros.Classe})");
        System.Console.WriteLine("    3. Se soigner");
        System.Console.WriteLine("    4. Afficher le journal");
        System.Console.WriteLine();
    }

    public void AfficherCibles(IReadOnlyList<Ennemi> ennemis)
    {
        System.Console.WriteLine("Choisissez une cible :");

        for (int index = 0; index < ennemis.Count; index++)
        {
            var ennemi = ennemis[index];

            System.Console.WriteLine(
                $"{index + 1}. {ennemi.Nom} — {ennemi.PointsDeVie}/{ennemi.PointsDeVieMaximum} PV");
        }

        System.Console.WriteLine();
    }

    public void AfficherMessage(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        System.Console.WriteLine(message);
    }

    public void AfficherErreur(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        System.Console.WriteLine($"Erreur : {message}");
    }

    public void AfficherFin(CombatSession session)
    {
        System.Console.WriteLine();
        System.Console.WriteLine("══════════════════════════════════════════");

        if (session.EtatCourant.Type == CombatStateType.Victoire)
        {
            System.Console.WriteLine("              VICTOIRE !");
        }
        else
        {
            System.Console.WriteLine("              DÉFAITE !");
        }

        System.Console.WriteLine("══════════════════════════════════════════");
        System.Console.WriteLine();
    }
}
