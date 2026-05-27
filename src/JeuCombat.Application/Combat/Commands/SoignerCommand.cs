using JeuCombat.Application.Combat.Actions;
using JeuCombat.Application.Combat.Events;
using JeuCombat.Domain.Entites;

namespace JeuCombat.Application.Combat.Commands;

public sealed class SoignerCommand : ICommand
{
    private readonly ICombatAction _action;
    private readonly Heros _heros;
    private readonly ICombatEventPublisher _eventPublisher;

    public SoignerCommand(
        ICombatAction action,
        Heros heros,
        ICombatEventPublisher eventPublisher)
    {
        _action = action;
        _heros = heros;
        _eventPublisher = eventPublisher;
    }

    public string Nom => "Se soigner";

    public CombatCommandResult Executer()
    {
        var resultat = _action.Executer(_heros, null);

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
