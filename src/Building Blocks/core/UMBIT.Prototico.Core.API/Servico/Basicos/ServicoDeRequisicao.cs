using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UMBIT.Prototico.Core.API.Servico.Interface;

namespace UMBIT.Prototico.Core.API.Servico.Basicos
{
    //    services.AddHttpClient<InterfaceDeServico, Servico que Herda>("",  (configHTTPClient) =>{
    //                configHTTPClient.BaseAddress = new Uri("");
    //});
    public abstract class ServicoDeRequisicao : Service, IServicoDeRequisicao
    {
        private readonly HttpClient cliente;

        public ServicoDeRequisicao(HttpClient cliente, string baseURL)
        {
            this.cliente = cliente;
        }

        public async Task<RequestResponse<T1>> ExecuteGetAsync<T, T1>(T toContent, string path,Dictionary<string, string> keyValuePairs, string mediaType)
            where T : class
            where T1 : class
        {
            var nameValue = new NameValueCollection();
            
            foreach(var keyValue in keyValuePairs)
            {
                nameValue.Add(keyValue.Key, keyValue.Value);
            }

            var response = await this.cliente.GetAsync((path + nameValue.ToString() ?? ""),HttpCompletionOption.ResponseContentRead);

            var filterRes = await this.TratarerrosResponseAsync<T1>(response);
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
