using EverGreenWebApi.DBHelper;
using EverGreenWebApi.Interfaces;
using EverGreenWebApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace EverGreenWebApi.Repository
{
    public class UserLoginRepository : IUserLoginRepository
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public UserLoginModel WebsiteLogin(string username, string password, int storeid)
        {
            UserLoginModel data = new UserLoginModel();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                //var result = context.website_login.Where(u => u.UserName == username && u.Password == password && u.StoreId == storeid && u.IsActive == "Y")
                //    .Select(u => new UserLoginModel()
                //    {
                //        UserID = u.UserId,
                //        StoreId = (int)u.StoreId,
                //        UserName = u.UserName,
                //        FirstName = u.FisrtName,
                //        LastName = u.LastName
                //    }).FirstOrDefault();
                //return result;
                if (storeid == 0)
                {
                    data = (from u in context.website_login
                            join s in context.storemasters on u.StoreId equals s.StoreId into j1
                            from j2 in j1.DefaultIfEmpty()
                            where u.UserName == username && u.Password == password && u.IsActive == "Y"
                            select new UserLoginModel()
                            {
                                UserID = u.UserId,
                                StoreId = (int)u.StoreId,
                                UserName = u.UserName,
                                FirstName = u.FisrtName,
                                LastName = u.LastName,
                                StoreStatus = j2.StoreStatus == "Y" ? true : false,
                            }).FirstOrDefault();
                }
                else if (storeid > 0)
                {
                    data = (from u in context.website_login
                            join s in context.storemasters on u.StoreId equals s.StoreId into j1
                            from j2 in j1.DefaultIfEmpty()
                            where u.UserName == username && u.Password == password && u.StoreId == storeid && u.IsActive == "Y"
                            select new UserLoginModel()
                            {
                                UserID = u.UserId,
                                StoreId = (int)u.StoreId,
                                UserName = u.UserName,
                                FirstName = u.FisrtName,
                                LastName = u.LastName,
                                StoreStatus = j2.StoreStatus == "Y" ? true : false,
                            }).FirstOrDefault();
                }

                return data;
            }
        }
        public IEnumerable<DashboardModel> GetAllOrders(int storeid)
        {
            List<DashboardModel> orderList = new List<DashboardModel>();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                if (storeid == 0)
                {
                    orderList = (from o in context.ordermasters
                                 join s in context.orderstatusmasters on o.OrderStatusId equals s.OrderStatusId into j1
                                 from j2 in j1.DefaultIfEmpty()
                                 join t in context.transactionmasters on o.OrderNumber equals t.OrderNumber
                                 //into j3
                                 //from j4 in j3.DefaultIfEmpty()
                                 //where o.StoreId == storeid
                                 group j2 by new { j2.OrderStatusId, j2.OrderStatus }
                                    into g
                                 select new DashboardModel()
                                 {
                                     OrderStatusId = g.Key.OrderStatusId,
                                     OrderStatus = g.Key.OrderStatus,
                                     Count = g.Count()
                                 }).ToList();
                }
                else
                {
                    orderList = (from o in context.ordermasters
                                 join s in context.orderstatusmasters on o.OrderStatusId equals s.OrderStatusId into j1
                                 from j2 in j1.DefaultIfEmpty()
                                 join t in context.transactionmasters on o.OrderNumber equals t.OrderNumber
                                 //into j3
                                 //from j4 in j3.DefaultIfEmpty()
                                 where o.StoreId == storeid
                                 group j2 by new { j2.OrderStatusId, j2.OrderStatus }
                                    into g
                                 select new DashboardModel()
                                 {
                                     OrderStatusId = g.Key.OrderStatusId,
                                     OrderStatus = g.Key.OrderStatus,
                                     Count = g.Count()
                                 }).ToList();
                }
                return orderList;
            }
        }
        public IEnumerable<CustomerOrderModel> MyAllOrderListByStatusId(int StatusId, int StoreId)
        {
            List<CustomerOrderModel> order = new List<CustomerOrderModel>();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                if (StoreId == 0)
                {

                    order = (from x in context.ordermasters
                             join ts in context.transactionmasters on x.OrderNumber equals ts.OrderNumber 
                             join s in context.orderstatusmasters on x.OrderStatusId equals s.OrderStatusId into j1
                             from j2 in j1.DefaultIfEmpty()
                             join u in context.registrationmasters on x.LoginId equals u.LoginID into j3
                             from j4 in j3.DefaultIfEmpty()
                             join a in context.addressmasters on x.AddressId equals a.AddressId into j5
                             from j6 in j5.DefaultIfEmpty()
                             join t in context.storemasters on x.StoreId equals t.StoreId into j7
                             from j8 in j7.DefaultIfEmpty()
                             where x.OrderStatusId == StatusId
                             orderby x.OrderId descending
                             select new CustomerOrderModel()
                             {
                                 OrderId = x.OrderId,
                                 OrderNumber = x.OrderNumber,
                                 Address = j6.CompleteAddress,
                                 CustomerName = j4.Name,
                                 PhoneNumber = j4.PhoneNumber,
                                 Email = j4.EmailID,
                                 OrderTime = (DateTime)x.CreatedOn,
                                 OrderStatus = j2.OrderStatus,
                                 TotalPrice = (decimal)x.TotalPrice,
                                 NetPrice = (decimal)x.NetAmount,
                                 TranscationId = ts.TransactionId
                             }).ToList();
                }
                else
                {

                    order = (from x in context.ordermasters
                             join ts in context.transactionmasters on x.OrderNumber equals ts.OrderNumber
                             join s in context.orderstatusmasters on x.OrderStatusId equals s.OrderStatusId into j1
                             from j2 in j1.DefaultIfEmpty()
                             join u in context.registrationmasters on x.LoginId equals u.LoginID into j3
                             from j4 in j3.DefaultIfEmpty()
                             join a in context.addressmasters on x.AddressId equals a.AddressId into j5
                             from j6 in j5.DefaultIfEmpty()
                             join t in context.storemasters on x.StoreId equals t.StoreId into j7
                             from j8 in j7.DefaultIfEmpty()
                             where x.OrderStatusId == StatusId && x.StoreId == StoreId
                             orderby x.OrderId descending
                             select new CustomerOrderModel()
                             {
                                 OrderId = x.OrderId,
                                 OrderNumber = x.OrderNumber,
                                 Address = j6.CompleteAddress,
                                 CustomerName = j4.Name,
                                 PhoneNumber = j4.PhoneNumber,
                                 Email = j4.EmailID,
                                 OrderTime = (DateTime)x.CreatedOn,
                                 OrderStatus = j2.OrderStatus,
                                 TotalPrice = (decimal)x.TotalPrice,
                                 NetPrice = (decimal)x.NetAmount,
                                 TranscationId = ts.TransactionId
                             }).ToList();
                }

                return order;
            }
        }
        public CustomerOrderModel GetOrderByOrderId(int orderid, int storeid)
        {
            CustomerOrderModel order = new CustomerOrderModel();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                if (storeid == 0)
                {
                    order = (from x in context.ordermasters
                             join s in context.orderstatusmasters on x.OrderStatusId equals s.OrderStatusId
                             join p in context.promocodemasters on x.LoginId equals p.LoginId into j1
                             from j2 in j1.DefaultIfEmpty()
                             join m in context.registrationmasters on x.LoginId equals m.LoginID into j3
                             from j4 in j3.DefaultIfEmpty()
                             join a in context.addressmasters on x.AddressId equals a.AddressId into j5
                             from j6 in j5.DefaultIfEmpty()
                             join t in context.transactionmasters on x.OrderNumber equals t.OrderNumber into j7
                             from j8 in j7.DefaultIfEmpty()
                             where x.OrderId == orderid
                             orderby x.OrderId descending
                             select new CustomerOrderModel()
                             {
                                 LoginId = (int)x.LoginId,
                                 CustomerName = j4.Name,
                                 PhoneNumber = j4.PhoneNumber,
                                 Email = j4.EmailID,
                                 OrderId = x.OrderId,
                                 OrderNumber = x.OrderNumber,
                                 StoreId = (int)x.StoreId,
                                 AddressId = (int)x.AddressId,
                                 Address = j6.CompleteAddress + "," + j6.LandMark + "," + j6.ZipCode,
                                 ZipCode = j6.ZipCode,
                                 LandMark = j6.LandMark,
                                 PromoCode = j2.PromoCode,
                                 TotalPrice = (decimal)x.TotalPrice,
                                 GrandTotal = (decimal)x.GrandTotal,
                                 SpecialDiscount = (decimal)x.SpecialDiscount,
                                 PromoDiscount = (decimal)x.Discount,
                                 TotalGST = (decimal)x.TotalGST,
                                 NetPrice = (decimal)x.NetAmount,
                                 OrderTime = (DateTime)x.CreatedOn,
                                 SubTotal = (decimal)x.SubTotal,
                                 OrderStatus = s.OrderStatus,
                                 PaymentMode = j8.PaymentMode,
                                 DeliveryCharge = (decimal)x.TotalDeliveryCharge,
                             }).FirstOrDefault();

                    var orderdetails = (from z in context.ordermasters
                                        join r in context.orderdetails on z.OrderNumber equals r.OrderNumber
                                        join p in context.productmasters on r.ProductId equals p.ProductId
                                        where r.OrderNumber == order.OrderNumber
                                        orderby r.OrderDetailsId descending
                                        select new OrderDetailsModel()
                                        {
                                            ProductName = p.ProductName,
                                            UnitPrice = (decimal)p.UnitPrice,
                                            ProductId = (int)r.ProductId,
                                            Quantity = (decimal)r.Quantity,
                                            UOM = p.UOM,
                                            //ProductPicturesUrl = path + r.ProductId + "ProductPictures.jpg"
                                        }).ToList();
                    order.OrderDetails = orderdetails;
                }
                else
                {
                    order = (from x in context.ordermasters
                             join s in context.orderstatusmasters on x.OrderStatusId equals s.OrderStatusId
                             join p in context.promocodemasters on x.LoginId equals p.LoginId into j1
                             from j2 in j1.DefaultIfEmpty()
                             join m in context.registrationmasters on x.LoginId equals m.LoginID into j3
                             from j4 in j3.DefaultIfEmpty()
                             join a in context.addressmasters on x.AddressId equals a.AddressId into j5
                             from j6 in j5.DefaultIfEmpty()
                             join t in context.transactionmasters on x.OrderNumber equals t.OrderNumber into j7
                             from j8 in j7.DefaultIfEmpty()
                             where x.OrderId == orderid && x.StoreId == storeid
                             orderby x.OrderId descending
                             select new CustomerOrderModel()
                             {
                                 LoginId = (int)x.LoginId,
                                 CustomerName = j4.Name,
                                 PhoneNumber = j4.PhoneNumber,
                                 Email = j4.EmailID,
                                 OrderId = x.OrderId,
                                 OrderNumber = x.OrderNumber,
                                 StoreId = (int)x.StoreId,
                                 AddressId = (int)x.AddressId,
                                 Address = j6.CompleteAddress + "," + j6.LandMark + "," + j6.ZipCode,
                                 ZipCode = j6.ZipCode,
                                 LandMark = j6.LandMark,
                                 PromoCode = j2.PromoCode,
                                 TotalPrice = (decimal)x.TotalPrice,
                                 GrandTotal = (decimal)x.GrandTotal,
                                 SpecialDiscount = (decimal)x.SpecialDiscount,
                                 PromoDiscount = (decimal)x.Discount,
                                 TotalGST = (decimal)x.TotalGST,
                                 NetPrice = (decimal)x.NetAmount,
                                 OrderTime = (DateTime)x.CreatedOn,
                                 SubTotal = (decimal)x.SubTotal,
                                 OrderStatus = s.OrderStatus,
                                 PaymentMode = j8.PaymentMode,
                                 DeliveryCharge = (decimal)x.TotalDeliveryCharge,
                             }).FirstOrDefault();

                    var orderdetails = (from z in context.ordermasters
                                        join r in context.orderdetails on z.OrderNumber equals r.OrderNumber
                                        join p in context.productmasters on r.ProductId equals p.ProductId
                                        where r.OrderNumber == order.OrderNumber
                                        orderby r.OrderDetailsId descending
                                        select new OrderDetailsModel()
                                        {
                                            ProductName = p.ProductName,
                                            UnitPrice = (decimal)p.UnitPrice,
                                            ProductId = (int)r.ProductId,
                                            Quantity = (decimal)r.Quantity,
                                            UOM = p.UOM,
                                            //ProductPicturesUrl = path + r.ProductId + "ProductPictures.jpg"
                                        }).ToList();
                    order.OrderDetails = orderdetails;
                }
                return order;
            }
        }
        public ResponseStatus AcceptOrder(int orderid, int storeid)
        {
            ResponseStatus response = new ResponseStatus();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                if (storeid == 0)
                {
                    var data = context.ordermasters.Where(w => w.OrderId == orderid);
                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            item.OrderStatusId = 2;
                        }
                        var result = context.SaveChanges();
                        if (result > 0)
                        {
                            GetOrderDetails(orderid, storeid);
                            response.isSuccess = true;
                            response.serverResponseTime = DateTime.Now;
                        }
                    }
                    else
                    {
                        response.isSuccess = false;
                        response.serverResponseTime = DateTime.Now;
                    }
                }
                else
                {
                    var data = context.ordermasters.Where(w => w.OrderId == orderid && w.StoreId == storeid);
                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            item.OrderStatusId = 2;
                        }
                        var result = context.SaveChanges();
                        if (result > 0)
                        {
                            GetOrderDetails(orderid, storeid);
                            response.isSuccess = true;
                            response.serverResponseTime = DateTime.Now;
                        }
                    }
                    else
                    {
                        response.isSuccess = false;
                        response.serverResponseTime = DateTime.Now;
                    }
                }
            }
            return response;
        }
        public ResponseStatus DeclineOrder(int orderid, int storeid)
        {
            ResponseStatus response = new ResponseStatus();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                if (storeid == 0)
                {
                    var data = context.ordermasters.Where(w => w.OrderId == orderid);
                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            item.OrderStatusId = 6;
                        }
                        var result = context.SaveChanges();
                        if (result > 0)
                        {
                            GetOrderDetails(orderid, storeid);
                            response.isSuccess = true;
                            response.serverResponseTime = DateTime.Now;
                        }
                    }
                    else
                    {
                        response.isSuccess = false;
                        response.serverResponseTime = DateTime.Now;
                    }
                }
                else
                {
                    var data = context.ordermasters.Where(w => w.OrderId == orderid && w.StoreId == storeid);
                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            item.OrderStatusId = 6;
                        }
                        var result = context.SaveChanges();
                        if (result > 0)
                        {
                            GetOrderDetails(orderid, storeid);
                            response.isSuccess = true;
                            response.serverResponseTime = DateTime.Now;
                        }
                    }
                    else
                    {
                        response.isSuccess = false;
                        response.serverResponseTime = DateTime.Now;
                    }
                }
            }
            return response;
        }
        public IEnumerable<CustomerOrderModel> DispatchOrder(int orderid, int storeid)
        {
            List<CustomerOrderModel> order = new List<CustomerOrderModel>();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                if (storeid == 0)
                {
                    var data = context.ordermasters.Where(w => w.OrderId == orderid);
                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            item.OrderStatusId = 3;
                        }
                        var result = context.SaveChanges();
                        if (result > 0)
                        {


                            order = (from x in context.ordermasters
                                     join ts in context.transactionmasters on x.OrderNumber equals ts.OrderNumber
                                     join s in context.orderstatusmasters on x.OrderStatusId equals s.OrderStatusId into j1
                                     from j2 in j1.DefaultIfEmpty()
                                     join u in context.registrationmasters on x.LoginId equals u.LoginID into j3
                                     from j4 in j3.DefaultIfEmpty()
                                     join a in context.addressmasters on x.AddressId equals a.AddressId into j5
                                     from j6 in j5.DefaultIfEmpty()
                                     join t in context.storemasters on x.StoreId equals t.StoreId into j7
                                     from j8 in j7.DefaultIfEmpty()
                                     where x.StoreId == storeid && x.OrderStatusId == 2
                                     orderby x.OrderId descending
                                     select new CustomerOrderModel()
                                     {
                                         OrderId = x.OrderId,
                                         OrderNumber = x.OrderNumber,
                                         Address = j6.CompleteAddress,
                                         CustomerName = j4.Name,
                                         PhoneNumber = j4.PhoneNumber,
                                         Email = j4.EmailID,
                                         OrderTime = (DateTime)x.CreatedOn,
                                         OrderStatus = j2.OrderStatus,
                                         TotalPrice = (decimal)x.TotalPrice,
                                         NetPrice = (decimal)x.NetAmount,
                                         TranscationId = x.TranactionId
                                     }).ToList();
                            GetOrderDetails(orderid, storeid);
                        }
                    }
                }
                else
                {
                    var data = context.ordermasters.Where(w => w.OrderId == orderid && w.StoreId == storeid);
                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            item.OrderStatusId = 3;
                        }
                        var result = context.SaveChanges();
                        if (result > 0)
                        {


                            order = (from x in context.ordermasters
                                     join ts in context.transactionmasters on x.OrderNumber equals ts.OrderNumber
                                     join s in context.orderstatusmasters on x.OrderStatusId equals s.OrderStatusId into j1
                                     from j2 in j1.DefaultIfEmpty()
                                     join u in context.registrationmasters on x.LoginId equals u.LoginID into j3
                                     from j4 in j3.DefaultIfEmpty()
                                     join a in context.addressmasters on x.AddressId equals a.AddressId into j5
                                     from j6 in j5.DefaultIfEmpty()
                                     join t in context.storemasters on x.StoreId equals t.StoreId into j7
                                     from j8 in j7.DefaultIfEmpty()
                                     where x.StoreId == storeid && x.OrderStatusId == 2
                                     orderby x.OrderId descending
                                     select new CustomerOrderModel()
                                     {
                                         OrderId = x.OrderId,
                                         OrderNumber = x.OrderNumber,
                                         Address = j6.CompleteAddress,
                                         CustomerName = j4.Name,
                                         PhoneNumber = j4.PhoneNumber,
                                         Email = j4.EmailID,
                                         OrderTime = (DateTime)x.CreatedOn,
                                         OrderStatus = j2.OrderStatus,
                                         TotalPrice = (decimal)x.TotalPrice,
                                         NetPrice = (decimal)x.NetAmount,
                                         TranscationId = x.TranactionId
                                     }).ToList();
                            GetOrderDetails(orderid, storeid);
                        }
                    }
                }
                return order;
            }
        }
        public OrderModel GetOrderDetails(int orderid, int storeid)
        {
            OrderModel data = new OrderModel();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                if (storeid == 0)
                {

                    data = (from x in context.ordermasters
                            join s in context.orderstatusmasters on x.OrderStatusId equals s.OrderStatusId
                            join p in context.promocodemasters on x.LoginId equals p.LoginId into j1
                            from j2 in j1.DefaultIfEmpty()
                            join c in context.registrationmasters on x.LoginId equals c.LoginID into j3
                            from j4 in j3.DefaultIfEmpty()
                            join st in context.storemasters on x.StoreId equals st.StoreId into j5
                            from j6 in j5.DefaultIfEmpty()
                                //join m in context.orderdetails on x.OrderNumber equals m.OrderNumber
                            where x.OrderId == orderid
                            orderby x.OrderId descending
                            select new OrderModel()
                            {
                                OrderId = x.OrderId,
                                CustomerName = j4.Name,
                                PhoneNumber = j4.PhoneNumber,
                                EmailId = j4.EmailID,
                                DeviceId = j4.DeviceId,
                                OrderNumber = x.OrderNumber,
                                StoreId = (int)x.StoreId,
                                StoreName = j6.StoreName,
                                StorePhoneNumber = j6.StorePhoneNumber,
                                AddressId = (int)x.AddressId,
                                LoginId = (int)x.LoginId,
                                PromoCode = j2.PromoCode,
                                TotalPrice = (decimal)x.TotalPrice,
                                GrandTotal = (decimal)x.GrandTotal,
                                SpecialDiscount = (decimal)x.SpecialDiscount,
                                PromoDiscount = (decimal)x.Discount,
                                NetPrice = (decimal)x.NetAmount,
                                SubTotal = (decimal)x.SubTotal,
                                TotalGST = (decimal)x.TotalGST,
                                OrderTime = (DateTime)x.CreatedOn,
                                OrderStatus = s.OrderStatus,
                                DeliveryCharge = (decimal)x.TotalDeliveryCharge,
                            }).First();
                }
                else
                {

                    data = (from x in context.ordermasters
                            join s in context.orderstatusmasters on x.OrderStatusId equals s.OrderStatusId
                            join p in context.promocodemasters on x.LoginId equals p.LoginId into j1
                            from j2 in j1.DefaultIfEmpty()
                            join c in context.registrationmasters on x.LoginId equals c.LoginID into j3
                            from j4 in j3.DefaultIfEmpty()
                            join st in context.storemasters on x.StoreId equals st.StoreId into j5
                            from j6 in j5.DefaultIfEmpty()
                                //join m in context.orderdetails on x.OrderNumber equals m.OrderNumber
                            where x.OrderId == orderid && x.StoreId == storeid
                            orderby x.OrderId descending
                            select new OrderModel()
                            {
                                OrderId = x.OrderId,
                                CustomerName = j4.Name,
                                PhoneNumber = j4.PhoneNumber,
                                EmailId = j4.EmailID,
                                DeviceId = j4.DeviceId,
                                OrderNumber = x.OrderNumber,
                                StoreId = (int)x.StoreId,
                                StoreName = j6.StoreName,
                                StorePhoneNumber = j6.StorePhoneNumber,
                                AddressId = (int)x.AddressId,
                                LoginId = (int)x.LoginId,
                                PromoCode = j2.PromoCode,
                                TotalPrice = (decimal)x.TotalPrice,
                                GrandTotal = (decimal)x.GrandTotal,
                                SpecialDiscount = (decimal)x.SpecialDiscount,
                                PromoDiscount = (decimal)x.Discount,
                                NetPrice = (decimal)x.NetAmount,
                                SubTotal = (decimal)x.SubTotal,
                                TotalGST = (decimal)x.TotalGST,
                                OrderTime = (DateTime)x.CreatedOn,
                                OrderStatus = s.OrderStatus,
                                DeliveryCharge = (decimal)x.TotalDeliveryCharge,
                            }).First();
                }

                var orderdetails = (from z in context.ordermasters
                                    join r in context.orderdetails on z.OrderNumber equals r.OrderNumber
                                    join p in context.productmasters on r.ProductId equals p.ProductId
                                    where r.OrderNumber == data.OrderNumber
                                    orderby r.OrderDetailsId descending
                                    select new OrderDetailsModel()
                                    {
                                        ProductName = p.ProductName,
                                        UnitPrice = (decimal)p.UnitPrice,
                                        ProductId = (int)r.ProductId,
                                        Quantity = (decimal)r.Quantity,
                                        UOM = p.UOM,
                                        //ProductPicturesUrl = path + r.ProductId + "ProductPictures.jpg"
                                    }).ToList();
                data.OrderDetails = orderdetails;

                if (data.OrderId > 0)
                {
                    SendSMS(data);
                    SendEmail(data);
                    SendNotification(data);
                }
            }
            return data;
        }
        public CustomerOrderModel GetLatestOrderDetails(int StoreId)
        {
            CustomerOrderModel order = new CustomerOrderModel();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                if (StoreId == 0)
                {

                    order = (from x in context.ordermasters
                             join ts in context.transactionmasters on x.OrderNumber equals ts.OrderNumber
                             join s in context.orderstatusmasters on x.OrderStatusId equals s.OrderStatusId into j1
                             from j2 in j1.DefaultIfEmpty()
                             join u in context.registrationmasters on x.LoginId equals u.LoginID into j3
                             from j4 in j3.DefaultIfEmpty()
                             join a in context.addressmasters on x.AddressId equals a.AddressId into j5
                             from j6 in j5.DefaultIfEmpty()
                             join t in context.storemasters on x.StoreId equals t.StoreId into j7
                             from j8 in j7.DefaultIfEmpty()
                             where x.OrderStatusId == 1
                             orderby x.CreatedOn descending
                             select new CustomerOrderModel()
                             {
                                 OrderId = x.OrderId,
                                 OrderNumber = x.OrderNumber,
                                 Address = j6.CompleteAddress,
                                 CustomerName = j4.Name,
                                 PhoneNumber = j4.PhoneNumber,
                                 Email = j4.EmailID,
                                 OrderTime = (DateTime)x.CreatedOn,
                                 OrderStatus = j2.OrderStatus,
                                 TotalPrice = (decimal)x.TotalPrice,
                                 NetPrice = (decimal)x.NetAmount,
                                 TranscationId = x.TranactionId
                             }).FirstOrDefault();
                }
                else
                {

                    order = (from x in context.ordermasters
                             join ts in context.transactionmasters on x.OrderNumber equals ts.OrderNumber
                             join s in context.orderstatusmasters on x.OrderStatusId equals s.OrderStatusId into j1
                             from j2 in j1.DefaultIfEmpty()
                             join u in context.registrationmasters on x.LoginId equals u.LoginID into j3
                             from j4 in j3.DefaultIfEmpty()
                             join a in context.addressmasters on x.AddressId equals a.AddressId into j5
                             from j6 in j5.DefaultIfEmpty()
                             join t in context.storemasters on x.StoreId equals t.StoreId into j7
                             from j8 in j7.DefaultIfEmpty()
                             where x.OrderStatusId == 1 && x.StoreId == StoreId
                             orderby x.CreatedOn descending
                             select new CustomerOrderModel()
                             {
                                 OrderId = x.OrderId,
                                 OrderNumber = x.OrderNumber,
                                 Address = j6.CompleteAddress,
                                 CustomerName = j4.Name,
                                 PhoneNumber = j4.PhoneNumber,
                                 Email = j4.EmailID,
                                 OrderTime = (DateTime)x.CreatedOn,
                                 OrderStatus = j2.OrderStatus,
                                 TotalPrice = (decimal)x.TotalPrice,
                                 NetPrice = (decimal)x.NetAmount,
                                 TranscationId = x.TranactionId
                             }).FirstOrDefault();
                }

                return order;
            }
        }
        public IEnumerable<CustomerOrderModel> GetAllOrdersByStoreList(int StoreId)
        {
            List<CustomerOrderModel> order = new List<CustomerOrderModel>();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                if (StoreId == 0)
                {

                    order = (from x in context.ordermasters
                             join ts in context.transactionmasters on x.OrderNumber equals ts.OrderNumber
                             join s in context.orderstatusmasters on x.OrderStatusId equals s.OrderStatusId into j1
                             from j2 in j1.DefaultIfEmpty()
                             join u in context.registrationmasters on x.LoginId equals u.LoginID into j3
                             from j4 in j3.DefaultIfEmpty()
                             join a in context.addressmasters on x.AddressId equals a.AddressId into j5
                             from j6 in j5.DefaultIfEmpty()
                             join t in context.storemasters on x.StoreId equals t.StoreId into j7
                             from j8 in j7.DefaultIfEmpty()
                             //where x.OrderStatusId == StatusId
                             orderby x.OrderId descending
                             select new CustomerOrderModel()
                             {
                                 OrderId = x.OrderId,
                                 OrderNumber = x.OrderNumber,
                                 Address = j6.CompleteAddress,
                                 CustomerName = j4.Name,
                                 PhoneNumber = j4.PhoneNumber,
                                 Email = j4.EmailID,
                                 OrderTime = (DateTime)x.CreatedOn,
                                 OrderStatus = j2.OrderStatus,
                                 TotalPrice = (decimal)x.TotalPrice,
                                 NetPrice = (decimal)x.NetAmount,
                                 TranscationId = x.TranactionId
                             }).ToList();
                }
                else
                {

                    order = (from x in context.ordermasters
                             join ts in context.transactionmasters on x.OrderNumber equals ts.OrderNumber
                             join s in context.orderstatusmasters on x.OrderStatusId equals s.OrderStatusId into j1
                             from j2 in j1.DefaultIfEmpty()
                             join u in context.registrationmasters on x.LoginId equals u.LoginID into j3
                             from j4 in j3.DefaultIfEmpty()
                             join a in context.addressmasters on x.AddressId equals a.AddressId into j5
                             from j6 in j5.DefaultIfEmpty()
                             join t in context.storemasters on x.StoreId equals t.StoreId into j7
                             from j8 in j7.DefaultIfEmpty()
                             where x.StoreId == StoreId
                             orderby x.OrderId descending
                             select new CustomerOrderModel()
                             {
                                 OrderId = x.OrderId,
                                 OrderNumber = x.OrderNumber,
                                 Address = j6.CompleteAddress,
                                 CustomerName = j4.Name,
                                 PhoneNumber = j4.PhoneNumber,
                                 Email = j4.EmailID,
                                 OrderTime = (DateTime)x.CreatedOn,
                                 OrderStatus = j2.OrderStatus,
                                 TotalPrice = (decimal)x.TotalPrice,
                                 NetPrice = (decimal)x.NetAmount,
                                 TranscationId = x.TranactionId
                             }).ToList();
                }

                return order;
            }
        }
        public void SendSMS(OrderModel order)
        {
            try
            {
                string body = "";
                string orderdate = Convert.ToString(order.OrderTime.ToString("dd-MMM-yyyy"));
                if (order.OrderStatus == "Preparing")
                {
                    body = "Dear " + order.CustomerName + ",\nYour Order No is : " + order.OrderNumber + "\nYour Order has been accepted by : " + order.StoreName + ".\nOrder Date : " + orderdate + "\nTotal Amount : Rs." + order.NetPrice + "\nThanks & Regards\n " + order.StoreName + "\n " + order.StorePhoneNumber + "";
                    string _user = HttpUtility.UrlEncode("shamsweet"); // API user name to send SMS
                    string _pass = HttpUtility.UrlEncode("12345");     // API password to send SMS
                    string _route = HttpUtility.UrlEncode("transactional");
                    string _senderid = HttpUtility.UrlEncode("WISHHH");
                    string _recipient = HttpUtility.UrlEncode(order.PhoneNumber);  // who will receive message
                    string _messageText = HttpUtility.UrlEncode(body); // text message

                    // Creating URL to send sms
                    string _createURL = "http://www.smsnmedia.com/api/push?user=" + _user + "&pwd=" + _pass + "&route=" + _route + "&sender=" + _senderid + "&mobileno=91" + _recipient + "&text=" + _messageText;

                    HttpWebRequest _createRequest = (HttpWebRequest)WebRequest.Create(_createURL);
                    // getting response of sms
                    HttpWebResponse myResp = (HttpWebResponse)_createRequest.GetResponse();
                    StreamReader _responseStreamReader = new StreamReader(myResp.GetResponseStream());
                    string responseString = _responseStreamReader.ReadToEnd();
                    _responseStreamReader.Close();
                    myResp.Close();
                }
                if (order.OrderStatus == "Store Rejected")
                {
                    body = "Dear " + order.CustomerName + ",\nYour Order No is : " + order.OrderNumber + "\nYour Order has been decline by : " + order.StoreName + ".\nOrder Date : " + orderdate + "\nTotal Amount : Rs." + order.NetPrice + "\nThanks & Regards\n " + order.StoreName + "\n " + order.StorePhoneNumber + "";
                    string _user = HttpUtility.UrlEncode("shamsweet"); // API user name to send SMS
                    string _pass = HttpUtility.UrlEncode("12345");     // API password to send SMS
                    string _route = HttpUtility.UrlEncode("transactional");
                    string _senderid = HttpUtility.UrlEncode("WISHHH");
                    string _recipient = HttpUtility.UrlEncode(order.PhoneNumber);  // who will receive message
                    string _messageText = HttpUtility.UrlEncode(body); // text message

                    // Creating URL to send sms
                    string _createURL = "http://www.smsnmedia.com/api/push?user=" + _user + "&pwd=" + _pass + "&route=" + _route + "&sender=" + _senderid + "&mobileno=91" + _recipient + "&text=" + _messageText;

                    HttpWebRequest _createRequest = (HttpWebRequest)WebRequest.Create(_createURL);
                    // getting response of sms
                    HttpWebResponse myResp = (HttpWebResponse)_createRequest.GetResponse();
                    StreamReader _responseStreamReader = new StreamReader(myResp.GetResponseStream());
                    string responseString = _responseStreamReader.ReadToEnd();
                    _responseStreamReader.Close();
                    myResp.Close();
                }
                if (order.OrderStatus == "Dispatched")
                {
                    body = "Dear " + order.CustomerName + ",\nYour Order No is : " + order.OrderNumber + "\nYour Order is Dispatch by : " + order.StoreName + ".\nOrder Date : " + orderdate + "\nTotal Amount : Rs." + order.NetPrice + "\nThanks & Regards\n " + order.StoreName + "\n " + order.StorePhoneNumber + "";
                    string _user = HttpUtility.UrlEncode("shamsweet"); // API user name to send SMS
                    string _pass = HttpUtility.UrlEncode("12345");     // API password to send SMS
                    string _route = HttpUtility.UrlEncode("transactional");
                    string _senderid = HttpUtility.UrlEncode("WISHHH");
                    string _recipient = HttpUtility.UrlEncode(order.PhoneNumber);  // who will receive message
                    string _messageText = HttpUtility.UrlEncode(body); // text message

                    // Creating URL to send sms
                    string _createURL = "http://www.smsnmedia.com/api/push?user=" + _user + "&pwd=" + _pass + "&route=" + _route + "&sender=" + _senderid + "&mobileno=91" + _recipient + "&text=" + _messageText;

                    HttpWebRequest _createRequest = (HttpWebRequest)WebRequest.Create(_createURL);
                    // getting response of sms
                    HttpWebResponse myResp = (HttpWebResponse)_createRequest.GetResponse();
                    StreamReader _responseStreamReader = new StreamReader(myResp.GetResponseStream());
                    string responseString = _responseStreamReader.ReadToEnd();
                    _responseStreamReader.Close();
                    myResp.Close();
                }
            }
            catch (Exception ex)
            {

            }

        }
        public void SendEmail(OrderModel order)
        {
            try
            {
                string orderdate = Convert.ToString(order.OrderTime.ToString("dd-MMM-yyyy"));
                string subject = "";
                string body = "";
                if (order.OrderStatus == "Preparing")
                {
                    subject = "Your Order has been accepted " + order.OrderNumber;
                    body = "Dear " + order.CustomerName + "," +
                                 "<br>Your Order has been accepted by : " + order.StoreName +
                                  "<br>Your Order No is : " + order.OrderNumber +
                                 "<br>Order Date: " + orderdate +
                                 "<br>Total Price: " + order.TotalPrice +
                                 "<br>Discount: " + order.SpecialDiscount +
                                 "<br>GrandTotal: " + order.GrandTotal +
                                 "<br>Net Amount: " + order.NetPrice +
                                 "<br>Looking forwards to serve you." +
                                 "<br>Thanks & Regards " +
                                 "<br> " + order.StoreName +
                                 "<br> " + order.StorePhoneNumber;
                    string FromMail = "parkballuchi77@pindballuchi.com";
                    string emailTo = order.EmailId;
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.rediffmailpro.com");
                    mail.From = new MailAddress(FromMail);
                    mail.To.Add(emailTo);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("parkballuchi77@pindballuchi.com", "Aindia10");
                    SmtpServer.EnableSsl = false;
                    SmtpServer.Send(mail);
                }
                if (order.OrderStatus == "Store Rejected")
                {
                    subject = "Your Order has been rejected " + order.OrderNumber;
                    body = "Dear " + order.CustomerName + "," +
                                 "<br>Your Order has been rejected by : " + order.StoreName +
                                  "<br>Your Order No is : " + order.OrderNumber +
                                 "<br>Order Date: " + orderdate +
                                 "<br>Total Price: " + order.TotalPrice +
                                 "<br>Discount: " + order.SpecialDiscount +
                                 "<br>GrandTotal: " + order.GrandTotal +
                                 "<br>Net Amount: " + order.NetPrice +
                                 "<br>Looking forwards to serve you." +
                                 "<br>Thanks & Regards " +
                                 "<br> " + order.StoreName +
                                 "<br> " + order.StorePhoneNumber;
                    string FromMail = "parkballuchi77@pindballuchi.com";
                    string emailTo = order.EmailId;
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.rediffmailpro.com");
                    mail.From = new MailAddress(FromMail);
                    mail.To.Add(emailTo);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("parkballuchi77@pindballuchi.com", "Aindia10");
                    SmtpServer.EnableSsl = false;
                    SmtpServer.Send(mail);
                }
                if (order.OrderStatus == "Dispatched")
                {
                    subject = "Your Order is Dispatch " + order.OrderNumber;
                    body = "Dear " + order.CustomerName + "," +
                                 "<br>Your Order is Dispatch by : " + order.StoreName +
                                  "<br>Your Order No is : " + order.OrderNumber +
                                 "<br>Order Date: " + orderdate +
                                 "<br>Total Price: " + order.TotalPrice +
                                 "<br>Discount: " + order.SpecialDiscount +
                                 "<br>GrandTotal: " + order.GrandTotal +
                                 "<br>Net Amount: " + order.NetPrice +
                                 "<br>Looking forwards to serve you." +
                                 "<br>Thanks & Regards " +
                                 "<br> " + order.StoreName +
                                 "<br> " + order.StorePhoneNumber;
                    string FromMail = "parkballuchi77@pindballuchi.com";
                    string emailTo = order.EmailId;
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.rediffmailpro.com");
                    mail.From = new MailAddress(FromMail);
                    mail.To.Add(emailTo);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("parkballuchi77@pindballuchi.com", "Aindia10");
                    SmtpServer.EnableSsl = false;
                    SmtpServer.Send(mail);
                }
            }
            catch (Exception ex)
            {

            }
        }
        public AndroidFCMPushNotificationStatus SendNotification(OrderModel order)
        {
            AndroidFCMPushNotificationStatus result = new AndroidFCMPushNotificationStatus();

            if (order.OrderStatus == "Preparing")
            {
                try
                {
                    string orderdate = Convert.ToString(order.OrderTime.ToString("dd-MMM-yyyy"));
                    var applicationID = "AAAA2rG7ES8:APA91bHodgOIwfVE3hOy6H8hivDQKNGVQFt79qqKgQa18ygSf1yZYeyr6Ng6VxwrFqeR0MeajeCAHuTKVpb6V0LVyaXGEZn3Lpoa5W9e_FXcfPVPftolsyGazG1P1V4fE3sn5VGcdn2q";
                    var senderId = "939284697391";
                    string deviceId = order.DeviceId;
                    WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                    tRequest.Method = "post";
                    tRequest.ContentType = "application/json";

                    var notificationdata = new
                    {
                        to = deviceId,
                        data = new
                        {
                            title = "Your order: " + order.OrderNumber + " has been Accepted by Store",
                            OrderNumber = order.OrderNumber,
                            StoreName = order.StoreName,
                            OrderId = order.OrderId,
                            StoreId = order.StoreId,
                            LoginId = order.LoginId,
                        },
                        priority = "high"
                    };


                    var serializer = new JavaScriptSerializer();
                    var json = serializer.Serialize(notificationdata);
                    Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                    tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                    tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                    tRequest.ContentLength = byteArray.Length;

                    using (Stream dataStream = tRequest.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        using (WebResponse tResponse = tRequest.GetResponse())
                        {
                            using (Stream dataStreamResponse = tResponse.GetResponseStream())
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                JavaScriptSerializer j = new JavaScriptSerializer();
                                object a = j.Deserialize(sResponseFromServer, typeof(object));
                                result.Response = a;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Successful = false;
                    result.Response = null;
                    result.Error = ex;
                }
            }
            if (order.OrderStatus == "Store Rejected")
            {
                try
                {
                    string orderdate = Convert.ToString(order.OrderTime.ToString("dd-MMM-yyyy"));
                    var applicationID = "AAAA2rG7ES8:APA91bHodgOIwfVE3hOy6H8hivDQKNGVQFt79qqKgQa18ygSf1yZYeyr6Ng6VxwrFqeR0MeajeCAHuTKVpb6V0LVyaXGEZn3Lpoa5W9e_FXcfPVPftolsyGazG1P1V4fE3sn5VGcdn2q";
                    var senderId = "939284697391";
                    string deviceId = order.DeviceId;
                    WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                    tRequest.Method = "post";
                    tRequest.ContentType = "application/json";

                    var notificationdata = new
                    {
                        to = deviceId,
                        data = new
                        {
                            title = "Your order: " + order.OrderNumber + " has been Rejected by Store",
                            OrderNumber = order.OrderNumber,
                            StoreName = order.StoreName,
                            OrderId = order.OrderId,
                            StoreId = order.StoreId,
                            LoginId = order.LoginId,
                        },
                        priority = "high"
                    };

                    var serializer = new JavaScriptSerializer();
                    var json = serializer.Serialize(notificationdata);
                    Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                    tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                    tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                    tRequest.ContentLength = byteArray.Length;

                    using (Stream dataStream = tRequest.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        using (WebResponse tResponse = tRequest.GetResponse())
                        {
                            using (Stream dataStreamResponse = tResponse.GetResponseStream())
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                JavaScriptSerializer j = new JavaScriptSerializer();
                                object a = j.Deserialize(sResponseFromServer, typeof(object));
                                result.Response = a;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Successful = false;
                    result.Response = null;
                    result.Error = ex;
                }
            }
            if (order.OrderStatus == "Dispatched")
            {
                try
                {
                    string orderdate = Convert.ToString(order.OrderTime.ToString("dd-MMM-yyyy"));
                    var applicationID = "AAAA2rG7ES8:APA91bHodgOIwfVE3hOy6H8hivDQKNGVQFt79qqKgQa18ygSf1yZYeyr6Ng6VxwrFqeR0MeajeCAHuTKVpb6V0LVyaXGEZn3Lpoa5W9e_FXcfPVPftolsyGazG1P1V4fE3sn5VGcdn2q";
                    var senderId = "939284697391";
                    string deviceId = order.DeviceId;
                    WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                    tRequest.Method = "post";
                    tRequest.ContentType = "application/json";

                    var notificationdata = new
                    {
                        to = deviceId,
                        data = new
                        {
                            title = "Your order: " + order.OrderNumber + " is Dispatch by Store",
                            OrderNumber = order.OrderNumber,
                            StoreName = order.StoreName,
                            OrderId = order.OrderId,
                            StoreId = order.StoreId,
                            LoginId = order.LoginId,
                        },
                        priority = "high"
                    };

                    var serializer = new JavaScriptSerializer();
                    var json = serializer.Serialize(notificationdata);
                    Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                    tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                    tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                    tRequest.ContentLength = byteArray.Length;

                    using (Stream dataStream = tRequest.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        using (WebResponse tResponse = tRequest.GetResponse())
                        {
                            using (Stream dataStreamResponse = tResponse.GetResponseStream())
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                JavaScriptSerializer j = new JavaScriptSerializer();
                                object a = j.Deserialize(sResponseFromServer, typeof(object));
                                result.Response = a;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Successful = false;
                    result.Response = null;
                    result.Error = ex;
                }
            }
            return result;
        }
    }
}