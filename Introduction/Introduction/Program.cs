using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Introduction.Model;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Serialization.Json;

namespace Introduction
{
    static class Program
    {
        static void Main(string[] args)
        {
            postBinaryFile();
        }

        static void GetString()
        {
            var client = new RestClient("https://reqres.in/");

            var request = new RestRequest("/api/unknown", Method.GET);
            request.AddHeader("Accept", "application/json");

            IRestResponse response = client.Execute(request);
            var content = response.Content; // raw content as string

            Console.WriteLine(content);

            Console.ReadLine();
        }

        static void GetStringAsync()
        {
            var client = new RestClient("https://reqres.in/");

            var request = new RestRequest("/api/unknown", Method.GET);
            request.AddHeader("Accept", "application/json");

            // easy async support
            client.ExecuteAsync(request, responseData =>
            {
                Console.WriteLine(responseData.Content);
            });


            Console.ReadLine();
        }

        static void GetStringDataObject()
        {
            var client = new RestClient("https://reqres.in/");

            var request = new RestRequest("/api/users/2", Method.GET);
            request.AddHeader("Accept", "application/json");

            var asyncHandle = client.ExecuteAsync<Person>(request, response =>
            {

                Console.WriteLine(response.Data.data.last_name);
            });

            Console.ReadLine();
        }

        public static void Chuan()
        {
            string jsonString = @"{
                                        ""name"": ""morpheus"",
                                        ""job"": ""leader""
                                  }";

            RestApiHelper<CreateUser> restApi = new RestApiHelper<CreateUser>();
            var restUrl = restApi.SetUrl("api/users");
            var restRequest = restApi.CreatePostRequest(jsonString);
            var resResponse = restApi.GetResponse(restUrl, restRequest);

            CreateUser content = restApi.GetContent<CreateUser>(resResponse);
        }

        public static void GetMethod()
        {
            var client = new RestClient("https://reqres.in/");
            var request = new RestRequest("api/users/2", Method.GET);
            request.AddUrlSegment("postId", 2);

            IRestResponse response = client.Execute<Person>(request);
            var content = response.Content; // raw content as string;

            var deserialize = new JsonDeserializer();
            var output = deserialize.Deserialize<Dictionary<string, string>>(response);

            JObject obs = JObject.Parse(content);

            var result = obs["data"];

            var result1 = output["data"];

        }

        public static void PostWithAnonymousBody()
        {
            var client = new RestClient("https://reqres.in/");
            var request = new RestRequest("api/users/2", Method.POST);

            request.AddJsonBody(new People {name= "morpheus",job= "leader" });

            request.RequestFormat = DataFormat.Json;

            request.AddUrlSegment("postId", 2);

            var response = client.Execute<CreateUser>(request);

            var deserialize = new JsonDeserializer();
            var output = deserialize.Deserialize<Dictionary<string, string>>(response);
            var result = output["name"];

            // another way
           var responseAsync= ExecuteAsyncRequest<People>(client, request).GetAwaiter().GetResult();

        }

        public static async Task<IRestResponse<T>> ExecuteAsyncRequest<T>(RestClient client,IRestRequest request) where T: class, new()
        {
            var taskCompletitionSource = new TaskCompletionSource<IRestResponse<T>>();

            client.ExecuteAsync<T>(request, restResponse =>
            {
                if (restResponse.ErrorException!=null)
                {
                    const string message = "Error retrieveing response";
                    throw new ApplicationException(message, restResponse.ErrorException);
                }

                taskCompletitionSource.SetResult(restResponse);

            });

            return await taskCompletitionSource.Task;
        }

        public static void postFile()
        {
            var client = new RestClient("http://localhost:49869/");
            var request = new RestRequest("api/Management", Method.POST);

            request.AddFile("Toan.rtf", @"C:\Users\ross.pham\Desktop\SimpleInjectiorLearn.rtf");

            request.RequestFormat = DataFormat.Json;

            // another way
            var responseAsync = ExecuteAsyncRequest<People>(client, request).GetAwaiter().GetResult();
        }

        public static void postBinaryFile()
        {
            byte[] array = Encoding.ASCII.GetBytes("Tetsting test data");

            var client = new RestClient("http://localhost:49869/");
            var request = new RestRequest("api/Management", Method.POST);

            request.AddFile("File", array, "Testing.rtf");

            request.RequestFormat = DataFormat.Json;

            // another way
            var responseAsync = ExecuteAsyncRequest<People>(client, request).GetAwaiter().GetResult();

        }
    }
}
