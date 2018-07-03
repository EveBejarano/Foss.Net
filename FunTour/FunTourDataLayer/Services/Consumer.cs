using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FunTourDataLayer.Services
{
    public class Consumer<TEntity> where TEntity : class
    {
        private static readonly HttpClient client = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(5)
        };

        public async Task<TEntity> ReLoadEntities(string URL, string typeRequest, object ModelRequest)
        {
            var ModelResponse = await GetEntitiesFromAPIReturnModelRequest(URL, typeRequest, ModelRequest);

            return ModelResponse;
        }


        private async Task<TEntity> GetEntitiesFromAPIReturnModelRequest(string URL, string typeRequest,
            object ModelRequest)
        {
            var Url = URL;

            string data;

            var Json = JsonConvert.SerializeObject(ModelRequest);
            var request = new StringContent(Json, Encoding.UTF8, "application/json");

            var client = new HttpClient();

            switch (typeRequest)
            {
                case "PUT":
                        var resPut = client.PutAsync(Url, request).Result;
                        data = await resPut.Content.ReadAsStringAsync();

                        break;
                case "GET":
                    var resGet = client.GetAsync(Url).Result;
                    data = await resGet.Content.ReadAsStringAsync();

                    break;
                case "POST":
                    var resPost = client.PostAsync(Url, request).Result;
                    data = await resPost.Content.ReadAsStringAsync();


                    break;
                default:
                    data = "";
                    break;
            }


            return JsonConvert.DeserializeObject<TEntity>(data);
        }
    }
}