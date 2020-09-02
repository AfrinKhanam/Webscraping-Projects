using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IndianBank_ChatBOT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IndianBank_ChatBOT.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly AppSettings _appSettings;

        public HomeController(IOptions<AppSettings> appsettings)
        {
            _appSettings = appsettings.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetBotParams()
        {
            var url = $"{_appSettings.DirectLineBaseUrl}/v3/directline/tokens/generate";

            var userId = GetRandomUserName();

            var data = await PostStreamAsync(url, new { userId });

            var tokenData = JsonConvert.DeserializeObject(data) as JToken;

            return Json(new
            {
                UserId = userId,
                DirectLineUrl = $"{_appSettings.DirectLineBaseUrl}/v3/directline",
                DirectLineToken = tokenData.Value<string>("token")
            });
        }

        private string GetRandomUserName()
        {
            var r = new Random();
            var randNum = r.Next(1000000);

            return $"User-{randNum:D6}";
        }

        private async Task<string> PostStreamAsync(string url, object content, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, url))
            using (var httpContent = CreateHttpContent(content))
            {
                request.Content = httpContent;

                using (var response = await client
                    .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                    .ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();

                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        private HttpContent CreateHttpContent(object content)
        {
            HttpContent httpContent = null;

            if (content != null)
            {
                var ms = new MemoryStream();
                SerializeJsonIntoStream(content, ms);
                ms.Seek(0, SeekOrigin.Begin);
                httpContent = new StreamContent(ms);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            return httpContent;
        }

        public void SerializeJsonIntoStream(object value, Stream stream)
        {
            using (var sw = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            using (var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None })
            {
                var js = new JsonSerializer();
                js.Serialize(jtw, value);
                jtw.Flush();
            }
        }
    }
}