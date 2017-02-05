using BlackNails.DAL;
using BlackNails.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BlackNails.Controllers
{
    public class OrderController : MVC5_BaseController
    {
        private OrderServices _OrderServices = new OrderServices();
        private OrderHistoryServices _OrderHistoryServices = new OrderHistoryServices();
        private OutsideTroubleManServices _OutsideTroubleManServices = new OutsideTroubleManServices();
        private AssessmentServices _AssessmentServices = new AssessmentServices();

        public ActionResult Index()
        {
            ViewBag.Title = "订单列表";
            return View();
        }

        [HttpGet]
        public ActionResult MyJsonList()
        {
            var Role = Session["RoleName"].ToString();
            var OrderJson = _OrderServices.FindList().ToList();

            var list = new List<object>();
            foreach (OrderModel _OrderModel in OrderJson) {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("Order_ID", _OrderModel.Order_ID);
                dic.Add("Time", _OrderModel.Time);
                dic.Add("Address", _OrderModel.Address);
                dic.Add("Phone", _OrderModel.Phone);
                dic.Add("Tel", _OrderModel.Tel);
                dic.Add("Type", _OrderModel.Type);
                dic.Add("Status", _OrderModel.Status);
                dic.Add("OTM_ID", _OrderModel.OTM_ID);
                dic.Add("OTMName", _OutsideTroubleManServices.getOTMName(_OrderModel.OTM_ID));
                dic.Add("AssessmentContent", _AssessmentServices.getAssessment(_OrderModel.Assessment_ID).Content);
                dic.Add("Role", Role);
                list.Add(dic);
            }
            var resonse = new Response();
            resonse.Code = 0;
            resonse.Message = "获取订单列表成功！";
            resonse.Data = list;
            return Json(resonse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewPage(int Order_ID)
        {
            ViewBag.Title = "订单详情";
            TempData.Add("Order_ID", Order_ID);
            return View();
        }

        [HttpGet]
        public ActionResult MyJsonOrder(int Order_ID)
        {
            var Role = Session["RoleName"].ToString();
            OrderModel _OrderModel = _OrderServices.Find(Order_ID);
            Dictionary<string, object> dic = new Dictionary<string, object>();

            dic.Add("Order_ID", _OrderModel.Order_ID);
            dic.Add("Time", _OrderModel.Time);
            dic.Add("Address", _OrderModel.Address);
            dic.Add("Phone", _OrderModel.Phone);
            dic.Add("Tel", _OrderModel.Tel);
            dic.Add("Type", _OrderModel.Type);
            dic.Add("Status", _OrderModel.Status);
            dic.Add("OTM_ID", _OrderModel.OTM_ID);
            dic.Add("OTMName", _OutsideTroubleManServices.getOTMName(_OrderModel.OTM_ID));
            dic.Add("Role", Role);
            dic.Add("Content", _AssessmentServices.getAssessment(_OrderModel.Assessment_ID).Content);
            dic.Add("Star", _AssessmentServices.getAssessment(_OrderModel.Assessment_ID).Star);

            var resonse = new Response();
            resonse.Code = 0;
            resonse.Message = "获取订单详情成功！";
            resonse.Data = dic;
            return Json(resonse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Bind(int Order_ID, int OTM_ID)
        {
            OrderModel _OrderModel = _OrderServices.Find(Order_ID);
            _OrderModel.OTM_ID = OTM_ID;
            _OrderModel.Status = "处理中";
            _OrderModel.UpdatePerson = "System";
            _OrderModel.UpdateTime = DateTime.Now;
            _OrderServices.Update(_OrderModel);

            OrderHistoryModel _OrderHistoryModel = new OrderHistoryModel();
            _OrderHistoryModel.CreatePerson = Session["UserName"].ToString();
            _OrderHistoryModel.CreateTime = DateTime.Now;
            _OrderHistoryModel.Order_ID = _OrderModel.Order_ID;
            _OrderHistoryModel.Status = _OrderModel.Status;
            _OrderHistoryServices.Add(_OrderHistoryModel);

            var resonse = new Response();
            resonse.Code = 0;
            resonse.Message = "修改订单详情成功！";
            resonse.Data = _OrderModel;
            return Json(resonse, JsonRequestBehavior.AllowGet);
        }
    }
}