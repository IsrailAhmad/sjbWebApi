using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InstamojoAPI;
using System.IO;
using EverGreenWebApi.Interfaces;
using EverGreenWebApi.Repository;
using EverGreenWebApi.Models;

namespace EverGreenWebApi.Controllers
{
    public class PaymentGatewayController : ApiController
    {
        static readonly IPaymentGatewayRepository _repository = new PaymentGatewayRepository();

        [HttpPost]
        public HttpResponseMessage CreatePaymentOrder(PaymentGatewayModel model)
        {
            try
            {
                if (model.LoginID > 0 && model.OrderNumber !="" && model.StoreId > 0)
                {
                    var data = _repository.CreatePaymentOrder(model.LoginID, model.OrderNumber, model.StoreId);
                    if (data != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { data });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data });
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !", ex);
            }
        }



        #region   2. Get All your Payment Orders List
        //  Get All your Payment Orders
        //try
        //{
        //    PaymentOrderListRequest objPaymentOrderListRequest = new PaymentOrderListRequest();
        //    //Optional Parameters
        //    objPaymentOrderListRequest.limit = 21;
        //    objPaymentOrderListRequest.page = 3;

        //    PaymentOrderListResponse objPaymentRequestStatusResponse = objClass.getPaymentOrderList(objPaymentOrderListRequest);
        //    foreach (var item in objPaymentRequestStatusResponse.orders)
        //    {
        //        Console.WriteLine(item.email + item.description + item.amount);
        //    }
        //    MessageBox.Show("Order List = " + objPaymentRequestStatusResponse.orders.Count());
        //}
        //catch (ArgumentNullException ex)
        //{
        //    MessageBox.Show(ex.Message);
        //}
        //catch (WebException ex)
        //{
        //    MessageBox.Show(ex.Message);
        //}
        //catch (Exception ex)
        //{
        //    MessageBox.Show("Error:" + ex.Message);
        //}
        #endregion

        #region   3. Get details of this payment order Using Order Id
        ////  Get details of this payment order
        //try
        //{
        //    PaymentOrderDetailsResponse objPaymentRequestDetailsResponse = objClass.getPaymentOrderDetails("3189cff7c68245bface8915cac1f"); //"3189cff7c68245bface8915cac1f89df");
        //    MessageBox.Show("Transaction Id = " + objPaymentRequestDetailsResponse.transaction_id);
        //}
        //catch (ArgumentNullException ex)
        //{
        //    MessageBox.Show(ex.Message);
        //}
        //catch (WebException ex)
        //{
        //    MessageBox.Show(ex.Message);
        //}
        //catch (Exception ex)
        //{
        //    MessageBox.Show("Error:" + ex.Message);
        //}
        #endregion

        #region   4. Get details of this payment order Using TransactionId
        ////  Get details of this payment order Using TransactionId
        //try
        //{
        //    PaymentOrderDetailsResponse objPaymentRequestDetailsResponse = objClass.getPaymentOrderDetailsByTransactionId("test1");
        //    MessageBox.Show("Transaction Id = " + objPaymentRequestDetailsResponse.transaction_id);
        //}
        //catch (ArgumentNullException ex)
        //{
        //    MessageBox.Show(ex.Message);
        //}
        //catch (WebException ex)
        //{
        //    MessageBox.Show(ex.Message);
        //}
        //catch (Exception ex)
        //{
        //    MessageBox.Show("Error:" + ex.Message);
        //}
        #endregion

        #region   5. Create Refund
        //  Create Payment Order
        //        Refund objRefundRequest = new Refund();
        //Required POST parameters
        //objPaymentRequest.name = "ABCD";
        //        objRefundRequest.payment_id = "MOJO6701005J41260385";
        //                objRefundRequest.type = "TNR";
        //                objRefundRequest.body = "abcd";
        //                objRefundRequest.refund_amount = 9;

        //                if (objRefundRequest.validate())
        //                {
        //                    if (objRefundRequest.payment_idInvalid)
        //                    {
        //                        MessageBox.Show("payment_id is not valid");
        //                    }
        //}
        //                else
        //                {
        //                    try
        //                    {
        //                        CreateRefundResponce objRefundResponse = objClass.createNewRefundRequest(objRefundRequest);
        //MessageBox.Show("Refund Id = " + objRefundResponse.refund.id);
        //                    }
        //                    catch (ArgumentNullException ex)
        //                    {
        //                        MessageBox.Show(ex.Message);
        //                    }
        //                    catch (WebException ex)
        //                    {
        //                        MessageBox.Show(ex.Message);
        //                    }
        //                    catch (IOException ex)
        //                    {
        //                        MessageBox.Show(ex.Message);
        //                    }
        //                    catch (InvalidPaymentOrderException ex)
        //                    {
        //                        MessageBox.Show(ex.Message);
        //                    }
        //                    catch (ConnectionException ex)
        //                    {
        //                        MessageBox.Show(ex.Message);
        //                    }
        //                    catch (BaseException ex)
        //                    {
        //                        MessageBox.Show(ex.Message);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        MessageBox.Show("Error:" + ex.Message);
        //                    }
        //                }
        #endregion
    }
}
