using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using UMBIT.Prototico.Core.API.Extensions;
using System.Threading.Tasks;
using System;

namespace UMBIT.Prototico.Core.API.Servico.Basicos
{

    public abstract class Service
    {
        protected async Task<RequestResponse<T>> TratarerrosResponseAsync<T>(HttpResponseMessage response) where T : class
        {
            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            switch ((int)response.StatusCode)
            {

                case 401:
                case 403:
                case 404:
                case 500:
                    throw new CustomHttpRequestException(response.StatusCode);

                case 400:
                    {
                        var responseErro = new RequestResponse<T>();

                        ResponseResult resDeseriaze = JsonSerializer.Deserialize<ResponseResult>(await response.Content.ReadAsStringAsync(), option);

                        resDeseriaze.Errors.Mensagens.ForEach((msg) => responseErro.AdicionarErro(msg));

                        return responseErro;
                    }; 

                    
            }

            response.EnsureSuccessStatusCode();

            var responseFilter = new RequestResponse<T>();

            responseFilter.Frombory = JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), option);

            return responseFilter;

        }

        protected async Task<ResponseResult> TratarerrosResponseAsync(HttpResponseMessage response)
        {
            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            switch ((int)response.StatusCode)
            {

                case 401:
                case 403:
                case 404:
                case 500:
                    throw new CustomHttpRequestException(response.StatusCode);

                case 400:
                    {
                        ResponseResult resDeseriaze = JsonSerializer.Deserialize<ResponseResult>(await response.Content.ReadAsStringAsync(), option);
                        return resDeseriaze;
                    };


            }
            response.EnsureSuccessStatusCode();
            return new ResponseResult { Status = (int)response.StatusCode ,Title = response.Headers.ToString() };

        }
    }


    public class RequestResponse<T> : RequestResponseBase where T: class
    {
        public T Frombory { get; set; } 
    }
    public abstract class RequestResponseBase
    {
        [JsonIgnore]
        public ValidationResult ValidationResult { get; protected set; }

        private ICollection<string> Erros = new List<string>();

        public void AdicionarErro(string erro)
        {
            this.ValidationResult.Errors.Add(new ValidationFailure() { ErrorMessage = erro });
        }
    }
    public class ResponseResult
    {
        public string Title { get; set; }
        public int Status { get; set; }
        public ResponseErrorMessages Errors { get; set; }
    }
    public class ResponseErrorMessages
    {
        public List<string> Mensagens { get; set; }
    }
}
