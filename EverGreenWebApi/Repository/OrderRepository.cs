using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EverGreenWebApi.DBHelper;
using EverGreenWebApi.Interfaces;
using EverGreenWebApi.Models;
using System.Net;
using System.Web.Script.Serialization;
using System.Text;
using System.IO;
using System.Net.Mail;

namespace EverGreenWebApi.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        string path = "http://103.233.79.234/Data/SJB_Android/ProductPictures/";
        string strSequnceNumber;
        public string MakeIntoSequence(int i, int total_length, string prefix)
        {
            string output = i.ToString();
            int length_minus_prefix = total_length - prefix.Length;
            while (output.Length < length_minus_prefix)
                output = "0" + output;
            return prefix + output;
        }
        public OrderModel CreateOrder(OrderModel model)
        {

            OrderModel data = new OrderModel();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                var maxseq = 0;
                var first = context.ordermasters.Where(x => x.StoreId == model.StoreId).Select(s => s.OrderId).FirstOrDefault();
                if (first == 0)
                {
                    strSequnceNumber = "SJB00001";
                }
                else
                {
                    maxseq = context.ordermasters.Where(x => x.StoreId == model.StoreId).Max(s => s.OrderId);
                }

                if (maxseq != null && maxseq > 0)
                {
                    strSequnceNumber = MakeIntoSequence(maxseq + 1, 8, "SJB");
                }
                else
                {
                    strSequnceNumber = "SJB00001";
                }

                string strOrderNumber = strSequnceNumber;
                decimal grandtotal = 0;
                decimal deliverycharge = 0;
                decimal promocodediscount = 0;
                decimal specialdiscount = 0;
                decimal productprice = 0;
                decimal GSTvalue = 0;
                decimal productvalue = 0;
                decimal taxabale_value = 0;
                decimal gstvalue = 0;
                decimal productnetvalue = 0;
                decimal Itotal = 0;

                decimal Total = 0;
                decimal TotalDiscount = 0;
                decimal TotalGST = 0;
                decimal TotalSubTotal = 0;
                decimal TotalGrandTotal = 0;
                decimal NetPrice = 0;
                decimal TotalDeliveryCharge = 0;

                foreach (var pro in model.OrderDetails)
                {
                    var product = (from p in context.productmasters
                                   where p.ProductId == pro.ProductId
                                   select p).FirstOrDefault();
                    if (product.TaxType != null && product.TaxType == "I")
                    {
                        productvalue = pro.Quantity * (decimal)product.UnitPrice;
                        specialdiscount = (productvalue * (decimal)product.Discount / 100);
                        taxabale_value = productvalue - specialdiscount;
                        GSTvalue = taxabale_value * (decimal)product.GST / (100 + (decimal)product.GST);
                        Itotal = taxabale_value - GSTvalue;
                        productprice = (decimal)pro.Quantity * Itotal + GSTvalue;
                    }
                    else
                    {
                        productvalue = pro.Quantity * (decimal)product.UnitPrice;
                        specialdiscount = (productvalue * (decimal)product.Discount / 100);
                        taxabale_value = productvalue - specialdiscount;
                        GSTvalue = taxabale_value * (decimal)product.GST / 100;
                        productprice = taxabale_value + GSTvalue;
                    }
                    TotalDeliveryCharge += (decimal)product.DeliveryCharge;
                    Total += productvalue;
                    TotalDiscount += specialdiscount;
                    TotalSubTotal += taxabale_value;
                    TotalGST += GSTvalue;
                    TotalGrandTotal += productprice;

                }
                if (model.PromoCodeId > 0)
                {
                    promocodediscount = (decimal)(from p in context.promocodemasters
                                                  where p.PromoCodeId == model.PromoCodeId && p.LoginId == model.LoginId
                                                  select p.Discount).FirstOrDefault();
                }
                else
                {
                    promocodediscount = 0;
                }

                NetPrice = TotalGrandTotal + TotalDeliveryCharge - promocodediscount;
                ordermaster o = new ordermaster();
                o.OrderNumber = strOrderNumber;
                o.StoreId = model.StoreId;
                o.AddressId = model.AddressId;
                o.LoginId = model.LoginId;
                o.TotalPrice = Total;
                o.GrandTotal = TotalGrandTotal;
                o.SpecialDiscount = TotalDiscount;
                o.Discount = promocodediscount;
                o.NetAmount = NetPrice;
                o.TotalGST = TotalGST;
                o.SubTotal = TotalSubTotal;
                o.TotalDeliveryCharge = TotalDeliveryCharge;
                o.OrderStatusId = 1;
                foreach (var product in model.OrderDetails)
                {
                    orderdetail x = new orderdetail();
                    x.OrderNumber = strOrderNumber;
                    x.ProductId = product.ProductId;
                    x.Quantity = product.Quantity;
                    context.orderdetails.Add(x);
                }
                context.ordermasters.Add(o);

                var result = context.SaveChanges();

                var order = (from x in context.ordermasters
                             join s in context.orderstatusmasters on x.OrderStatusId equals s.OrderStatusId
                             join p in context.promocodemasters on x.LoginId equals p.LoginId into j1
                             from j2 in j1.DefaultIfEmpty()
                             join c in context.registrationmasters on x.LoginId equals c.LoginID into j3
                             from j4 in j3.DefaultIfEmpty()
                             join st in context.storemasters on x.StoreId equals st.StoreId into j5
                             from j6 in j5.DefaultIfEmpty()
                                 //join m in context.orderdetails on x.OrderNumber equals m.OrderNumber
                             where x.LoginId == model.LoginId
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
                return order;
            }
        }
        public OrderModel GetOrderByOrderNumber(string ordernumber)
        {
            //OrderModel data = new OrderModel();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                var order = (from x in context.ordermasters
                             join s in context.orderstatusmasters on x.OrderStatusId equals s.OrderStatusId
                             join p in context.promocodemasters on x.LoginId equals p.LoginId into j1
                             from j2 in j1.DefaultIfEmpty()
                             join st in context.storemasters on x.StoreId equals st.StoreId into j3
                             from j4 in j3.DefaultIfEmpty()
                             where x.OrderNumber == ordernumber
                             orderby x.OrderId descending
                             select new OrderModel()
                             {
                                 OrderId = x.OrderId,
                                 OrderNumber = x.OrderNumber,
                                 StoreId = (int)x.StoreId,
                                 StoreName = j4.StoreName,
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
                                        ProductPicturesUrl = path + r.ProductId + "ProductPictures.jpg"
                                    }).ToList();
                order.OrderDetails = orderdetails;
                return order;
            }
        }
        public IEnumerable<OrderModel> GetAllOrderByUser(int loginid)
        {
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                var order = (from x in context.ordermasters
                             join s in context.orderstatusmasters on x.OrderStatusId equals s.OrderStatusId
                             join r in context.orderdetails on x.OrderNumber equals r.OrderNumber
                             join a in context.storemasters on x.StoreId equals a.StoreId into j1
                             from j2 in j1.DefaultIfEmpty()
                             join p in context.promocodemasters on x.LoginId equals p.LoginId into j3
                             from j4 in j3.DefaultIfEmpty()
                             join t in context.transactionmasters on x.OrderNumber equals t.OrderNumber into j5
                             from j6 in j5.DefaultIfEmpty()
                             where x.LoginId == loginid && (j6.PaymentMode == "ONLINE" || j6.PaymentMode == "COD")
                             orderby x.OrderId descending
                             select new OrderModel()
                             {
                                 OrderId = x.OrderId,
                                 OrderNumber = x.OrderNumber,
                                 StoreId = (int)x.StoreId,
                                 StoreName = j2.StoreName,
                                 AddressId = (int)x.AddressId,
                                 LoginId = (int)x.LoginId,
                                 PromoCode = j4.PromoCode,
                                 TotalPrice = (decimal)x.TotalPrice,
                                 GrandTotal = (decimal)x.GrandTotal,
                                 SpecialDiscount = (decimal)x.SpecialDiscount,
                                 PromoDiscount = (decimal)x.Discount,
                                 NetPrice = (decimal)x.NetAmount,
                                 SubTotal = (decimal)x.SubTotal,
                                 TotalGST = (decimal)x.TotalGST,
                                 DeliveryCharge = (decimal)x.TotalDeliveryCharge,
                                 OrderTime = (DateTime)x.CreatedOn,
                                 OrderStatus = s.OrderStatus,
                                 OrderDetails = (from z in context.ordermasters
                                                 join r in context.orderdetails on z.OrderNumber equals r.OrderNumber
                                                 join p in context.productmasters on r.ProductId equals p.ProductId into productDetails
                                                 from tempc in productDetails.DefaultIfEmpty()
                                                 where z.LoginId == loginid
                                                 orderby r.OrderDetailsId descending
                                                 select new OrderDetailsModel()
                                                 {
                                                     ProductName = tempc.ProductName,
                                                     UnitPrice = (decimal)tempc.UnitPrice,
                                                     ProductId = (int)r.ProductId,
                                                     Quantity = (decimal)r.Quantity,
                                                     UOM = tempc.UOM,
                                                     ProductPicturesUrl = path + r.ProductId + "ProductPictures.jpg"
                                                 }).ToList(),
                             }).ToList();

                return order.GroupBy(x => x.OrderId)
                            .Select(g => g.First())
                            .ToList();
            }
        }
        public OrderModel TrackOrderStatus(int loginid, string ordernumber)
        {
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                var order = (from x in context.ordermasters
                             join s in context.orderstatusmasters on x.OrderStatusId equals s.OrderStatusId
                             where x.LoginId == loginid && x.OrderNumber == ordernumber
                             orderby x.OrderId descending
                             select new OrderModel()
                             {
                                 OrderId = x.OrderId,
                                 OrderNumber = x.OrderNumber,
                                 StoreId = (int)x.StoreId,
                                 AddressId = (int)x.AddressId,
                                 LoginId = (int)x.LoginId,
                                 OrderTime = (DateTime)x.CreatedOn,
                                 TotalPrice = (decimal)x.TotalPrice,
                                 OrderStatus = s.OrderStatus
                             }).First();

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
                                        ProductPicturesUrl = path + r.ProductId + "ProductPictures.jpg"
                                    }).ToList();
                order.OrderDetails = orderdetails;
                return order;
            }
        }
        public IEnumerable<OrderModel> MyAllOrderList(int loginid)
        {
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                var order = (from x in context.ordermasters
                             join s in context.orderstatusmasters on x.OrderStatusId equals s.OrderStatusId
                             join r in context.orderdetails on x.OrderNumber equals r.OrderNumber
                             join a in context.storemasters on x.StoreId equals a.StoreId into j1
                             from j2 in j1.DefaultIfEmpty()
                             join p in context.promocodemasters on x.LoginId equals p.LoginId into j3
                             from j4 in j3.DefaultIfEmpty()
                             join t in context.transactionmasters on x.OrderNumber equals t.OrderNumber into j5
                             from j6 in j5.DefaultIfEmpty()
                             where x.LoginId == loginid && (j6.PaymentMode == "ONLINE" || j6.PaymentMode == "COD") && s.OrderStatusId == 3
                             orderby x.OrderId descending
                             select new OrderModel()
                             {
                                 OrderId = x.OrderId,
                                 OrderNumber = x.OrderNumber,
                                 StoreId = (int)x.StoreId,
                                 StoreName = j2.StoreName,
                                 AddressId = (int)x.AddressId,
                                 LoginId = (int)x.LoginId,
                                 PromoCode = j4.PromoCode,
                                 TotalPrice = (decimal)x.TotalPrice,
                                 GrandTotal = (decimal)x.GrandTotal,
                                 SpecialDiscount = (decimal)x.SpecialDiscount,
                                 PromoDiscount = (decimal)x.Discount,
                                 NetPrice = (decimal)x.NetAmount,
                                 SubTotal = (decimal)x.SubTotal,
                                 TotalGST = (decimal)x.TotalGST,
                                 DeliveryCharge = (decimal)x.TotalDeliveryCharge,
                                 OrderTime = (DateTime)x.CreatedOn,
                                 OrderStatus = s.OrderStatus,
                                 OrderDetails = (from z in context.ordermasters
                                                 join r in context.orderdetails on z.OrderNumber equals r.OrderNumber
                                                 join p in context.productmasters on r.ProductId equals p.ProductId into productDetails
                                                 from tempc in productDetails.DefaultIfEmpty()
                                                 where z.LoginId == loginid
                                                 orderby r.OrderDetailsId descending
                                                 select new OrderDetailsModel()
                                                 {
                                                     ProductName = tempc.ProductName,
                                                     UnitPrice = (decimal)tempc.UnitPrice,
                                                     ProductId = (int)r.ProductId,
                                                     Quantity = (decimal)r.Quantity,
                                                     UOM = tempc.UOM,
                                                     ProductPicturesUrl = path + r.ProductId + "ProductPictures.jpg"
                                                 }).ToList(),
                             }).ToList();

                return order.GroupBy(x => x.OrderId)
                            .Select(g => g.First())
                            .ToList();
            }
        }


    }
}