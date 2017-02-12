﻿using BlackNails.Controllers;
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
        public HttpResponseMessage CreateOrder(CustomerModel _CustomerModel)
        {
            var response = new Response();
            response.Code = 0;
            response.Message = "添加订单成功！";
            _CustomerModel.CreatePerson = "Mobile";
            _CustomerModel.CreateTime = DateTime.Now;
            _CustomerModel.UpdateTime = DateTime.Now;
            _CustomerServices.Add(_CustomerModel);
            OrderModel _OrderModel = new OrderModel();
            _OrderModel.CreatePerson = "Mobile";
            _OrderModel.CreateTime = DateTime.Now;
            _OrderModel.UpdateTime = DateTime.Now;
            _OrderModel.OTM_ID = _MatchingServices.getOTM_IDByAddress(_CustomerModel.StreedRoad + _CustomerModel.Number);
            if (_OrderModel.OTM_ID == 0)
            {
                _OrderModel.Status = "新订单";
            } else
            {
                _OrderModel.Status = "处理中";
            }
            OrderHistoryModel _OrderHistoryModel = new OrderHistoryModel();
            _OrderHistoryModel.CreatePerson = "System";
            _OrderHistoryModel.CreateTime = DateTime.Now;
            _OrderHistoryModel.Order_ID = _OrderModel.Order_ID;
            _OrderHistoryModel.Status = _OrderModel.Status;
            _OrderHistoryModel.UpdateTime = DateTime.Now;

            response.Data = _OrderModel;
            return toJson(response);
        }
    }
}