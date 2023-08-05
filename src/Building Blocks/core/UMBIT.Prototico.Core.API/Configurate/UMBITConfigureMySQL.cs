using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UMBIT.Core.Repositorio;
using UMBIT.Core.Repositorio.Contexto;
using UMBIT.Prototico.Core.API.Data;

namespace UMBIT.Prototico.Core.API.Configurate
{
    public static class UMBITConfigureMySQL
    {
        public static void AddUMBITServiceMySQL(this IServiceCollection services, IConfiguration configuration)
        {

            StackTrace stackTrace = new StackTrace();
            var nameApi = stackTrace.GetFrame(1).GetMethod().DeclaringType.Assembly.GetName().Name;

            var conexao = configuration.GetSection("ConnectionString").Value ?? "";
            services.AddDbContext<DbContext, DataContext>(options => options.UseMySql(conexao, ServerVersion.AutoDetect(conexao), b => b.MigrationsAssembly(nameApi)));
            services.AddScoped<IUnidadeDeTrabalho, UnidadeDeTrabalho>();
        }
        public static IApplicationBuilder UseUMBITServiceMySQL(this IApplicationBuilder app)
        {

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope())
            {
                var context = serviceScope?.ServiceProvider.GetRequiredService<IdentityContext>();

                if (context.Database.GetPendingMigrations().Any())
                {
                    try
                    {
                        context.Database.Migrate();
                    }
                    catch (Exception)
                    {
#if DEBUG
                        if (context?.Database.EnsureDeleted() ?? false)
                            context.Database.Migrate();

                        context?.Database.EnsureCreated();
#endif
                    }

                }
            }

            return app;

        }
    }
}
