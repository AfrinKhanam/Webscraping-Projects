using IndianBank_ChatBOT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IndianBank_ChatBOT.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppSettings _appSettings;
        public HomeController(IOptions<AppSettings> appsettings)
        {
            _appSettings = appsettings.Value;
        }

        public IActionResult Index()
        {
            var chatBotSettings = new ChatBotSettings
            {
                DirectLineBaseUrl = _appSettings.DirectLineBaseUrl
            };
            return View(chatBotSettings);
        }
    }
}
