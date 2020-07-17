using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;

namespace IndianBank_ChatBOT.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            var exceptionHandler = HttpContext.Features.Get<IExceptionHandlerFeature>() as ExceptionHandlerFeature;

            var path = exceptionHandler.Path;

            var exception = exceptionHandler.Error;

            var errorNumber = Guid.NewGuid();

            return View(errorNumber);
        }
    }
}
