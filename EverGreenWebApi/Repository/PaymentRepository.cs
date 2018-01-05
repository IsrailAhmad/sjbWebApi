using EverGreenWebApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EverGreenWebApi.Models;
using EverGreenWebApi.DBHelper;
using System.Net;
using System.IO;
using System.Net.Mail;
using System.Web.Script.Serialization;
using System.Text;

namespace EverGreenWebApi.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public PaymentModel PaymentConfirm(PaymentModel model)
        {
            PaymentModel data = new PaymentModel();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                transactionmaster t = new transactionmaster();
                t.LoginId = model.LoginId;
                t.OrderId = model.OrderId;
                t.OrderNumber = model.OrderNumber;
                t.StoreId = model.StoreId;
                t.TransactionId = model.TransactionId;
                t.PaymentId = model.PaymentId;
                t.PaymentOrderId = model.PaymentOrderId;
                t.PaymentMode = model.PaymentMode;

                context.transactionmasters.Add(t);
                var result = context.SaveChanges();
                if (result > 0)
                {
                    var payment = (from o in context.ordermasters
                                   join tr in context.transactionmasters on o.OrderNumber equals tr.OrderNumber into j1
                                   from j2 in j1.DefaultIfEmpty()
                                   where o.OrderNumber == model.OrderNumber
                                   select new PaymentModel()
                                   {
                                       //transation id,orderNumber,total amount,paymentMode;
                                       Id = j2.Id,
                                       TransactionId = j2.TransactionId,
                                       OrderNumber = j2.OrderNumber,
                                       TotalAmount = (decimal)o.TotalPrice,
                                       PaymentMode = j2.PaymentMode
                                   }).FirstOrDefault();
                    data = payment;
                }

                if (data.Id > 0)
                {
                    SendSmsAndEmail(model.OrderNumber);
                }
                return data;
            }
        }

        public PaymentModel PaymentConfirmforCOD(PaymentModel model)
        {
            PaymentModel data = new PaymentModel();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                transactionmaster t = new transactionmaster();
                t.OrderNumber = model.OrderNumber;
                t.LoginId = model.LoginId;
                t.StoreId = model.StoreId;
                t.PaymentMode = model.PaymentMode;
                context.transactionmasters.Add(t);
                var result = context.SaveChanges();
                if (result > 0)
                {
                    var payment = (from o in context.ordermasters
                                   join tr in context.transactionmasters on o.OrderNumber equals tr.OrderNumber into j1
                                   from j2 in j1.DefaultIfEmpty()
                                   where o.OrderNumber == model.OrderNumber
                                   select new PaymentModel()
                                   {
                                       Id = j2.Id,
                                       OrderNumber = j2.OrderNumber,
                                       TotalAmount = (decimal)o.TotalPrice,
                                       PaymentMode = j2.PaymentMode
                                   }).FirstOrDefault();
                    data = payment;
                }
                if (data.Id > 0)
                {
                    SendSmsAndEmail(model.OrderNumber);
                }
                return data;

            }
        }

        public OrderModel SendSmsAndEmail(string ordernumber)
        {
            OrderModel data = new OrderModel();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                var order = (from x in context.ordermasters
                             join s in context.orderstatusmasters on x.OrderStatusId equals s.OrderStatusId
                             join p in context.promocodemasters on x.LoginId equals p.LoginId into j1
                             from j2 in j1.DefaultIfEmpty()
                             join c in context.registrationmasters on x.LoginId equals c.LoginID into j3
                             from j4 in j3.DefaultIfEmpty()
                             join st in context.storemasters on x.StoreId equals st.StoreId into j5
                             from j6 in j5.DefaultIfEmpty()
                                 //join m in context.orderdetails on x.OrderNumber equals m.OrderNumber
                             where x.OrderNumber == ordernumber
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
                data = order;
                if (data.OrderNumber != null)
                {
                    SendSMS(data);
                    SendEmail(data);
                    SendNotification(data);
                }

            }
            return data;
        }
        public void SendSMS(OrderModel order)
        {
            try
            {
                string orderdate = Convert.ToString(order.OrderTime.ToString("dd-MMM-yyyy"));
                string _user = HttpUtility.UrlEncode("shamsweet"); // API user name to send SMS
                string _pass = HttpUtility.UrlEncode("12345");     // API password to send SMS
                string _route = HttpUtility.UrlEncode("transactional");
                string _senderid = HttpUtility.UrlEncode("WISHHH");
                string _recipient = HttpUtility.UrlEncode(order.PhoneNumber);  // who will receive message
                string _messageText = HttpUtility.UrlEncode("Dear " + order.CustomerName + ",\nYour Order is confirmed with OrderNo: " + Convert.ToString(order.OrderNumber) + ".\nOrder Date : " + orderdate + "\nTotal Amount : Rs." + order.NetPrice + "\nThanks & Regards\n " + order.StoreName + "\n " + order.StorePhoneNumber + ""); // text message

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
            catch (Exception ex)
            {

            }
        }
        public void SendEmail(OrderModel order)
        {
            try
            {
                string orderdate = Convert.ToString(order.OrderTime.ToString("dd-MMM-yyyy"));
                string subject = "Your order: " + order.OrderNumber + " has been received";
                string body = "Dear " + order.CustomerName + "," +
                              "<br>Your Order is confirmed with OrderNo : " + Convert.ToString(order.OrderNumber) +
                              "<br>Order Date: " + orderdate +
                              //"<br>Total Price: " + order.TotalPrice +
                              //"<br>Discount: " + order.SpecialDiscount +
                              //"<br>GrandTotal: " + order.GrandTotal +
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
            catch
            {

            }
        }
        public void SendNotification(OrderModel order)
        {
            AndroidFCMPushNotificationStatus result = new AndroidFCMPushNotificationStatus();
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
                        title = "Your order: " + order.OrderNumber + " has been received",
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
            //return result;
        }
    }
}