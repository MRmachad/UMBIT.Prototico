using System.Collections.Generic;
using System.Text.Json.Serialization;
using FluentValidation.Results;

namespace UMBIT.Prototico.Core.Recursos.Command
{
    public abstract class CommandResponse
    {
        [JsonIgnore]
        public ValidationResult ValidationResult { get; protected set; }

        private ICollection<string> Erros = new List<string>();

        public void AdicionarErro(string erro)
        {
            ValidationResult.Errors.Add(new ValidationFailure() { ErrorMessage = erro });
        }
    }
}
