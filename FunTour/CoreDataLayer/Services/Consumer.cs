using FunTourDataLayer;
using FunTourDataLayer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FunTourDataLayer.Services
{
    public class Consumer<TEntity> where TEntity : class
    {

        public async Task<TEntity> ReLoadEntities(string URL, string typeRequest, object ModelRequest) 
        {

                var ModelResponse = await GetEntitiesFromAPIReturnModelRequest(URL, typeRequest, ModelRequest);
           
                return ModelResponse;

        }

        
        private async Task<TEntity> GetEntitiesFromAPIReturnModelRequest(string URL, string typeRequest, object ModelRequest)
        {
            var Url = URL;

            string data;

            string Json = JsonConvert.SerializeObject(ModelRequest);
            var request = new StringContent(Json, Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();
            
                switch (typeRequest)
                {
                    case "PUT":
                        using (HttpResponseMessage res = await client.PutAsync(Url, request))
                        {
                            using (HttpContent content = res.Content)
                            {
                                data = await content.ReadAsStringAsync();
                            }
                        }
                        break;
                    case "GET":
                        using (HttpResponseMessage res = await client.GetAsync(Url))
                        {
                            using (HttpContent content = res.Content)
                            {
                                data = await content.ReadAsStringAsync();
                            }
                        }
                        break;
                    case "POST":
                        using (HttpResponseMessage res = await client.PostAsync(Url, request))
                        {
                            using (HttpContent content = res.Content)
                            {
                                data = await content.ReadAsStringAsync();
                            }
                        }
                        break;
                    default:
                        data = "";
                        break;
                }
            

            return JsonConvert.DeserializeObject<TEntity>(data);
        }

    }
}
