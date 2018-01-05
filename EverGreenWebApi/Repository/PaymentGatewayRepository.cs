using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EverGreenWebApi.Interfaces;
using InstamojoAPI;
using System.IO;
using EverGreenWebApi.Models;
using EverGreenWebApi.DBHelper;

namespace EverGreenWebApi.Repository
{
    public class PaymentGatewayRepository : IPaymentGatewayRepository
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public PaymentResponseModel CreatePaymentOrder(int loginid, string ordernumber, int storeid)
        {
            PaymentResponseModel response = new PaymentResponseModel();
            string transaction_id = "";
            string OrderNumber = "";
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                var orderDetails = (from r in context.registrationmasters
                                    join o in context.ordermasters on r.LoginID equals o.LoginId
                                    where r.LoginID == loginid && o.OrderNumber == ordernumber && o.StoreId == storeid
                                    select new PaymentGatewayModel()
                                    {
                                        name = r.Name,
                                        phone = r.PhoneNumber,
                                        email = r.EmailID,
                                        description = o.OrderNumber,
                                        amount = (double)o.NetAmount
                                    }).First();
                //For Production
                //string Insta_client_id = "YPmEwDVMbYy4hH9EZOcY4Vsw3xJWjnAd3qlTS85J",
                //       Insta_client_secret = "fSiNKEG8PRLdOXSzpQrBba02Ix1sfbxDSHg3f562AFY0YqvgKXMm70wbE6vqxzmmpVOKB1MyG3tlwDzX4rzNXwRwqaHVafFQv1gkj4kNf14x1eeync9PoMuGtOulBxJk",
                //       Insta_Endpoint = InstamojoConstants.INSTAMOJO_API_ENDPOINT,
                //       Insta_Auth_Endpoint = InstamojoConstants.INSTAMOJO_AUTH_ENDPOINT;

                //For Test

                string Insta_client_id = "";
                string Insta_client_secret = "";
                string Insta_Endpoint = "";
                string Insta_Auth_Endpoint = "";
                if (storeid == 1)
                {
                    Insta_client_id = "HhWceDXORiCKCKIechUmXZX6vd4nj44kKvCUIVJv";
                    Insta_client_secret = "5B6CAfNsEoaakuzWLia7CMtgFsp6gt0MTrN5jR0MzsTIerNXKjmS9k1bBS1J8LdANjKXWtmlR1RuiOM8o71JVYYawBCl4NRiWbpKrI8OwkwnjNlvOuYRnjfkGnjozZDM";
                    Insta_Endpoint = InstamojoConstants.INSTAMOJO_API_ENDPOINT;
                    Insta_Auth_Endpoint = InstamojoConstants.INSTAMOJO_AUTH_ENDPOINT;
                }
                if (storeid == 2)
                {
                    Insta_client_id = "tmLkZZ0zV41nJwhayBGBOI4m4I7bH55qpUBdEXGS";
                    Insta_client_secret = "IDejdccGqKaFlGav9bntKULvMZ0g7twVFolC9gdrh9peMS0megSFr7iDpWwWIDgFUc3W5SlX99fKnhxsoy6ipdAv9JeQwebmOU6VRvOEQnNMWwZnWglYmDGrfgKRheXs";
                    Insta_Endpoint = InstamojoConstants.INSTAMOJO_API_ENDPOINT;
                    Insta_Auth_Endpoint = InstamojoConstants.INSTAMOJO_AUTH_ENDPOINT;
                }
                if (storeid == 3)
                {
                    Insta_client_id = "tmLkZZ0zV41nJwhayBGBOI4m4I7bH55qpUBdEXGS";
                    Insta_client_secret = "IDejdccGqKaFlGav9bntKULvMZ0g7twVFolC9gdrh9peMS0megSFr7iDpWwWIDgFUc3W5SlX99fKnhxsoy6ipdAv9JeQwebmOU6VRvOEQnNMWwZnWglYmDGrfgKRheXs";
                    Insta_Endpoint = InstamojoConstants.INSTAMOJO_API_ENDPOINT;
                    Insta_Auth_Endpoint = InstamojoConstants.INSTAMOJO_AUTH_ENDPOINT;
                }

                InstamojoAPI.Instamojo objClass = InstamojoImplementation.getApi(Insta_client_id, Insta_client_secret, Insta_Endpoint, Insta_Auth_Endpoint);


                PaymentOrder objPaymentRequest = new PaymentOrder();
                //Required POST parameters
                objPaymentRequest.name = orderDetails.name;
                objPaymentRequest.email = orderDetails.email;
                objPaymentRequest.phone = orderDetails.phone;
                objPaymentRequest.description = orderDetails.description;
                objPaymentRequest.amount = orderDetails.amount;
                objPaymentRequest.currency = "INR";

                string randomName = Path.GetRandomFileName();
                randomName = randomName.Replace(".", string.Empty);
                objPaymentRequest.transaction_id = "EVG" + randomName;

                transaction_id = objPaymentRequest.transaction_id;
                OrderNumber = objPaymentRequest.description;

                //For Production
                //objPaymentRequest.redirect_url = "http://103.233.79.234:1000/";
                //objPaymentRequest.webhook_url = "https://your.server.com/webhook";
                //Extra POST parameters 

                //For Test
                objPaymentRequest.redirect_url = "https://swaggerhub.com/api/saich/pay-with-instamojo/1.0.0";
                objPaymentRequest.webhook_url = "https://your.server.com/webhook";

                if (objPaymentRequest.validate())
                {
                    //if (objPaymentRequest.emailInvalid)
                    //{
                    //    response.Message = "Email is not valid";
                    //}
                    //if (objPaymentRequest.nameInvalid)
                    //{
                    //    response.Message = "Name is not valid";
                    //}
                    if (objPaymentRequest.phoneInvalid)
                    {
                        response.Message = "Phone is not valid";
                    }
                    if (objPaymentRequest.amountInvalid)
                    {
                        response.Message = "Amount is not valid";
                    }
                    if (objPaymentRequest.currencyInvalid)
                    {
                        response.Message = "Currency is not valid";
                    }
                    if (objPaymentRequest.transactionIdInvalid)
                    {
                        response.Message = "Transaction Id is not valid";
                    }
                    if (objPaymentRequest.redirectUrlInvalid)
                    {
                        response.Message = "Redirect Url Id is not valid";
                    }
                    if (objPaymentRequest.webhookUrlInvalid)
                    {
                        response.Message = "Webhook URL is not valid";
                    }

                }
                else
                {
                    try
                    {
                        CreatePaymentOrderResponse objPaymentResponse = objClass.createNewPaymentRequest(objPaymentRequest);
                        response.PaymentURL = objPaymentResponse.payment_options.payment_url;
                        var data = context.ordermasters.Where(x => x.LoginId == loginid && x.OrderNumber == ordernumber && x.StoreId == storeid).FirstOrDefault();
                        if (data != null)
                        {
                            context.ordermasters.Where(x => x.LoginId == loginid && x.OrderNumber == ordernumber && x.StoreId == storeid).ToList().ForEach(x => x.TranactionId = transaction_id);
                            context.SaveChanges();
                        }
                    }
                    catch (ArgumentNullException ex)
                    {
                        response.Message = ex.Message;
                    }
                    catch (WebException ex)
                    {
                        response.Message = ex.Message;
                    }
                    catch (IOException ex)
                    {
                        response.Message = ex.Message;
                    }
                    catch (InvalidPaymentOrderException ex)
                    {
                        if (!ex.IsWebhookValid())
                        {
                            response.Message = "Webhook is invalid";
                        }

                        if (!ex.IsCurrencyValid())
                        {
                            response.Message = "Currency is Invalid";
                        }

                        if (!ex.IsTransactionIDValid())
                        {
                            response.Message = "Transaction ID is Invalid";
                        }
                    }
                    catch (ConnectionException ex)
                    {
                        response.Message = ex.Message;
                    }
                    catch (BaseException ex)
                    {
                        response.Message = ex.Message;
                    }
                    catch (Exception ex)
                    {
                        response.Message = ex.Message;
                    }
                }               
            }
            return response;
        }
    }
}