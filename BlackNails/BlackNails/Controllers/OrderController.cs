using BlackNails.CommonClass;
using BlackNails.DAL;
using BlackNails.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BlackNails.Controllers
{
    /// <summary>
    /// 订单管理
    /// </summary>
    public class OrderController : MVC5_BaseController
    {
        private OrderServices _OrderServices = new OrderServices();
        private OrderHistoryServices _OrderHistoryServices = new OrderHistoryServices();
        private OutsideTroubleManServices _OutsideTroubleManServices = new OutsideTroubleManServices();
        private AssessmentServices _AssessmentServices = new AssessmentServices();
        private MatchingServices _MatchingServices = new MatchingServices();

        /// <summary>
        /// 订单列表首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Title = "订单列表";
            return View();
        }

        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MyJsonList()
        {
            var Role = Session["RoleName"].ToString();
            var OrderJson = _OrderServices.FindList().ToList();

            var list = new List<object>();
            foreach (OrderModel _OrderModel in OrderJson) {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("Order_ID", _OrderModel.Order_ID);
                dic.Add("Time", _OrderModel.CreateTime.ToString());
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

        /// <summary>
        /// 获取订单详情跳转页
        /// </summary>
        /// <param name="Order_ID">订单ID</param>
        /// <returns></returns>
        public ActionResult ViewPage(int Order_ID)
        {
            ViewBag.Title = "订单详情";
            TempData.Add("Order_ID", Order_ID);
            return View();
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="Order_ID">订单ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MyJsonOrder(int Order_ID)
        {
            var Role = Session["RoleName"].ToString();
            OrderModel _OrderModel = _OrderServices.Find(Order_ID);
            Dictionary<string, object> dic = new Dictionary<string, object>();

            dic.Add("Order_ID", _OrderModel.Order_ID);
            dic.Add("Time", _OrderModel.CreateTime.ToShortDateString());
            dic.Add("Address", _OrderModel.Address);
            dic.Add("Phone", _OrderModel.Phone);
            dic.Add("Tel", _OrderModel.Tel);
            dic.Add("Type", _OrderModel.Type);
            dic.Add("Status", _OrderModel.Status);
            dic.Add("OTM_ID", _OrderModel.OTM_ID);
            dic.Add("OTMName", _OutsideTroubleManServices.getOTMName(_OrderModel.OTM_ID));
            dic.Add("Role", Role);
            dic.Add("Content", _AssessmentServices.getAssessment(_OrderModel.Assessment_ID).Content);
            dic.Add("Star", _AssessmentServices.getAssessment(_OrderModel.Assessment_ID).ComprehensiveStar);

            var resonse = new Response();
            resonse.Code = 0;
            resonse.Message = "获取订单详情成功！";
            resonse.Data = dic;
            return Json(resonse, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 指派外线员
        /// </summary>
        /// <param name="Order_ID">订单ID</param>
        /// <param name="OTM_ID">外线员ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Bind(int Order_ID, int OTM_ID)
        {
            OrderModel _OrderModel = _OrderServices.Find(Order_ID);
            _OrderModel.OTM_ID = OTM_ID;
            _OrderModel.Status = "待接单";
            _OrderModel.UpdatePerson = "System";
            _OrderModel.UpdateTime = DateTime.Now;
            _OrderServices.Update(_OrderModel);

            OrderHistoryModel _OrderHistoryModel = new OrderHistoryModel();
            _OrderHistoryModel.CreatePerson = Session["UserName"].ToString();
            _OrderHistoryModel.CreateTime = DateTime.Now;
            _OrderHistoryModel.UpdateTime = DateTime.Now;
            _OrderHistoryModel.Order_ID = _OrderModel.Order_ID;
            _OrderHistoryModel.Status = _OrderModel.Status;
            _OrderHistoryServices.Add(_OrderHistoryModel);

            if (!string.Empty.Equals(_OrderModel.MatchingAddress))
            {
                if (_MatchingServices.isAddressNotExist(_OrderModel.MatchingAddress))
                {
                    MatchingModel _MatchingModel = new MatchingModel();
                    _MatchingModel.CreateTime = DateTime.Now;
                    _MatchingModel.UpdateTime = DateTime.Now;
                    _MatchingModel.CreatePerson = "System";
                    _MatchingModel.Address = _OrderModel.MatchingAddress;
                    _MatchingModel.OTM_ID = _OrderModel.OTM_ID;
                    _MatchingServices.Add(_MatchingModel);
                }
            }

            var resonse = new Response();
            resonse.Code = 0;
            resonse.Message = "修改订单详情成功！";
            resonse.Data = _OrderModel;
            return Json(resonse, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 首页合同数统计
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MyJsonListCount()
        {
            var Role = Session["RoleName"].ToString();
            var OrderJson = _OrderServices.FindList().ToList();

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("New", _OrderServices.FindList().Where(om => om.Status == "新订单").Count());
            dic.Add("Dealing", _OrderServices.FindList().Where(om => om.Status == "处理中").Count());
            dic.Add("Complete", _OrderServices.FindList().Where(om => om.Status == "已完成").Count());

            var resonse = new Response();
            resonse.Code = 0;
            resonse.Message = "获取订单数量成功！";
            resonse.Data = dic;
            return Json(resonse, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="Order_ID">订单ID</param>
        /// <returns></returns>
        public ActionResult DeleteOrder(int Order_ID)
        {
            ViewBag.Title = "删除订单";
            OrderModel _OrderModel = _OrderServices.Find(Order_ID);
            OrderHistoryModel _OrderHistoryModel = new OrderHistoryModel();
            _OrderHistoryModel.CreatePerson = Session["UserName"].ToString();
            _OrderHistoryModel.CreateTime = DateTime.Now;
            _OrderHistoryModel.UpdateTime = DateTime.Now;
            _OrderHistoryModel.Order_ID = _OrderModel.Order_ID;
            _OrderHistoryModel.Status = "删除订单";
            _OrderHistoryServices.Add(_OrderHistoryModel);
            _OrderServices.Delete(Order_ID);
            var resonse = new Response();
            resonse.Code = 0;
            resonse.Message = "删除订单成功！";
            resonse.Data = _OrderModel;
            return Json(resonse, JsonRequestBehavior.AllowGet);
        }
    }
}