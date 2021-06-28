using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Helpers
{
    public class HttpHelper : IDisposable
    {
        protected readonly string BookStoreApiUrl;

        private JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
        public HttpHelper(string bookStoreApiUrl)
        {
            BookStoreApiUrl = bookStoreApiUrl;
        }

        public HttpParams Params { get; private set; } = new HttpParams();
        public virtual HttpWebResponse GetResponse(WebRequest req) => (HttpWebResponse)req.GetResponse();

        #region [ HTTP GET ]
        public HttpWebResponse Response = null;
        public T Get<T>(string route, HttpParams httpParams = null, WebHeaderCollection headers = null)
        {
            var uri = new Uri(Path.Combine(BookStoreApiUrl, route) + BuildParams(httpParams));
            try
            {
                var req = GetWebRequest(uri, "GET");

                if (headers != null)
                    req.Headers.Add(headers);

                Response = GetResponse(req);
                using (var stream = Response.GetResponseStream())
                using (var streamReader = new StreamReader(stream))
                {
                    var response = streamReader.ReadToEnd();
                    return JsonConvert.DeserializeObject<T>(response, _jsonSerializerSettings);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return default(T);
            }

        }

        public async Task<T> GetAsync<T>(string route, object objContent, HttpParams httpParams = null, WebHeaderCollection headers = null)
        {
            try
            {
                var uri = new Uri(Path.Combine(BookStoreApiUrl, route) + BuildParams(httpParams));
                var content = JsonConvert.SerializeObject(objContent, _jsonSerializerSettings);

                var req = GetWebRequest(uri, "GET");
                if (headers != null)
                {
                    req.Headers.Add(headers);
                }
                
                var httpResponse = (HttpWebResponse)await req.GetResponseAsync();
                using (var stream = httpResponse.GetResponseStream())
                {
                    if (stream == null)
                        return default;

                    using (var streamReader = new StreamReader(stream))
                    {
                        var response = await streamReader.ReadToEndAsync();
                        return JsonConvert.DeserializeObject<T>(response);
                    }
                }
            }
            catch (WebException webEx)
            {
                var response = new StreamReader(webEx.Response.GetResponseStream()).ReadToEnd();
                return JsonConvert.DeserializeObject<T>(response);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default;
            }

        }

        #endregion

        #region [ HTTP POST ]
        public string Post(string route, string objContent, HttpParams httpParams = null, WebHeaderCollection headers = null)
        {
            var uri = new Uri(Path.Combine(BookStoreApiUrl, route) + BuildParams(httpParams));

            var req = GetWebRequest(uri, "POST");

            if (headers != null)
            {
                req.Headers.Add(headers);
            }

            using (var stream = req.GetRequestStream())
            using (var streamWriter = new StreamWriter(stream))
                streamWriter.WriteAsync(objContent);
            try
            {
                var httpResponse = (HttpWebResponse)req.GetResponse();
                using (var stream = httpResponse.GetResponseStream())
                {
                    if (stream == null)
                        return null;

                    using (var streamReader = new StreamReader(stream))
                        return streamReader.ReadToEnd();
                }
            }
            catch (WebException e)
            {
                string text = string.Empty;
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    using Stream data = response.GetResponseStream();
                    using var reader = new StreamReader(data);
                    text = reader.ReadToEnd();
                }
                throw new Exception(text);
            }

        }

        public T Post<T>(string route, string objContent, HttpParams httpParams = null, WebHeaderCollection headers = null)
        {
            var response = Post(route, objContent, httpParams, headers);
            return JsonConvert.DeserializeObject<T>(response, _jsonSerializerSettings);
        }

        public async Task<T> PostAsync<T>(string route, object objContent, HttpParams httpParams = null, WebHeaderCollection headers = null)
        {
            try
            {
                var uri = new Uri(Path.Combine(BookStoreApiUrl, route) + BuildParams(httpParams));
                var content = JsonConvert.SerializeObject(objContent, _jsonSerializerSettings);

                var req = GetWebRequest(uri, "POST");
                if (headers != null)
                {
                    req.Headers.Add(headers);
                }
                using (var stream = await req.GetRequestStreamAsync())
                using (var streamWriter = new StreamWriter(stream))
                    await streamWriter.WriteAsync(content);

                var httpResponse = (HttpWebResponse)await req.GetResponseAsync();
                using (var stream = httpResponse.GetResponseStream())
                {
                    if (stream == null)
                        return default;

                    using (var streamReader = new StreamReader(stream))
                    {
                        var response = await streamReader.ReadToEndAsync();
                        return JsonConvert.DeserializeObject<T>(response);
                    }
                }
            }
            catch (WebException webEx)
            {
                var response = new StreamReader(webEx.Response.GetResponseStream()).ReadToEnd();
                return JsonConvert.DeserializeObject<T>(response);

            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion

        private string BuildParams(HttpParams httpParams)
        {
            var strParams = string.Empty;

            if (Params.HasParams)
            {
                strParams = Params.ToString();

                if (httpParams != null)
                {
                    if (httpParams.HasParams)
                        strParams += httpParams.ToString().Replace("?", "&");
                }
            }
            else
            {
                if (httpParams != null)
                {
                    if (httpParams.HasParams)
                        strParams = httpParams.ToString();
                }
            }

            return strParams;
        }

        private WebRequest GetWebRequest(Uri uri, string methodType)
        {
            var req = WebRequest.Create(uri);
            req.Method = methodType;
            req.ContentType = "application/json";
            return req;
        }

    }



    public class HttpParams
    {
        private Dictionary<string, object> _params = new();

        public bool HasParams { get { return _params.Count > 0; } }

        public void Add(string keyParam, object valueParam) => _params.Add(keyParam, valueParam);

        public void Remove(string keyParam) => _params.Remove(keyParam);

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var param in _params)
            {
                var k = sb.Length == 0 ? "?" : "&";
                sb.Append($"{k}{param.Key}={param.Value}");
            }

            return sb.ToString();
        }
    }

}
