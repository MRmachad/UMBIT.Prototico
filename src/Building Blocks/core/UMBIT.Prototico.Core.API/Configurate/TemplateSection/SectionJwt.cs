namespace Prototico.Core.API.Configurate.TemplateSection
{
    public class SectionJwt
    {
        public string Secret { get; set; }
        public int ExpiracaoMins { get; set; }
        public string Emissor { get; set; }
        public string ValidoEm { get; set; }
    }
}
