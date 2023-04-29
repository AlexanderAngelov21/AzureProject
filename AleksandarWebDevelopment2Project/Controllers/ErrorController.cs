using Microsoft.AspNetCore.Mvc;

namespace AleksandarWebDevelopment2Project.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                  
                    break;
            }
            return View("NotFound");
        }
    }
}
