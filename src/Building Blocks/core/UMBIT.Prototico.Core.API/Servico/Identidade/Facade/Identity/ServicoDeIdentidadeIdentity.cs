using System;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Prototico.Core.API.Configurate.TemplateSection;
using UMBIT.Prototico.Core.API.Model;

namespace UMBIT.Prototico.Core.API.Servico.Identidade.Facade.Identity
{
    public class ServicoDeIdentidadeIdentity : IServicoDeIdentidade
    {

        public AuthResponse AuthResponse;
        public readonly IOptions<SectionJwt> SectionJWT;
        public readonly UserManager<IdentityUser> UserManager;
        public readonly IUserValidator<IdentityUser> UserValidator;
        private readonly SignInManager<IdentityUser> SignInManager;
        public ServicoDeIdentidadeIdentity(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IUserValidator<IdentityUser> UserValidator,
            IOptions<SectionJwt> sectionJWT) : base()
        {
            UserManager = userManager;
            SectionJWT = sectionJWT;
            SignInManager = signInManager;
            this.UserValidator = UserValidator;

            this.AuthResponse = new AuthResponse();
        }
        public  async Task<AuthResponse> AutenticaUsuarioAsync(string usuario, string senha)
        {
            var validator = await UserValidator.ValidateAsync(UserManager, new IdentityUser
            {
                Email = usuario,
                UserName = usuario,
                PasswordHash = senha,
            });

            foreach (var error in validator.Errors)
            {
                AuthResponse.Errors.Add(new ValidationFailure(error.Code, error.Description));
            };

            return AuthResponse;
        }

        public  async Task<AuthResponse> Cadastro(string usuario, string senha)
        {
            var identityUser = new IdentityUser
            {
                Email = usuario,
                UserName = usuario,
                EmailConfirmed = true
            };

            var result = await UserManager.CreateAsync(identityUser, senha);

            if (result.Succeeded)
            {

                identityUser = await UserManager.FindByEmailAsync(usuario);

                var Token = await ServicoDeJWT.GetToken(
                    identityUser.Email,
                    SectionJWT.Value.Secret,
                    SectionJWT.Value.ExpiracaoMins,
                    SectionJWT.Value.ValidoEm,
                    SectionJWT.Value.Emissor
                    );

                AuthResponse.tokenJWT = Token;

                return AuthResponse;
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    AuthResponse.Errors.Add(new ValidationFailure(error.Code, error.Description));
                }
                return AuthResponse;
            }

        }

        public  async Task<AuthResponse> Login(string usuario, string senha)
        {
            var result = await SignInManager.PasswordSignInAsync(usuario,
                                                                      senha,
                                                                      isPersistent: false,
                                                                      lockoutOnFailure: true);

            if (result.Succeeded)
            {
                var Token = await ServicoDeJWT.GetToken(
                    usuario,
                    SectionJWT.Value.Secret,
                    SectionJWT.Value.ExpiracaoMins,
                    SectionJWT.Value.ValidoEm,
                    SectionJWT.Value.Emissor
                    );

                AuthResponse.tokenJWT = Token;
                return AuthResponse;
            }

            if (result.IsLockedOut)
            {
                AuthResponse.Errors.Add(new ValidationFailure("Falha De login", "Usuario bloqueado temporariamente por tentantivas invalidadas"));

                return AuthResponse;
            }

            AuthResponse.Errors.Add(new ValidationFailure("Falha De login", "Usuario ou senha incorreta"));

            return AuthResponse;
        }

        public async Task<AuthResponse> Logout(string usuario, string senha)
        {

            throw new NotImplementedException();
        }


    }
}
