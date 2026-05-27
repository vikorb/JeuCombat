using JeuCombat.Application.Combat.Actions;
using JeuCombat.Application.Combat.Events;
using JeuCombat.Domain.Entites;

namespace JeuCombat.Application.Combat.Commands;

public sealed class UtiliserCompetenceCommand : ICommand
{
    private readonly ICombatAction _action;
    private readonly Heros _heros;
    private readonly Ennemi _cible;
    private readonly ICombatEventPublisher _eventPublisher;

    public UtiliserCompetenceCommand(
        ICombatAction action,
        Heros heros,
        Ennemi cible,
        ICombatEventPublisher eventPublisher)
    {
        _action = action;
        _heros = heros;
        _cible = cible;
        _eventPublisher = eventPublisher;
    }

    public string Nom => "Utiliser une compétence";

    public CombatCommandResult Executer()
    {
        var resultat = _action.Executer(_heros, _cible);

        if (resultat.EstReussi)
        {
            _eventPublisher.Publier(new CombatEvent(resultat.Message));
        }

        return new CombatCommandResult(
            resultat.EstReussi,
            resultat.EstReussi,
            resultat.Message);
    }
}
