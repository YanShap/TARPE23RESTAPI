using Microsoft.AspNetCore.Mvc;

namespace ITB2203Application.Controllers
{
    public class SpeakerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
