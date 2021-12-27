using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

// https://json2csharp.com/

namespace CsharpRestClientExample
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Links
    {
        public object previous { get; set; }
        public string current { get; set; }
        public string next { get; set; }
    }

    public class Pagination
    {
        public int total { get; set; }
        public int pages { get; set; }
        public int page { get; set; }
        public int limit { get; set; }
        public Links links { get; set; }
    }

    public class Meta
    {
        public Pagination pagination { get; set; }
    }

    public class Datum
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public string status { get; set; }
    }

    public class Root
    {
        public Meta meta { get; set; }
        public List<Datum> data { get; set; }
    }

    class Program
    {
        private const string URL = "https://gorest.co.in/public/v1/users";
        private const string urlParameters = "";

        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            // Blocking call! Program will wait here until a response is received or a timeout occurs.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                //Make sure to add a reference to System.Net.Http.Formatting.dll
                // var dataObjects = response.Content.ReadAsAsync<IEnumerable<Root>>().Result;
                var dataObjects = response.Content.ReadAsAsync<Root>().Result;

                // 打印資料
                Console.WriteLine(dataObjects.meta.pagination.total);
                Console.WriteLine(dataObjects.meta.pagination.pages);
                Console.WriteLine(dataObjects.meta.pagination.page);
                Console.WriteLine(dataObjects.meta.pagination.limit);
                Console.WriteLine(dataObjects.meta.pagination.links.previous);
                Console.WriteLine(dataObjects.meta.pagination.links.current);
                Console.WriteLine(dataObjects.meta.pagination.links.next);

                // 打印data
                foreach (var item in dataObjects.data)
                {
                    Console.WriteLine("ID:{0}\nNAME:{1}\nEMAIL:{2}\nGENDER:{3}\nSTATUS:{4}\n\n",
                        item.id,
                        item.name,
                        item.email,
                        item.gender,
                        item.status);
                }
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            // Make any other calls using HttpClient here.

            // Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
             client.Dispose();
        }
    }
}
