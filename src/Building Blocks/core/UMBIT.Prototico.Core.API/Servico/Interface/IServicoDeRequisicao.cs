using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMBIT.Prototico.Core.API.Servico.Basicos;

namespace UMBIT.Prototico.Core.API.Servico.Interface
{
    public interface IServicoDeRequisicao
    {
        Task<ResponseResult> ExecutePostAsync<T>(T toContent, string path, Encoding encoding, string mediaType)
             where T : class;
        
         Task<RequestResponse<T1>> ExecuteGetAsync<T, T1>(T toContent, string path, Dictionary<string, string> keyValuePairs, string mediaType)
             where T : class
             where T1 : class;
    }
}
//    services.AddHttpClient<InterfaceDeServico, Servico que Herda>("",  (configHTTPClient) =>{
//                configHTTPClient.BaseAddress = new Uri("");
//});
