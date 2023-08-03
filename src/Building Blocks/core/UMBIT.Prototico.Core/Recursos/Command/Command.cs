using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using FluentValidation.Results;
using MediatR;

namespace UMBIT.Prototico.Core.Recursos.Command
{
    public abstract class Command : Message, IRequest<ValidationResult>
    {
        public DateTime Timestamp { get; private set; }

        [JsonIgnore]
        public ValidationResult ValidationResult { get; protected set; }

        public Command()
        {
            Timestamp = DateTime.Now;
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
            Timestamp = DateTime.Now;
        }

        public abstract bool EhValido();
    }

    
}
