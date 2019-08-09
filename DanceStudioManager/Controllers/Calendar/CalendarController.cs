using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DanceStudioManager
{
    public class CalendarController : Controller
    {
        public IActionResult Index()
        {
            return View("Views/Studio/Calendar.cshtml");
        }
    }
}