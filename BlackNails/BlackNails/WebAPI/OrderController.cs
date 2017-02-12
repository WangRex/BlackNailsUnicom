using BlackNails.Controllers;
using BlackNails.DAL;
using BlackNails.Models;
using System;
using System.Net.Http;
using System.Web.Http;

namespace BlackNails.WebAPI
{
    public class OrderController : WebAPI2BaseController
    {
        private OrderServices _OrderServices = new OrderServices();
        private OrderHistoryServices _OrderHistoryServices = new OrderHistoryServices();
        private MatchingServices _MatchingServices = new MatchingServices();
        private CustomerServices _CustomerServices = new CustomerServices();

        [HttpPost]
        public HttpResponseMessage CreateOrder(OrderModel _OrderModel)
        {
            var response = new Response();
            response.Code = 0;
            response.Message = "添加订单成功！";
            CustomerModel _CustomerModel = new CustomerModel();
            _CustomerModel.CreatePerson = "Mobile";
            _CustomerModel.CreateTime = DateTime.Now;
            _CustomerModel.UpdateTime = DateTime.Now;
            _CustomerModel.Name = _OrderModel.Name;
            _CustomerModel.Phone = _OrderModel.Phone;
            _CustomerModel.Tel = _OrderModel.Tel;
            _CustomerModel.Address = _OrderModel.Address;
            _CustomerServices.Add(_CustomerModel);
            _OrderModel.CreatePerson = "Mobile";
            _OrderModel.CreateTime = DateTime.Now;
            _OrderModel.UpdateTime = DateTime.Now;
            _OrderModel.Assessment_ID = 0;
            _OrderModel.OTM_ID = _MatchingServices.getOTM_IDByAddress(_OrderModel.MatchingAddress);
            if (_OrderModel.OTM_ID == 0)
            {
                _OrderModel.Status = "新订单";
            } else
            {
                _OrderModel.Status = "处理中";
            }
            _OrderServices.Add(_OrderModel);
            OrderHistoryModel _OrderHistoryModel = new OrderHistoryModel();
            _OrderHistoryModel.CreatePerson = "System";
            _OrderHistoryModel.CreateTime = DateTime.Now;
            _OrderHistoryModel.Order_ID = _OrderModel.Order_ID;
            _OrderHistoryModel.Status = _OrderModel.Status;
            _OrderHistoryModel.UpdateTime = DateTime.Now;
            _OrderHistoryServices.Add(_OrderHistoryModel);
            response.Data = _OrderModel;
            return toJson(response);
        }
    }
}