using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prototico.Core.API.Configurate.JsonWebToken;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UMBIT.Prototico.Core.API.Data;
using UMBIT.Prototico.Core.API.Extensions;
using UMBIT.Prototico.Core.API.Servico;
using UMBIT.Prototico.Core.API.Servico.Interface;

namespace UMBIT.Prototico.Core.API.Configurate.IdentityConfigurate
{
    public static class UMBITConfigureIdentity
    {
        public static IServiceCollection AddUMBITIdentityConfiguration(this IServiceCollection services,
                                                                        IConfiguration configuration)
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

            app.UseAuthorization();
            app.UseAuthorization();

            return app;


        }
    }
}
