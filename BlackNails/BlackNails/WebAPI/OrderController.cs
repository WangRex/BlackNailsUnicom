using BlackNails.Controllers;
using BlackNails.DAL;
using BlackNails.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BlackNails.WebAPI
{
    /// <summary>  
    /// 订单信息  
    /// </summary>  
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class OrderController : WebAPI2BaseController
    {
        private OrderServices _OrderServices = new OrderServices();
        private OrderHistoryServices _OrderHistoryServices = new OrderHistoryServices();
        private MatchingServices _MatchingServices = new MatchingServices();
        private CustomerServices _CustomerServices = new CustomerServices();
        private OutsideTroubleManServices _OutsideTroubleManServices = new OutsideTroubleManServices();
        private AssessmentServices _AssessmentServices = new AssessmentServices();
        log4net.ILog log = log4net.LogManager.GetLogger("testApp.Logging");

        /// <summary>  
        /// 用户创建订单
        /// </summary>  
        /// <param name="_OrderModel">订单详情</param>
        [HttpPost]
        public HttpResponseMessage CreateOrder(OrderModel _OrderModel)
        {
            log.Debug("OrderController.CreateOrder Start!");
            log.Debug("Address is " + _OrderModel.Address + ", MatchingAddress is " + _OrderModel.MatchingAddress + ", Phone is " + _OrderModel.Phone + ", Tel is " + _OrderModel.Tel + ", Type is " + _OrderModel.Type + ", Description is " + _OrderModel.Description);
            var response = new Response();
            response.Code = 0;
            response.Message = "添加订单成功！";
            CustomerModel _CustomerModel = new CustomerModel();
            _CustomerModel.CreatePerson = _OrderModel.Phone;
            _CustomerModel.CreateTime = DateTime.Now;
            _CustomerModel.UpdateTime = DateTime.Now;
            _CustomerModel.Name = _OrderModel.Name;
            _CustomerModel.Phone = _OrderModel.Phone;
            _CustomerModel.Tel = _OrderModel.Tel;
            _CustomerModel.Address = _OrderModel.Province + _OrderModel.City + _OrderModel.Area + _OrderModel.Address;
            _CustomerServices.Add(_CustomerModel);
            _OrderModel.CreatePerson = "Mobile";
            _OrderModel.CreateTime = DateTime.Now;
            _OrderModel.UpdateTime = DateTime.Now;
            _OrderModel.Assessment_ID = 0;
            _OrderModel.OTM_ID = _MatchingServices.getOTM_IDByAddress(_OrderModel.MatchingAddress);
            if (_OrderModel.OTM_ID == 0)
            {
                _OrderModel.Status = "新订单";
                response.Data = "暂无外线员";
                if (!string.Empty.Equals(_OrderModel.MatchingAddress))
                {
                    if (_MatchingServices.isAddressNotExist(_OrderModel.MatchingAddress))
                    {
                        MatchingModel _MatchingModel = new MatchingModel();
                        _MatchingModel.CreateTime = DateTime.Now;
                        _MatchingModel.UpdateTime = DateTime.Now;
                        _MatchingModel.CreatePerson = "System";
                        _MatchingModel.Address = _OrderModel.MatchingAddress;
                        _MatchingModel.OTM_ID = 0;
                        _MatchingServices.Add(_MatchingModel);
                    }
                }
            }
            else
            {
                _OrderModel.Status = "待接单";
                response.Code = 1;
                Dictionary<string, object> dic = new Dictionary<string, object>();
                OutsideTroubleManModel _OutsideTroubleManModel = _OutsideTroubleManServices.Find(_OrderModel.OTM_ID);
                dic.Add("Name", _OutsideTroubleManModel.Name);
                dic.Add("Phone", _OutsideTroubleManModel.Phone);
                dic.Add("EmployeeNo", _OutsideTroubleManModel.EmployeeNo);
                dic.Add("WorkYear", _OutsideTroubleManModel.WorkYear);
                dic.Add("Business", _OutsideTroubleManModel.Business);
                dic.Add("ResponsibleAreaBrief", _OutsideTroubleManModel.ResponsibleAreaBrief);
                response.Data = dic;
            }
            _OrderServices.Add(_OrderModel);
            OrderHistoryModel _OrderHistoryModel = new OrderHistoryModel();
            _OrderHistoryModel.CreatePerson = "System";
            _OrderHistoryModel.CreateTime = DateTime.Now;
            _OrderHistoryModel.Order_ID = _OrderModel.Order_ID;
            _OrderHistoryModel.Status = _OrderModel.Status;
            _OrderHistoryModel.UpdateTime = DateTime.Now;
            _OrderHistoryServices.Add(_OrderHistoryModel);
            return toJson(response);
        }

        /// <summary>  
        /// 外线员获取订单列表  
        /// </summary>  
        /// <param name="OTM_ID">外线员ID</param>
        /// <param name="status">订单状态</param>
        [HttpGet]
        public HttpResponseMessage GetOrdersByOTM(int OTM_ID, string status)
        {
            log.Debug("OrderController.GetOrdersByOTM Start!");
            log.Debug("OTM_ID is " + OTM_ID + ", status is " + status);
            var response = new Response();
            response.Code = 0;
            response.Message = "获取订单列表成功！";
            response.Data = _OrderServices.getOrdersByOTM(OTM_ID, status);
            return toJson(response);
        }

        /// <summary>  
        /// 获取订单
        /// </summary>  
        /// <param name="Order_ID">订单ID</param>
        [HttpGet]
        public HttpResponseMessage GetOrder(int Order_ID)
        {
            log.Debug("OrderController.AcceptOrderByOTM Start!");
            log.Debug("Order_ID is " + Order_ID);
            var response = new Response();
            response.Code = 0;
            response.Message = "获取订单成功！";
            OrderModel _OrderModel = _OrderServices.Find(Order_ID);
            response.Data = _OrderModel;
            return toJson(response);
        }

        /// <summary>  
        /// 外线员接受订单
        /// </summary>  
        /// <param name="OTM_ID">外线员ID</param>
        /// <param name="Order_ID">订单ID</param>
        [HttpGet]
        public HttpResponseMessage AcceptOrderByOTM(int OTM_ID, int Order_ID)
        {
            log.Debug("OrderController.AcceptOrderByOTM Start!");
            log.Debug("OTM_ID is " + OTM_ID + ", Order_ID is " + Order_ID);
            var response = new Response();
            response.Code = 0;
            response.Message = "接单成功！";
            OrderModel _OrderModel = _OrderServices.Find(Order_ID);
            _OrderModel.UpdatePerson = _OutsideTroubleManServices.Find(OTM_ID).Name;
            _OrderModel.UpdateTime = DateTime.Now;
            _OrderModel.Status = "处理中";
            _OrderServices.Update(_OrderModel);
            OrderHistoryModel _OrderHistoryModel = new OrderHistoryModel();
            _OrderHistoryModel.CreatePerson = _OrderModel.UpdatePerson;
            _OrderHistoryModel.CreateTime = DateTime.Now;
            _OrderHistoryModel.Order_ID = _OrderModel.Order_ID;
            _OrderHistoryModel.Status = _OrderModel.Status;
            _OrderHistoryModel.UpdateTime = DateTime.Now;
            _OrderHistoryServices.Add(_OrderHistoryModel);
            response.Data = true;
            return toJson(response);
        }

        /// <summary>  
        /// 外线员完成订单
        /// </summary>  
        /// <param name="OTM_ID">外线员ID</param>
        /// <param name="Order_ID">订单ID</param>
        [HttpGet]
        public HttpResponseMessage CompleteOrderByOTM(int OTM_ID, int Order_ID)
        {
            log.Debug("OrderController.CompleteOrderByOTM Start!");
            log.Debug("OTM_ID is " + OTM_ID + ", Order_ID is " + Order_ID);
            var response = new Response();
            response.Code = 0;
            response.Message = "外线员完成订单成功！";
            OrderModel _OrderModel = _OrderServices.Find(Order_ID);
            _OrderModel.UpdatePerson = _OutsideTroubleManServices.Find(OTM_ID).Name;
            _OrderModel.UpdateTime = DateTime.Now;
            _OrderModel.Status = "待评价";
            _OrderServices.Update(_OrderModel);
            OrderHistoryModel _OrderHistoryModel = new OrderHistoryModel();
            _OrderHistoryModel.CreatePerson = _OrderModel.UpdatePerson;
            _OrderHistoryModel.CreateTime = DateTime.Now;
            _OrderHistoryModel.Order_ID = _OrderModel.Order_ID;
            _OrderHistoryModel.Status = _OrderModel.Status;
            _OrderHistoryModel.UpdateTime = DateTime.Now;
            _OrderHistoryServices.Add(_OrderHistoryModel);
            response.Data = true;
            return toJson(response);
        }

        /// <summary>  
        /// 用户获取待评价订单列表
        /// </summary>  
        /// <param name="Phone">用户手机号</param>
        [HttpGet]
        public HttpResponseMessage GetWaitingAssessmentOrders(string Phone)
        {
            log.Debug("OrderController.GetWaitingAssessmentOrderByPhone Start!");
            log.Debug("Phone is " + Phone);
            var response = new Response();
            response.Code = 0;
            response.Message = "获取待评价订单列表成功！";
            response.Data = _OrderServices.getWaitingAssessmentOrders(Phone);
            return toJson(response);
        }

        /// <summary>  
        /// 用户获取评价订单详情
        /// </summary>  
        /// <param name="Phone">用户手机号</param>
        /// <param name="Order_ID">订单ID</param>
        [HttpGet]
        public HttpResponseMessage GetWaitingAssessmentOrder(string Phone, int Order_ID)
        {
            log.Debug("OrderController.GetWaitingAssessmentOrder Start!");
            log.Debug("Phone is " + Phone + ", Order_ID is " + Order_ID);
            var response = new Response();
            response.Code = 0;
            response.Message = "用户获取待评价订单成功！";
            OrderModel _OrderModel = _OrderServices.Find(Order_ID);
            OutsideTroubleManModel _OutsideTroubleManModel = _OutsideTroubleManServices.Find(_OrderModel.OTM_ID);
            var json = new
            {
                Name = _OutsideTroubleManModel.Name,
                Phone = _OutsideTroubleManModel.Phone,
                EmployeeNo = _OutsideTroubleManModel.EmployeeNo,
                WorkYear = _OutsideTroubleManModel.WorkYear,
                Business = _OutsideTroubleManModel.Business,
                ResponsibleAreaBrief = _OutsideTroubleManModel.ResponsibleAreaBrief,
                Description = _OrderModel.Description
            };
            response.Data = json;
            return toJson(response);
        }

        /// <summary>  
        /// 用户评价订单
        /// </summary>  
        /// <param name="_AssessmentModel">用户评价详情</param>
        [HttpPost]
        public HttpResponseMessage AssessmentOrder(AssessmentModel _AssessmentModel)
        {
            log.Debug("OrderController.GetWaitingAssessmentOrderByPhone Start!");
            log.Debug("Phone is " + _AssessmentModel.Phone + ", Order_ID is " + _AssessmentModel.Order_ID + ", OTM_ID is " + _AssessmentModel.OTM_ID + ", ComprehensiveStar is " + _AssessmentModel.ComprehensiveStar + ", AttitudeStar is " + _AssessmentModel.AttitudeStar + ", QuestionStar is " + _AssessmentModel.QuestionStar + ", Content is " + _AssessmentModel.Content);
            var response = new Response();
            response.Code = 0;
            response.Message = "用户评价订单成功！";
            _AssessmentModel.CreatePerson = _AssessmentModel.Phone;
            _AssessmentModel.CreateTime = DateTime.Now;
            _AssessmentModel.UpdateTime = DateTime.Now;
            _AssessmentServices.Add(_AssessmentModel);
            OrderModel _OrderModel = _OrderServices.Find(_AssessmentModel.Order_ID);
            _OrderModel.UpdatePerson = _AssessmentModel.Phone;
            _OrderModel.UpdateTime = DateTime.Now;
            _OrderModel.Status = "已完成";
            _OrderModel.Assessment_ID = _AssessmentModel.Assessment_ID;
            _OrderServices.Update(_OrderModel);
            OrderHistoryModel _OrderHistoryModel = new OrderHistoryModel();
            _OrderHistoryModel.CreatePerson = _OrderModel.UpdatePerson;
            _OrderHistoryModel.CreateTime = DateTime.Now;
            _OrderHistoryModel.Order_ID = _OrderModel.Order_ID;
            _OrderHistoryModel.Status = _OrderModel.Status;
            _OrderHistoryModel.UpdateTime = DateTime.Now;
            _OrderHistoryServices.Add(_OrderHistoryModel);
            response.Data = true;
            return toJson(response);
        }
    }
}