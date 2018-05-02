using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.IO;
using System.Net;

namespace WebCameraSelfie.Storage
{
    public class YandexDiskStorage : IStorage
    {
        private string _token = "AQAAAAACatVlAAT4JTBxFSdd_0YTikZkbJ9EYEQ";
        private string _host = "https://cloud-api.yandex.net/v1/disk/resources";
        public async Task<string> UploadAndGetLink(byte[] data)
        {
            var restClient = new RestClient(_host)
            {
                Authenticator = new RestSharp.Authenticators.OAuth2AuthorizationRequestHeaderAuthenticator(_token),
                
            };
            var restRequest = new RestRequest("upload", Method.GET);
            
            var fileName = $"vid{DateTime.Now.ToUniversalTime().Ticks}.mp4";
            var path = $"app:/{fileName}";

            restRequest.AddQueryParameter("path", path);
            var restResponse = await restClient.ExecuteTaskAsync<ResourceUploadLink>(restRequest);
            var href = restResponse.Data.Href;

            await UploadFile(href, fileName, data);

            restRequest = new RestRequest("publish", Method.PUT);
            restRequest.AddQueryParameter("path", path);
            await restClient.ExecuteTaskAsync(restRequest);

            restRequest = new RestRequest(Method.GET);
            restRequest.AddQueryParameter("path", path);
            restRequest.AddQueryParameter("fields", "public_url");
            var response = await restClient.ExecuteTaskAsync<ResourceInfo>(restRequest);
            return response.Data.PublicUrl;
        }

        private async Task UploadFile(string href, string fileName, byte[] data)
        {
            var restClient = new RestClient(href);
            var restRequest = new RestRequest(Method.PUT);

            var webClient = new WebClient();
            await webClient.UploadDataTaskAsync(href, "PUT", data);
        }
    }

    public struct ResourceUploadLink
    {
        public string Href { get; set; }
    }
    public struct ResourceInfo
    {
        public string PublicUrl { get; set; }
    }
}
