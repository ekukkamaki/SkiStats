using System;
using System.Net.Http;
using IdentityModel.Client;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AuthenticationTest
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var client = new HttpClient();
            //var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000/round/GetRounds");
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5001");

            if (disco.IsError)
            {
                Console.WriteLine("täällä ollaan");
                Console.WriteLine(disco.Error);
                return;
            }

            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest {
                Address = disco.TokenEndpoint,
                ClientId = "ro.client",
                ClientSecret = "secret",

                UserName = "alice",
                Password = "password",
                Scope = "api1"
            });            

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.GetAsync("http://localhost:5000/api/round");

            if (!response.IsSuccessStatusCode) {
                Console.WriteLine("ei onnistunut");
                Console.WriteLine(response.StatusCode);
            }
            else {
                var content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(JArray.Parse(content));
            }

        }
    }
}
