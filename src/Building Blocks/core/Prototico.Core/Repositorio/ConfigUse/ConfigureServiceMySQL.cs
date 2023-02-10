using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using UMBIT.Core.Mediator;
using UMBIT.Core.Repositorio.Contexto;

namespace UMBIT.Core.Repositorio.ConfigUse
{
    public class ConfigureServiceMySQL
    {
        public void ConfigureServices(IServiceCollection services, IOptions<ConnectionStrings> options)
        {
            string mySqlConnectionStr = options.Value.ConnectionString;
            services.AddDbContextPool<ContextoDB>(options => options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr)));
        }

    }
}
