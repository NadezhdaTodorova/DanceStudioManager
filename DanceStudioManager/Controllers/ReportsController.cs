using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DanceStudioManager
{
    public class ReportsController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.text = "Reports";
            return View("Views/Studio/Reports.cshtml");
        }
    }
}