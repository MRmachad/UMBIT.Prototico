using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;

namespace UMBIT.Prototico.Core.Recursos.Command
{
    public abstract class CommandHandler<T> : IRequestHandler<T, ValidationResult> where T : Command
    {
        protected ValidationResult ValidationResult;

        protected CommandHandler()
        {
            ValidationResult = new ValidationResult();
        }

        public abstract Task<ValidationResult> Handle(T request, CancellationToken cancellationToken);

        protected void AdicionarErro(string mensagem)
        {
            ValidationResult.Errors.Add(new ValidationFailure(string.Empty, mensagem));
        }

    }
    public abstract class CommandHandler<T, TRes> : IRequestHandler<T, TRes> where T : Command<TRes> where TRes : CommandResponse
    {
        protected ValidationResult ValidationResult;

        protected CommandHandler()
        {
            ValidationResult = new ValidationResult();
        }

        public abstract Task<TRes> Handle(T request, CancellationToken cancellationToken);

        protected void AdicionarErro(string mensagem)
        {
            ValidationResult.Errors.Add(new ValidationFailure(string.Empty, mensagem));
        }

    }
}
