using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Prototico.Core.API.Configurate.JsonWebToken;
using Prototico.Core.API.Configurate.TemplateSection;
using System;
using System.Threading.Tasks;
using UMBIT.API.EXEMPLO.Configurations.basic;
using UMBIT.Prototico.Core.API.Servico.Basicos;
using UMBIT.Prototico.Core.API.Servico.Interface;

namespace UMBIT.Prototico.Core.API.Servico
{
    public class ServicoDeIdentidadeIdentity : ServicoDeIdentidade
    {

        public readonly IOptions<SectionJwt> SectionJWT;
        public readonly UserManager<IdentityUser> UserManager;
        public readonly IUserValidator<IdentityUser> UserValidator;
        private readonly SignInManager<IdentityUser> SignInManager;
        public ServicoDeIdentidadeIdentity(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IUserValidator<IdentityUser> UserValidator, IOptions<SectionJwt> sectionJWT): base()
        {
            this.UserManager = userManager;
            this.SectionJWT = sectionJWT;
            this.SignInManager = signInManager;
            this.UserValidator = UserValidator;
        }
        public override async Task<AuthResponse> AutenticaUsuarioAsync(string usuario, string senha)
        {
            var validator = await this.UserValidator.ValidateAsync(this.UserManager, new IdentityUser
            {
                Email = usuario,
                UserName = usuario,
                PasswordHash = senha,
            });

            foreach (var error in validator.Errors)
            {
                this.AuthResponse.Errors.Add(new ValidationFailure(error.Code, error.Description));
            };

            return this.AuthResponse;
        }

        public override async Task<AuthResponse> Cadastro(string usuario, string senha)
        {
            var identityUser = new IdentityUser
            {
                Email = usuario,
                UserName = usuario,
                EmailConfirmed = true
            };

            var result = await this.UserManager.CreateAsync(identityUser, senha);

            if (result.Succeeded)
            {

                identityUser = await this.UserManager.FindByEmailAsync(usuario);

                var Token = await ServicoDeJWT.GetToken(
                    identityUser.Email,
                    this.SectionJWT.Value.Secret,
                    this.SectionJWT.Value.ExpiracaoMins,
                    this.SectionJWT.Value.ValidoEm,
                    this.SectionJWT.Value.Emissor
                    );

                this.AuthResponse.tokenJWT = Token;

                return this.AuthResponse;
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    this.AuthResponse.Errors.Add(new ValidationFailure(error.Code, error.Description));
                }
                return this.AuthResponse;
            }

        }

        public override async Task<AuthResponse> Login(string usuario, string senha)
        {
            var result = await this.SignInManager.PasswordSignInAsync(usuario,
                                                                      senha,
                                                                      isPersistent: false,
                                                                      lockoutOnFailure: true);

            if (result.Succeeded)
            {
                var Token = await ServicoDeJWT.GetToken(
                    usuario,
                    this.SectionJWT.Value.Secret,
                    this.SectionJWT.Value.ExpiracaoMins,
                    this.SectionJWT.Value.ValidoEm,
                    this.SectionJWT.Value.Emissor
                    );

                this.AuthResponse.tokenJWT = Token;
                return AuthResponse;
            }

            if (result.IsLockedOut)
            {
                this.AuthResponse.Errors.Add(new ValidationFailure("Falha De login", "Usuario bloqueado temporariamente por tentantivas invalidadas"));

                return this.AuthResponse;
            }

            this.AuthResponse.Errors.Add(new ValidationFailure("Falha De login", "Usuario ou senha incorreta"));

            return this.AuthResponse;
        }

        public override Task<AuthResponse> Logout(string usuario, string senha)
        {

            throw new NotImplementedException();
        }

        
    }
}
