using MediatR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UMBIT.Prototico.Core.API.Entidade;
using UMBIT.Prototico.Core.API.Servico.Interface;

namespace UMBIT.Prototico.Core.API.Servico.Basicos
{
    public class ServicoDeJWT : IServicoDeJWT
    {
        public async Task<TokenJWT> GetToken(string user, string secret, double expiracaoMins, string validoEm, string emissor)
        {
            return await GerarJwt(user, secret, expiracaoMins, validoEm, emissor);
        }

        private async Task<TokenJWT> GerarJwt(string user, string secret, double expiracaoMins, string validoEm, string emissor)
        {

            var identityClaims = await ObtenhaClaims(user);

            var encodedToken = ObtenhaToken(identityClaims, secret, expiracaoMins, validoEm, emissor);
            var encodedRefreshToken = ObtenhaToken(identityClaims, secret, 30 * 24 * 60, validoEm, emissor);

            return await ObtenhaRespostaToken(user, encodedToken, encodedRefreshToken, expiracaoMins, identityClaims);
        }
        private Task<TokenJWT> ObtenhaRespostaToken(string user, string encodedToken, string encodedRefreshToken, double expiracaoMins, ClaimsIdentity claimsIdentity)
        {
            var response = new TokenJWT
            {
                AccessToken = encodedToken,
                RefreshToken = encodedRefreshToken,
                ExpiresIn = TimeSpan.FromMinutes(expiracaoMins).TotalSeconds,
                UsuarioToken = new UsuarioToken
                {
                    Login = user,
                    Claims = claimsIdentity.Claims.Select(c => new UsuarioClaim { Type = c.Type, Value = c.Value })
                }
            };

            return Task.FromResult(response);
        }
        private Task<ClaimsIdentity> ObtenhaClaims(string user, IList<Claim> claim = null, IList<string> userRoles = null)
        {
            var identityClaims = new ClaimsIdentity();
            var claims = claim != null ? claim : new List<Claim>();

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            if (userRoles != null)
                foreach (var userRole in userRoles)
                {
                    claims.Add(new Claim("role", userRole));
                }
            if (claims != null)
                identityClaims.AddClaims(claims);

            return Task.FromResult(identityClaims);

        }
        private string ObtenhaToken(ClaimsIdentity identityClaims, string secret, double expiracaoMins, string validoEm, string emissor)
        {
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {

                Issuer = emissor,
                Audience = validoEm,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddMinutes(expiracaoMins),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }
        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1910, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

    }
}
