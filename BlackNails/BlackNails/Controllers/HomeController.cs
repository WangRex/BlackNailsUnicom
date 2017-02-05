using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlackNails.Controllers
{
    public class HomeController : MVC5_BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Title = "首页";
            return View();
        }
    }
}