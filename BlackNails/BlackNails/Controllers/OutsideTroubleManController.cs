using BlackNails.DAL;
using BlackNails.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BlackNails.Controllers
{
    public class OutsideTroubleManController : MVC5_BaseController
    {
        private OutsideTroubleManServices _OutsideTroubleManServices = new OutsideTroubleManServices();

        public ActionResult index()
        {
            ViewBag.Title = "外线员列表";
            return View();
        }

        [HttpGet]
        public ActionResult MyJsonList()
        {
            var Role = Session["RoleName"].ToString();
            var OutsideTroubleManJson = _OutsideTroubleManServices.FindList().Select(otm => new
            {
                OutsideTroubleMan_ID = otm.OutsideTroubleMan_ID,
                Name = otm.Name,
                Phone = otm.Phone,
                WorkYear = otm.WorkYear,
                WorkNo = otm.WorkNo,
                ResponsibleAreaBrief = otm.ResponsibleAreaBrief,
                Status = otm.Status,
                Role = Role
            }).ToList();
            var resonse = new Response();
            resonse.Code = 0;
            resonse.Message = "获取外线员列表成功！";
            resonse.Data = OutsideTroubleManJson;
            return Json(resonse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            ViewBag.Title = "添加外线员";
            return View();
        }

        [HttpPost]
        public ActionResult Add(OutsideTroubleManModel outsideTroubleManModel)
        {
            outsideTroubleManModel.CreatePerson = Session["UserName"].ToString();
            outsideTroubleManModel.CreateTime = DateTime.Now;
            outsideTroubleManModel.UpdateTime = DateTime.Now;
            outsideTroubleManModel.Status = "可工作";
            var resonse = _OutsideTroubleManServices.Add(outsideTroubleManModel);
            return RedirectToAction("index", "OutsideTroubleMan");
        }

        public ActionResult ViewPage(int OutsideTroubleMan_ID)
        {
            ViewBag.Title = "查看外线员";
            TempData.Add("OutsideTroubleMan_ID", OutsideTroubleMan_ID);
            return View();
        }

        [HttpGet]
        public ActionResult MyJsonOTM(int OutsideTroubleMan_ID)
        {
            var Role = Session["RoleName"].ToString();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            OutsideTroubleManModel outsideTroubleManModel = _OutsideTroubleManServices.Find(OutsideTroubleMan_ID);
            var resonse = new Response();
            resonse.Code = 0;
            resonse.Message = "获取外线员成功！";
            resonse.Data = outsideTroubleManModel;
            return Json(resonse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult MyJsonListSelect()
        {
            var OutsideTroubleManJson = _OutsideTroubleManServices.FindList().Select(otm => new
            {
                OutsideTroubleMan_ID = otm.OutsideTroubleMan_ID,
                Name = otm.Name,
            }).ToList();
            var resonse = new Response();
            resonse.Code = 0;
            resonse.Message = "获取外线员列表成功！";
            resonse.Data = OutsideTroubleManJson;
            return Json(resonse, JsonRequestBehavior.AllowGet);
        }
    }
}