using Prototico.Core.Repositorio;

namespace Prototico.API.Identidade
{
    public class MeuServico : ServiceBase<Teste>
    {
        public MeuServico(DataServiceFactory dataServiceFactory) : base(dataServiceFactory)
        {
        }
    }
}
