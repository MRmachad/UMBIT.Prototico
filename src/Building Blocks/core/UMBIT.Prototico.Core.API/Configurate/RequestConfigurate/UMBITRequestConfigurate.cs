using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMBIT.Prototico.Core.API.Servico.Basicos;
using UMBIT.Prototico.Core.API.Servico.Interface;

namespace UMBIT.Prototico.Core.API.Configurate.RequestConfigurate
{
    public static class UMBITRequestConfigurate
    {
        public static void AddUMBITRequestConfigurate<IT, T>(this IServiceCollection services, string baseAdress, string nameHTTPClient) where T : ServicoDeRequisicao where IT : IServicoDeRequisicao
        {

                services.AddHttpClient<IT,T>(nameHTTPClient,  (configHTTPClient) => {
                            configHTTPClient.BaseAddress = new Uri(baseAdress);

            });
        }
    }
}
