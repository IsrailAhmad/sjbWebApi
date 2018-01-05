using EverGreenWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EverGreenWebApi.Interfaces
{
    public interface IUserLoginRepository : IDisposable
    {
        UserLoginModel WebsiteLogin(string username, string password, int storeid);
        IEnumerable<DashboardModel> GetAllOrders(int storeid);
        IEnumerable<CustomerOrderModel> MyAllOrderListByStatusId(int StatusId, int StoreId);
        CustomerOrderModel GetOrderByOrderId(int orderid, int storeid);
        ResponseStatus AcceptOrder(int orderid, int storeid);
        ResponseStatus DeclineOrder(int orderid, int storeid);
        IEnumerable<CustomerOrderModel> DispatchOrder(int orderid, int storeid);
        CustomerOrderModel GetLatestOrderDetails(int StoreId);
        IEnumerable<CustomerOrderModel> GetAllOrdersByStoreList(int StoreId);
    }
}