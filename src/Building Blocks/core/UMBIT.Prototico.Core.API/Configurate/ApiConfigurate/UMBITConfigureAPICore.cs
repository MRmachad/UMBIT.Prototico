using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Reflection;
using UMBIT.Core.Repositorio;
using UMBIT.Core.Repositorio.Contexto;

namespace Prototico.Core.API.Configurate.ApiConfigurate
{
    public static class UMBITConfigureAPICore
    {
        public static void AddUMBITServiceMySQL(this IServiceCollection services, IConfiguration configuration)
        {

            StackTrace stackTrace = new StackTrace();
            var nameApi = stackTrace.GetFrame(1).GetMethod().DeclaringType.Assembly.GetName().Name;

            var conexao = configuration.GetSection("ConnectionString").Value ?? "";
            services.AddDbContext<DbContext, DataContext>(options => options.UseMySql(conexao, ServerVersion.AutoDetect(conexao), b => b.MigrationsAssembly(nameApi)));
            services.AddScoped<IUnidadeDeTrabalho, UnidadeDeTrabalho>();
        }
    }
}
