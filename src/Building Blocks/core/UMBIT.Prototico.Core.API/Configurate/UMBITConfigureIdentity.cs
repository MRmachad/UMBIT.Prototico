using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UMBIT.Prototico.Core.API.Data;
using UMBIT.Prototico.Core.API.Extensions;
using UMBIT.Prototico.Core.API.Servico.Identidade;
using UMBIT.Prototico.Core.API.Servico.Identidade.Facade.Identity;

namespace UMBIT.Prototico.Core.API.Configurate
{
    public static class UMBITConfigureIdentity
    {
        /// <summary>
        /// Adiciona aos serviços do APP o uso de Contexto de Identity e o serviço de Token JWT
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddUMBITIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            StackTrace stackTrace = new StackTrace();
            var nameApi = stackTrace.GetFrame(1).GetMethod().DeclaringType.Assembly.GetName().Name;

            var conexao = configuration.GetSection("ConnectionString").Value ?? "";
            services.AddDbContext<IdentityContext>(options => options.UseMySql(conexao, ServerVersion.AutoDetect(conexao), b => b.MigrationsAssembly(nameApi)));

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddErrorDescriber<IdentityMensagensPortugues>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.AddScoped<IServicoDeIdentidade, ServicoDeIdentidadeIdentity>();

            services.AddUMBITServiceJWT(configuration);

            return services;
        }

        /// <summary>
        ///  Define o uso da Migartions para databa do contexto do Identity
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseUMBITIdentityConfiguration(this IApplicationBuilder app)
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
                        if (!context?.Database.EnsureCreated() ?? false)
                            context.Database.EnsureDeleted();
                        context.Database.Migrate();
                    }

                }
            }

            return app;
        }
    }
}
