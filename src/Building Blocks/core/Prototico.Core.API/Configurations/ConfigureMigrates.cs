using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace UMBIT.API.EXEMPLO.Configurates
{
    public static class ConfigureMigrates
    {
        public static void UseConfigureMigrates(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope())
            {
                var context = serviceScope?.ServiceProvider.GetRequiredService<DbContext>();
                var criarMigartions = !context?.Database.EnsureCreated() ?? false;
                if (criarMigartions)
                {
                    if (context.Database.GetPendingMigrations().Any())
                        context.Database.EnsureDeleted();
                    context.Database.Migrate();

                }
            }
        }
    }
}
