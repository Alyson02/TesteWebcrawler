using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TesteWebcrawler.Models;

namespace TesteWebcrawler
{
    public static class ApiRequests
    {

        public static async Task<CreateExecutionResponse> CreateExecution(CreateExecutionModel body)
        {
            try
            {
                var client = new RestClient();
                var url = "http://localhost:4000/execution";
                var request = new RestRequest(url, Method.Post);
                request.AddJsonBody(body);
                var response = await client.ExecutePostAsync<CreateExecutionResponse>(request);

                if(response.StatusCode == HttpStatusCode.OK) return response.Data;

                throw new Exception();
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao inserir execuçao - " + e.Message);
            }
        }

        public static async Task<bool> UpdateExecution(UpdateExecutionModel body)
        {
            try
            {
                var client = new RestClient();
                var url = "http://localhost:4000/execution/" + body.ExecutionId;
                var request = new RestRequest(url, Method.Put);
                request.AddJsonBody(body);
                var response = await client.ExecutePutAsync(request);

                if (response.StatusCode == HttpStatusCode.OK) return true;

                throw new Exception();
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
