using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using UMBIT.Prototico.Core.API.Servico.RestAPI;
using UMBIT.Prototico.Core.API.Servico.RestAPI.Facade.SystemNet;

namespace UMBIT.Prototico.Core.API.Configurate
{
    public static class UMBITConfigureRequest
    {
        public static void AddUMBITRequestConfigurate<TInterface, TImplement>(this IServiceCollection services, IConfiguration configuration, string baseAdressSection)
            where TInterface : class, IServicoDeRequisicao
            where TImplement : ServicoDeRequisicao, TInterface
        {
            services.AddHttpClient<TInterface, TImplement>()
                  .ConfigureHttpClient(c => c.BaseAddress = new Uri(configuration.GetSection(baseAdressSection).Value));

        }
    }
}
