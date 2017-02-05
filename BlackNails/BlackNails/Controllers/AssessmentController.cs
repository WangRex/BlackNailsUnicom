using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlackNails.Controllers
{
    public class AssessmentController : MVC5_BaseController
    {
        // GET: Assessment
        public ActionResult Index()
        {
            return View();
        }
    }
}