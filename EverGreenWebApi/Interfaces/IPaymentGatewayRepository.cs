using EverGreenWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EverGreenWebApi.Interfaces
{
    public interface IPaymentGatewayRepository:IDisposable
    {
        PaymentResponseModel CreatePaymentOrder(int LoginID,string OrderNumber,int StoreId);
    }
}