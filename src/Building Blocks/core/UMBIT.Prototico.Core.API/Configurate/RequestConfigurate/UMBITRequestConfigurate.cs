using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using UMBIT.Prototico.Core.API.Servico.Basicos;
using UMBIT.Prototico.Core.API.Servico.Interface;

namespace UMBIT.Prototico.Core.API.Configurate.RequestConfigurate
{
    public static class UMBITRequestConfigurate
    {
        public static void AddUMBITRequestConfigurate<TI, T>(this IServiceCollection services, IConfiguration configuration, string baseAdressSection)
            where TI : class, IServicoDeRequisicao
            where T : ServicoDeRequisicao, TI 
        {
            services.AddHttpClient<TI, T>()
                  .ConfigureHttpClient(c => c.BaseAddress = new Uri(configuration.GetSection(baseAdressSection).Value));

        }
    }
}
