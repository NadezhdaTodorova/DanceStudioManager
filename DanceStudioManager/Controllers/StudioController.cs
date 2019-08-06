using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DanceStudioManager
{
    [Authorize]
    public class StudioController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}