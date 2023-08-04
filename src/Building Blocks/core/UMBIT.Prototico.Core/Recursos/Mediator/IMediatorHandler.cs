using FluentValidation.Results;
using System.Threading.Tasks;
using UMBIT.Prototico.Core.Recursos.Command;

namespace UMBIT.Prototico.Core.Recursos.Mediator
{
    public interface IMediatorHandler
    {
        Task<ValidationResult> EnviarComando<T>(T comando) where T : Command.Command;
        Task<TRes> EnviarComando<T, TRes>(T comando)
            where T : Command<TRes>
            where TRes : CommandResponse;
    }
}
