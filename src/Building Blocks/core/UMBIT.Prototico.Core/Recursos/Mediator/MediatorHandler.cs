using FluentValidation.Results;
using MediatR;
using System.Threading.Tasks;
using UMBIT.Prototico.Core.Recursos.Command;

namespace UMBIT.Prototico.Core.Recursos.Mediator
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task<ValidationResult> EnviarComando<T>(T comando) where T : Command.Command
        {
            return await _mediator.Send(comando);
        }

        public async Task<TResult> EnviarComando<T, TResult>(T comando)
            where T : Command<TResult>
            where TResult : CommandResponse
        {
            return await _mediator.Send<TResult>(comando);
        }

        public void PubliqueEvento<T>(T _event) where T : Event.Event
        {
            this._mediator.Publish(_event);
        }
    }
}
