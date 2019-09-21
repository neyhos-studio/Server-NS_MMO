using Application.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Application.Server.Mapper
{
    class ClientMapper
    {
        /// <summary>
        /// Allow the mapping of a Client object from a HttpResponseMessage from the API
        /// </summary>
        /// <param name="response">HttpResponseMessage </param>
        /// <returns>A Task<Client> you have to write ".Result" to transform the Task to a Client</Client></returns>
        public static async Task<Client> mapAsync(HttpResponseMessage response)
        {
            // This is a message from api to string 
            string message = await response.Content.ReadAsStringAsync();

            // String to JObject (JSON)
            JObject obj = JObject.Parse(message);

            // JObject(JSON) to Client
            Client client = JsonConvert.DeserializeObject<Client>(obj.ToString());

            return client;
        }
    }
}
