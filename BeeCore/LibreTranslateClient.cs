using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BeeCore
{
    public class LibreTranslateClient : IDisposable
    {
        private HttpClient _http;
        public string BaseUrl { get; private set; }

        public LibreTranslateClient(string baseUrl)
        {
            BaseUrl = baseUrl.TrimEnd('/');
            _http = new HttpClient();
            _http.Timeout = TimeSpan.FromSeconds(30);
        }
        public bool Ping()
        {
            return PingAsync().GetAwaiter().GetResult();
        }

        public bool Ping_NoDeadlock()
        {
            return System.Threading.Tasks.Task.Run(() => PingAsync())
                .GetAwaiter()
                .GetResult();
        }
        public void Dispose()
        {
            if (_http != null)
            {
                _http.Dispose();
                _http = null;
            }
        }
        public string Translate(string text, string target, string source)
        {
            // Cách 1: sync wrapper đơn giản
            return TranslateAsync(text, target, source, default(CancellationToken))
                .GetAwaiter()
                .GetResult();
        }
        /// <summary>
        /// Translate text offline via LibreTranslate local server
        /// source: "auto" | "vi" | "th" | "en" | ...
        /// target: "en" | "vi" | ...
        /// </summary>
        public async Task<string> TranslateAsync(
            string text,
            string target,
            string source,
            CancellationToken token = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            var payload = new
            {
                q = text,
                source = string.IsNullOrEmpty(source) ? "auto" : source,
                target = target,
                format = "text"
            };

            string json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage resp = await _http.PostAsync(
                BaseUrl + "/translate",
                content,
                token
            );

            string body = await resp.Content.ReadAsStringAsync();

            if (!resp.IsSuccessStatusCode)
                throw new Exception("LibreTranslate error: " + body);

            JObject jo = JObject.Parse(body);
            return jo["translatedText"] != null
                ? jo["translatedText"].ToString()
                : body;
        }

        /// <summary>
        /// Check server running
        /// </summary>
        public async Task<bool> PingAsync()
        {
            try
            {
                HttpResponseMessage r = await _http.GetAsync(BaseUrl + "/languages");
                return r.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
