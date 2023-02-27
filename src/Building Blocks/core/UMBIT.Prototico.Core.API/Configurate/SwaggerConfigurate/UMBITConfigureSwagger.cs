using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using UMBIT.API.EXEMPLO.Configurations.basic;

namespace Prototico.Core.API.Configurate.Swagger
{
    public static class UMBITConfigureSwagger
    {
        public static IServiceCollection AddUMBITSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Insira o token JWT desta maneira: Bearer {seu token}",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });

            });

            return services;
        }
        public static IServiceCollection AddUMBITSwaggerConfiguration(this IServiceCollection services, OpenApiInfo openApiInfo)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", openApiInfo);

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Insira o token JWT desta maneira: Bearer {seu token}",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });

            });

            return services;
        }
        public static IServiceCollection AddUMBITSwaggerConfiguration(this IServiceCollection services, IConfiguration configuration, string sectionName)
        {
            IConfigurationSection section = configuration.GetSection(sectionName);
            SectionApiInfo SectionSectionApiInfo = section.Get<SectionApiInfo>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = SectionSectionApiInfo.Titile ?? "",
                    Description = SectionSectionApiInfo.Description ?? "",
                    Contact = new OpenApiContact() { Name = SectionSectionApiInfo.ContactName ?? "", Email = SectionSectionApiInfo.ContactEmail ?? "" }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Insira o token JWT desta maneira: Bearer {seu token}",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });

            });

            return services;
        }

    }
}
