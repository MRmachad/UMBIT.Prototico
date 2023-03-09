using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using UMBIT.Prototico.Core.API.Entidade;

namespace UMBIT.Prototico.Core.API.Servico.Interface
{
    internal interface IServicoDeIdentidade
    {
        Task<AuthResponse> Login(string usuario, string senha);
        Task<AuthResponse> Logout(string usuario, string senha);
        Task<AuthResponse> Cadastro(string usuario, string senha);
        Task<AuthResponse> AutenticaUsuarioAsync(string usuario, string senha);
    }

    public class AuthResponse : ValidationResult
    {
        public TokenJWT tokenJWT { get; set; }
    }
}
