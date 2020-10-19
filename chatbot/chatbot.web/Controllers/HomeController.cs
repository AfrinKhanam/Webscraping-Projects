using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IndianBank_ChatBOT.Models;
using IndianBank_ChatBOT.Utils;
using IndianBank_ChatBOT.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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

        public async Task<IActionResult> Index([FromServices] IMemoryCache memoryCache)
        {
            var menuManager = new MenuManager(_appSettings.MenuItemsUrl, memoryCache);

            var menuItems = await menuManager.GetMenuItems();

            return View(menuItems);
        }

        public async Task<IActionResult> GetBotParams()
        {
            var url = $"{_appSettings.DirectLineBaseUrl}/v3/directline/tokens/generate";

            var r = new Random();
            var randNum = r.Next(1000000);

            var userId = $"User-{randNum:D6}";

            var jsonResponse = await HttpRequestUtils.PostJsonBody(url, new { userId });

            var tokenData = JsonConvert.DeserializeObject(jsonResponse) as JToken;

            return Json(new
            {
                UserId = userId,
                DirectLineUrl = $"{_appSettings.DirectLineBaseUrl}/v3/directline",
                DirectLineToken = tokenData.Value<string>("token")
            });
        }
    }
}