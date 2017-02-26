using BlackNails.Models;
using System.Collections.Generic;
using System.Linq;

namespace BlackNails.DAL
{
    public class OrderServices : BaseManager<OrderModel>
    {
        public int getOTMServiceNum(int OTM_ID)
        {
            //获取实体列表
            IQueryable<OrderModel> _Orders = base.Repository.FindList().Where(om => om.OTM_ID == OTM_ID && om.Status == "已完成");
            return _Orders.Count();
        }
        public List<OrderModel> getOrdersByOTM(int OTM_ID, string status)
        {
            var _Orders = base.Repository.FindList().Where(om => om.OTM_ID == OTM_ID && om.Status == status).ToList();
            return _Orders;
        }
        public List<OrderModel> getWaitingAssessmentOrders(string Phone, string Status)
        {
            //获取实体列表
            IQueryable<OrderModel> _Orders = base.Repository.FindList().Where(om => om.Phone == Phone && om.Status.Equals(Status));
            return _Orders.ToList();
        }
    }
    public class OrderHistoryServices : BaseManager<OrderHistoryModel>
    {
    }
}