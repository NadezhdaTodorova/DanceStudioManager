using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DanceStudioManager
{
    [Authorize]
    public class StudioController : Controller
    {
        private readonly StudentsDataAccess _studentDataAccess;

        public StudioController(StudentsDataAccess studentDataAccess)
        {
            _studentDataAccess = studentDataAccess;
        }
        public IActionResult Dashboard()
        {
            var model = new Studio();
            model.Name = "My dance studio";
            return View(model);
        }

        public IActionResult Students()
        {
            return View();
        }

        public List<Student> GetStudents()
        {
            return _studentDataAccess.GetAllStudents();
        }

        public IActionResult Teachers()
        {
            return View();
        }

        public IActionResult Classes()
        {
            return View();
        }

        public IActionResult Events()
        {
            return View();
        }
    }
}