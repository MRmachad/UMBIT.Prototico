using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UMBIT.Prototico.Core.API.Servico.RestAPI.Basicos;

namespace UMBIT.Prototico.Core.API.Servico.RestAPI.Facade.SystemNet
{
    public abstract class ServicoDeRequisicao : Service, IServicoDeRequisicao
    {
        private readonly HttpClient cliente;

        public ServicoDeRequisicao(HttpClient cliente)
        {
            this.cliente = cliente;
        }

        public async Task<RequestResponse<T>> ExecuteGetAsync<T>(string path, Dictionary<string, string> keyValuePairs = null)
            where T : class
        {

            var nameValue = new NameValueCollection();
            if (keyValuePairs != null)
                foreach (var keyValue in keyValuePairs)
                {
                    nameValue.Add(keyValue.Key, keyValue.Value);
                }

            string queryParam = string.IsNullOrEmpty(nameValue?.ToString()) ? "" : nameValue.ToString();

            var response = await cliente.GetAsync(path, HttpCompletionOption.ResponseContentRead);

            var filterRes = await TratarerrosResponseAsync<T>(response);
            return filterRes;

        }

        public async Task<ResponseResult> ExecutePostAsync<T>(T toContent, string path, Encoding encoding, string mediaType)
            where T : class
        {
            var content = new StringContent(JsonSerializer.Serialize(toContent), Encoding.UTF8, mediaType);

            var response = await cliente.PostAsync(path, content);

            var filterRes = await TratarerrosResponseAsync(response);
            return filterRes;
        }
    }
}
