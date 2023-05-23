using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TrelloToolsAccessInterfaces;

namespace TrelloToolsAccess
{
    public class WebRequestAC : IWebRequestAC
    {
        private static HttpWebRequest webRequest = null;

        public string GetRequest(string URI, Dictionary<string, string> parameters)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string URLConcateneted = String.Concat(URI, "?" + "key=", parameters["key"], "&token=", parameters["token"]);
            webRequest = (HttpWebRequest)WebRequest.Create(URLConcateneted);
            webRequest.Method = "GET";
            webRequest.ContentType = "application/json";
            webRequest.ContentLength = 0;
            WebResponse response = webRequest.GetResponse();
            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return responseString;
        }

        public async Task<string> SendRequest(string URI, Dictionary<string, string> parameters)
        {
            HttpClient httpClient = new HttpClient();
            MultipartFormDataContent form = new MultipartFormDataContent();
            AddParametersToForm(form, parameters);
            HttpResponseMessage response = await httpClient.PostAsync(URI, form);
            response.EnsureSuccessStatusCode();
            httpClient.Dispose();
            string responseResult = response.Content.ReadAsStringAsync().Result;
            return responseResult;
        }

        public async Task<string> SendRequestWithFile(string URI, byte[] fileContent, string fileName, Dictionary<string, string> parameters)
        {
            HttpClient httpClient = new HttpClient();
            MultipartFormDataContent form = new MultipartFormDataContent();
            AddParametersToForm(form, parameters);
            form.Add(new ByteArrayContent(fileContent, 0, fileContent.Length), "file", fileName);
            HttpResponseMessage response = await httpClient.PostAsync(URI, form);
            response.EnsureSuccessStatusCode();
            httpClient.Dispose();
            string responseResult = response.Content.ReadAsStringAsync().Result;
            return responseResult;
        }

        private void AddParametersToForm(MultipartFormDataContent form, Dictionary<string, string> parameters)
        {
            foreach (KeyValuePair<string, string> keyValue in parameters)
            {
                form.Add(new StringContent(keyValue.Value), keyValue.Key);
            }
        }
    }
}
