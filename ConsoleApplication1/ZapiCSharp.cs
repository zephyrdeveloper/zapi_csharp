using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ZapiCSharp
{
    class Program
    {
        static HttpClient client = new HttpClient();

        static void Main()
        {
            RunAsync().Wait();
            Console.ReadLine();
        }
        
        static async Task RunAsync()
        {

            var USER = "admin";
            var ACCESS_KEY = "MTZhOGQ5OTEtOTI0OS0zNzdmLWIyZTAtYTFkOTFhZTI2OTczIGFkbWluIFVTRVJfREVGQVVMVF9OQU1F";
            var SECRET_KEY = "4b6v9i0kP7EjVzcuLMc4CUdkyq9AeTWheO2pr5CotGc";
            var BASE_URL = "https://qa-api.zephyr4jiracloud.com";
            var CONTEXT_PATH = "/connect";
            var EXPIRE_TIME = 3600;

            client.BaseAddress = new Uri(BASE_URL);
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var utc0 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var issueTime = DateTime.Now;

            var iat = (long)issueTime.Subtract(utc0).TotalMilliseconds;
            var exp = (long)issueTime.AddMilliseconds(EXPIRE_TIME).Subtract(utc0).TotalMilliseconds;

            var RELATIVE_PATH = "/public/rest/api/1.0/cycles/search";
            var QUERY_STRING = "expand=action&projectId=10000&versionId=10000";

            var canonical_path = "GET&" + RELATIVE_PATH + "&" + QUERY_STRING;
            var payload = new Dictionary<string, object>()
                {
                    { "sub", USER },                    //assign subject 
                    { "qsh", getQSH(canonical_path) },  //assign query string hash
                    { "iss", ACCESS_KEY },              //assign issuer
                    { "iat", iat },                     //assign issue at(in ms)
                    { "exp", exp }                      //assign expiry time(in ms)
                };

            string token = JWT.JsonWebToken.Encode(payload, SECRET_KEY, JWT.JwtHashAlgorithm.HS256);
            client.DefaultRequestHeaders.Add("Authorization", "JWT " + token);
            client.DefaultRequestHeaders.Add("zapiAccessKey", ACCESS_KEY);
            client.DefaultRequestHeaders.Add("User-Agent", "ZAPI");

            try
            {
                HttpResponseMessage response = await client.GetAsync(CONTEXT_PATH + RELATIVE_PATH + "?" + QUERY_STRING);
                response.EnsureSuccessStatusCode();

                //write response in console
                Console.WriteLine(response);

                // Deserialize the updated product from the response body.
                string result = await response.Content.ReadAsStringAsync();

                //write Response in console
                Console.WriteLine(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static string getQSH(string qstring)
        {
            System.Security.Cryptography.SHA256Managed crypt = new System.Security.Cryptography.SHA256Managed();
            StringBuilder hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(qstring), 0, Encoding.UTF8.GetByteCount(qstring));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

    }
}
