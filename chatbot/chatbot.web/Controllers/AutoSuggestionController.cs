using System;
using System.Net.Http;
using System.Threading.Tasks;

using UjjivanBank_ChatBOT.Models;
using UjjivanBank_ChatBOT.ViewModel;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

namespace UjjivanBank_ChatBOT.Controllers
{
    //[Route("[controller]")]
    public class AutoSuggestionController : Controller
    {
        private readonly AppSettings _appSettings;
        public AutoSuggestionController(IOptions<AppSettings> appsettings)
        {
            _appSettings = appsettings.Value;
        }

        [HttpPost]
        public async Task<IActionResult> Suggest([FromBody] AutoSuggestionParam autoSuggestionParam)
        {
            try
            {
                if (string.IsNullOrEmpty(autoSuggestionParam.Query))
                {
                    return Ok(null);
                }

                var content = new StringContent(autoSuggestionParam.Query, System.Text.Encoding.UTF8, "application/json");
                string AutoSuggestionUrl = _appSettings.AutoSuggestionUrl;

                using var client = new HttpClient
                {
                    BaseAddress = new Uri(AutoSuggestionUrl)
                };

                //HTTP POST
                var responseTask = client.PostAsync("", content);
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var resultData = await result.Content.ReadAsStringAsync();
                    var suggestions = JsonConvert.DeserializeObject(resultData);
                    return Ok(suggestions);
                }
                else
                {
                    return Ok(null);
                }
            }
            catch
            {
                return Ok(null);
            }
        }
    }
}
