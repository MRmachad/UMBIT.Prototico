using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using UMBIT.Core.Repositorio;
using UMBIT.Core.Repositorio.Contexto;

namespace Prototico.Core.API.Configurate.ApiConfigurate
{
    public static class UMBITConfigureAPICore
    {
        public static void AddUMBITServiceMySQL(this IServiceCollection services, IConfiguration configuration)
        {
            var conexao = configuration.GetSection("ConnectionString").Value ?? "";
            services.AddDbContext<DbContext, DataContext>(options => options.UseMySql(conexao, ServerVersion.AutoDetect(conexao), b => b.MigrationsAssembly(Assembly.GetCallingAssembly().GetName().Name)));
            services.AddScoped<IDataServiceFactory, DataServiceFactory>();
        }
    }
}
