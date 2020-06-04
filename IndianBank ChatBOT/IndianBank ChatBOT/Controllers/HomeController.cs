using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IndianBank_ChatBOT.Models;
using IndianBank_ChatBOT.ViewModel;
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
                DirectLineConversationUrl = _appSettings.DirectLineConversationUrl,
                DirectLineTokenGenerationUrl = _appSettings.DirectLineTokenGenerationUrl
            };
            return View(chatBotSettings);
        }
    }
}
