using System.Collections.Generic;

namespace UMBIT.Prototico.Core.API.Entidade
{
    public class TokenJWT
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public double ExpiresIn { get; set; }
        public UsuarioToken UsuarioToken { get; set; }

    }
    public class UsuarioToken
    {
        public string Login { get; set; }
        public IEnumerable<UsuarioClaim> Claims { get; set; }
    }
    public class UsuarioClaim
    {
        public string Value { get; set; }
        public string Type { get; set; }
    }
}
