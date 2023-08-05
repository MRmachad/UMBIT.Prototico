using System.Threading.Tasks;
using UMBIT.Prototico.Core.API.Model;

namespace UMBIT.Prototico.Core.API.Servico.Identidade
{
    public interface IServicoDeIdentidade
    {
        Task<AuthResponse> Login(string usuario, string senha);
        Task<AuthResponse> Logout(string usuario, string senha);
        Task<AuthResponse> Cadastro(string usuario, string senha);
        Task<AuthResponse> AutenticaUsuarioAsync(string usuario, string senha);
    }
}
