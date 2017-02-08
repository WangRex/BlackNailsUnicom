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
        private OrderServices _OrderServices = new OrderServices();
        private AssessmentServices _AssessmentServices = new AssessmentServices();
        
        public ActionResult index()
        {
            ViewBag.Title = "外线员列表";
            return View();
        }

        [HttpGet]
        public ActionResult MyJsonList()
        {
            var Role = Session["RoleName"].ToString();
            var OutsideTroubleManJson = _OutsideTroubleManServices.FindList().ToList();
            //Select(otm => new
            //{
            //    OutsideTroubleMan_ID = otm.OutsideTroubleMan_ID,
            //    Name = otm.Name,
            //    Phone = otm.Phone,
            //    WorkYear = otm.WorkYear,
            //    WorkNo = otm.WorkNo,
            //    ResponsibleAreaBrief = otm.ResponsibleAreaBrief,
            //    Status = otm.Status,
            //    serviceNum = _OrderServices.getOTMServiceNum(otm.OutsideTroubleMan_ID),
            //    Role = Role
            //}).ToList();

            var list = new List<object>();
            foreach (OutsideTroubleManModel _OutsideTroubleManModel in OutsideTroubleManJson)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("OutsideTroubleMan_ID", _OutsideTroubleManModel.OutsideTroubleMan_ID);
                dic.Add("Name", _OutsideTroubleManModel.Name);
                dic.Add("Phone", _OutsideTroubleManModel.Phone);
                dic.Add("WorkYear", _OutsideTroubleManModel.WorkYear);
                dic.Add("WorkNo", _OutsideTroubleManModel.WorkNo);
                dic.Add("ResponsibleAreaBrief", _OutsideTroubleManModel.ResponsibleAreaBrief);
                dic.Add("Status", _OutsideTroubleManModel.Status);
                dic.Add("ServiceNum", _OrderServices.getOTMServiceNum(_OutsideTroubleManModel.OutsideTroubleMan_ID));
                dic.Add("GoodRaty", _AssessmentServices.getGoodAssessment(_OutsideTroubleManModel.OutsideTroubleMan_ID));
                dic.Add("Role", Role);
                list.Add(dic);
            }
            var resonse = new Response();
            resonse.Code = 0;
            resonse.Message = "获取外线员列表成功！";
            resonse.Data = list;
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