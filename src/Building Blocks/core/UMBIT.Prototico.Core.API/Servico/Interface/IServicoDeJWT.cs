using System.Threading.Tasks;
using UMBIT.Prototico.Core.API.Entidade;

namespace UMBIT.Prototico.Core.API.Servico.Interface
{
    public interface IServicoDeJWT
    {
        public Task<TokenJWT> GetToken(string user, string secret, double expiracaoMins, string validoEm, string emissor);
    }
}
