using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UMBIT.Prototico.Core.API.Servico.Interface;

namespace UMBIT.Prototico.Core.API.Servico.Basicos
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

            string queryParam = String.IsNullOrEmpty(nameValue?.ToString()) ? "" : nameValue.ToString();

            var response = await this.cliente.GetAsync((path), HttpCompletionOption.ResponseContentRead);

            var filterRes = await this.TratarerrosResponseAsync<T>(response);
            return filterRes;

        }

        public async Task<ResponseResult> ExecutePostAsync<T>(T toContent, string path, Encoding encoding, string mediaType)
            where T : class
        {
            var content = new StringContent(JsonSerializer.Serialize(toContent), Encoding.UTF8, mediaType);

            var response = await this.cliente.PostAsync(path, content);

            var filterRes = await this.TratarerrosResponseAsync(response);
            return filterRes;
        }
    }
}
