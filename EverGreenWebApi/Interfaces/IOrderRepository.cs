using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EverGreenWebApi.Models;

namespace EverGreenWebApi.Interfaces
{
    public interface IOrderRepository:IDisposable
    {
        OrderModel CreateOrder(OrderModel model);
        OrderModel GetOrderByOrderNumber(string ordernumber);
        IEnumerable<OrderModel> GetAllOrderByUser(int loginid);
        OrderModel TrackOrderStatus(int loginid, string ordernumber);
        IEnumerable<OrderModel> MyAllOrderList(int loginid);
    }
}