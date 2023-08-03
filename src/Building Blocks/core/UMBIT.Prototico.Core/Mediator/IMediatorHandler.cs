using FluentValidation.Results;
using System.Threading.Tasks;
using UMBIT.Prototico.Core.Recursos.Command;

namespace UMBIT.Core.Mediator
{
    public interface IMediatorHandler
    {
        Task<ValidationResult> EnviarComando<T>(T comando) where T : Command;
        Task<TRes> EnviarComando<T, TRes>(T comando) 
            where T : Command<TRes> 
            where TRes : CommandResponse;
        Task EnviaMensageRabbitMQ<T>(T evento) where T : class;
    }
}
