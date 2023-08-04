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
        }

        public async Task<ValidationResult> EnviarComando<T>(T comando) where T : Command.Command
        {
            return await _mediator.Send(comando);
        }

        public async Task<TRes> EnviarComando<T, TRes>(T comando)
            where T : Command<TRes>
            where TRes : CommandResponse
        {
            return await _mediator.Send(comando);
        }


    }
}
