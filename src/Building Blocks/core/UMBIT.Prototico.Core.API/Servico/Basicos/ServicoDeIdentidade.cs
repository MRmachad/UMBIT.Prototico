using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using UMBIT.Prototico.Core.API.Entidade;
using UMBIT.Prototico.Core.API.Servico.Interface;

namespace UMBIT.Prototico.Core.API.Servico.Basicos
{
    public abstract class ServicoDeIdentidade : IServicoDeIdentidade
    {
        public AuthResponse AuthResponse { get; set; }  
        public ServicoDeIdentidade()
        {
            this.AuthResponse = new AuthResponse();
        }
        public abstract Task<AuthResponse> AutenticaUsuarioAsync(string usuario, string senha);

        public abstract Task<AuthResponse> Cadastro(string usuario, string senha);

        public abstract Task<AuthResponse> Login(string usuario, string senha);

        public abstract Task<AuthResponse> Logout(string usuario, string senha);

    }
    


}
