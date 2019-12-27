using IndianBank_ChatBOT.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndianBank_ChatBOT.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        public UserController()
        {
        }

        public IActionResult UpdateFeedback(string activityId, string conversationId, ResonseFeedback resonseFeedback)
        {
            return null;
        }
    }
}
