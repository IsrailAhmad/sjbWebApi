using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EverGreenWebApi.Interfaces;
using EverGreenWebApi.Models;
using EverGreenWebApi.Repository;

namespace EverGreenWebApi.Controllers
{
    public class PromoCodeController : ApiController
    {
        static readonly IPromoCodeRepository _repository = new PromoCodeRepository();

        public HttpResponseMessage ApplyPromoCode(PromoCodeModel model)
        {
            ResponseStatus response = new ResponseStatus();
            PromoCodeModel data = new PromoCodeModel();
            try
            {
                if (Convert.ToInt64(model.LoginId) >= 0 && model.PromoCode != null)
                {
                    var result = _repository.ApplyPromoCode(model.LoginId, model.PromoCode);
                    if (result.PromoCode == model.PromoCode && result.LoginId == model.LoginId)
                    {
                        data.PromoCodeId = result.PromoCodeId;
                        data.PromoCode = result.PromoCode;
                        data.Discount = (decimal)result.Discount;
                        data.LoginId = (int)result.LoginId;
                        response.isSuccess = true;
                        return Request.CreateResponse(HttpStatusCode.OK, new {data, response });
                    }
                    else
                    {
                        response.serverResponseTime = System.DateTime.Now;
                        response.isSuccess = false;
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { response });
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
    }
}
