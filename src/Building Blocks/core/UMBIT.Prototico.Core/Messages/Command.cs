using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace UMBIT.Core.Messages
{
    public abstract class Command : Message, IRequest<ValidationResult>
    {
        public DateTime Timestamp { get; private set; }

        [JsonIgnore]
        public ValidationResult ValidationResult { get; protected set; }

        public Command()
        {
            this.Timestamp = DateTime.Now;
        }

        public abstract bool EhValido();
    }

    public abstract class Command<T> : Message, IRequest<T>
        where T : CommandResponse
    {
        public DateTime Timestamp { get; private set; }

        [JsonIgnore]
        public ValidationResult ValidationResult { get; protected set; }

        public Command()
        {
            this.Timestamp = DateTime.Now;
        }

        public abstract bool EhValido();
    }

    public abstract class CommandResponse
    {
        [JsonIgnore]
        public ValidationResult ValidationResult { get; protected set; }

        private ICollection<string> Erros = new List<string>();

        public void AdicionarErro(string erro)
        {
            this.ValidationResult.Errors.Add(new ValidationFailure() { ErrorMessage = erro});
        }
    }
}
