using FluentValidation.Results;
using UMBIT.Prototico.Core.API.Entidade;

namespace UMBIT.Prototico.Core.API.Model
{
    public class AuthResponse : ValidationResult
    {
        public TokenJWT tokenJWT { get; set; }
    }
}
