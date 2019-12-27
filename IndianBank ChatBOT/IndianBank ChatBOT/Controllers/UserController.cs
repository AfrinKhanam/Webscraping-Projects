using IndianBank_ChatBOT.Models;
using IndianBank_ChatBOT.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Controllers
{
    //[Route("[controller]")]
    public class UserController : Controller
    {
        public UserController()
        {
        }

        public string Index()
        {
            return "";
        }

        public IActionResult UpdateFeedback(FeedBackInfo feedBackInfo)
        {
            return null;
        }
    }
}
