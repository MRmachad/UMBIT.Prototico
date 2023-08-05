using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Prototico.Core.API.Configurate.TemplateSection;

namespace UMBIT.Prototico.Core.API.Configurate
{
    public static class UMBITConfigureJWT
    {
        public static void AddUMBITServiceJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var SectionJwtSection = configuration.GetSection("SectionJwt");

            var SectionJwt = SectionJwtSection.Get<SectionJwt>();

            var key = Encoding.ASCII.GetBytes(SectionJwt.Secret);

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                bearerOptions.RequireHttpsMetadata = true;
                bearerOptions.SaveToken = true;
                bearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidAudience = SectionJwt.ValidoEm,
                    ValidIssuer = SectionJwt.Emissor,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.Configure<SectionJwt>(configuration.GetSection("SectionJwt"));
        }
    }
}
