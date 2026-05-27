using JeuCombat.Application.Combat.Sessions;
using JeuCombat.Domain.Entites;

namespace JeuCombat.Infrastructure.ConsoleUI;

public interface IConsoleRenderer
{
    void AfficherAccueil();

    void AfficherClassesHero();

    void AfficherEtatCombat(CombatSession session);

    void AfficherMenuActions(Heros heros);

    void AfficherCibles(IReadOnlyList<Ennemi> ennemis);

    void AfficherMessage(string message);

    void AfficherErreur(string message);

    void AfficherFin(CombatSession session);
}
