using BlackNails.DAL;
using BlackNails.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BlackNails.Controllers
{
    public class DepartmentController : Controller
    {
        private DepartmentServices _DepartmentServices = new DepartmentServices();

        // GET: Department
        public ActionResult Index()
        {
            return View();
        }
        // GET: Department
        public ActionResult list()
        {
            return Json(_DepartmentServices.FindList().ToList());
        }
    }
}