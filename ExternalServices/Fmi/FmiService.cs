using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ExternalServices.Fmi
{
    public abstract class Repository
    {
        public FmiService Service { get; set; }

        protected Repository(FmiService service)
        {
            Service = service;
        }
    }

    public class FmiService
    {
        public HttpClient Client { get; set; }
        private readonly Semaphore semaphore;

        public FmiService(HttpClient client)
        {
            semaphore = new Semaphore(8, 8);

            client.BaseAddress = new Uri("https://");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            //client.DefaultRequestHeaders.Add("Accept",
            //    "application/vnd.github.v3+json");

            //client.DefaultRequestHeaders.Add("User-Agent",
            //    "HttpClientFactory-Sample");

            Client = client;
        }

        internal async Task<T> Get<T>(string url)
        {
            semaphore.WaitOne();

            string result;
            HttpResponseMessage response;

            try
            {
                response = await Client.GetAsync(url).ConfigureAwait(false);
                result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            finally
            {
                semaphore.Release();
            }

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e);
                throw new ApplicationException(result);
            }

            using (MemoryStream stream = new MemoryStream(StringToUTF8ByteArray(result)))
            using (new XmlTextWriter(stream, new UTF8Encoding(false)))
            {
                return (T)new XmlSerializer(typeof(T)).Deserialize(stream);
            }
        }
        private byte[] StringToUTF8ByteArray(string xml)
        {
            return new UTF8Encoding().GetBytes(xml);
        }
    }
}
