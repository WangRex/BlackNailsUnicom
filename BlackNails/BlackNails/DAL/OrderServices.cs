using BlackNails.Models;
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
    }
    public class OrderHistoryServices : BaseManager<OrderHistoryModel>
    {
    }
}