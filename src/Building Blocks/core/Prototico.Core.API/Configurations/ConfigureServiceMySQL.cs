using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using UMBIT.Core.Repositorio;
using UMBIT.Core.Repositorio.Contexto;

namespace UMBIT.CORE.API.Configurations
{
    public static class ConfigureServiceMySQL
    {
        public static void AddServiceMySQL(this IServiceCollection services, IConfiguration configuration)
        {
            string projectName = Assembly.GetCallingAssembly().GetName().Name;
            var conexao = configuration.GetSection("ConnectionString").Value ?? "";
            services.AddDbContext<DbContext, DataContext>(options => options.UseMySql(conexao, ServerVersion.AutoDetect(conexao), b => b.MigrationsAssembly(projectName)));
            services.AddScoped<IDataServiceFactory, DataServiceFactory>();
        }

    }
}
