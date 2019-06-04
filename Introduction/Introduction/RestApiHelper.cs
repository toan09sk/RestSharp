using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace Introduction
{
    public class RestApiHelper<DTO>
    {
        public RestClient _restClient { get; set; }
        public RestRequest _restRequest { get; set; }
        private const string _baseUrl = "https://reqres.in/";

        public RestClient SetUrl(string resourceUrl)
        {
            var url = Path.Combine(_baseUrl, resourceUrl);
            _restClient = new RestClient(url);
            return _restClient;
        }

        public RestRequest CreatePostRequest(string jsonString)
        {
            _restRequest = new RestRequest(Method.POST);
            _restRequest.AddHeader("Accept", "application/json");
            _restRequest.AddParameter("application/json", jsonString, ParameterType.RequestBody);

            return _restRequest;
        }

        public RestRequest CreateGetRequest(string jsonString)
        {
            _restRequest = new RestRequest(Method.GET);
            _restRequest.AddHeader("Accept", "application/json");
            _restRequest.AddParameter("application/json", jsonString, ParameterType.RequestBody);

            return _restRequest;
        }

        public RestRequest CreateDeleteRequest(string jsonString)
        {
            _restRequest = new RestRequest(Method.DELETE);
            _restRequest.AddHeader("Accept", "application/json");
            _restRequest.AddParameter("application/json", jsonString, ParameterType.RequestBody);

            return _restRequest;
        }

        public RestRequest CreatePUTRequest(string jsonString)
        {
            _restRequest = new RestRequest(Method.PUT);
            _restRequest.AddHeader("Accept", "application/json");
            _restRequest.AddParameter("application/json", jsonString, ParameterType.RequestBody);

            return _restRequest;
        }

        public IRestResponse GetResponse(RestClient restClient,RestRequest restRequest)
        {
            return restClient.Execute(restRequest);
        }

        public DTO GetContent<DTO>(IRestResponse response)
        {
            var content = response.Content;
            DTO deserializeObject = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO>(content);
            return deserializeObject;
        }

    }
}
