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
    public class AddressController : ApiController
    {
        static readonly IAddressRepository _repository = new AddressRepository();

        [HttpPost]
        public HttpResponseMessage GetAllAddress(UserModel user)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                if (user.LoginID > 0)
                {
                    var addresses = _repository.GetAllAddress(user.LoginID);
                    if (addresses != null)
                    {
                        response.isSuccess = true;
                        response.serverResponseTime = System.DateTime.Now;
                        return Request.CreateResponse(HttpStatusCode.OK, new { addresses, response });
                    }
                    else
                    {
                        response.isSuccess = false;
                        response.serverResponseTime = System.DateTime.Now;
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

        [HttpPost]
        public HttpResponseMessage AddNewAddress(AddressModel model)
        {
            ResponseStatus response = new ResponseStatus();
            //AddressModel data = new AddressModel();

            try
            {
                if (ModelState.IsValid)
                {
                    var result = _repository.AddNewAddress(model);
                    if (result.AddressId > 0)
                    {                        
                        response.serverResponseTime = System.DateTime.Now;
                        response.isSuccess = true;
                        return Request.CreateResponse(HttpStatusCode.OK, new { result, response });
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

        [HttpPost]
        public HttpResponseMessage UpdateAddress(AddressModel model)
        {
            ResponseStatus response = new ResponseStatus();
            AddressModel data = new AddressModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _repository.UpdateAddress(model);
                    if (result.AddressId > 0)
                    {
                        data.AddressId = result.AddressId;                      
                        data.CompleteAddress = result.CompleteAddress;                      
                        data.ZipCode = result.ZipCode;
                        data.LandMark = result.LandMark;
                        data.LoginID = result.LoginID;
                        data.PhoneNumber = result.PhoneNumber;
                        data.LocalityId = result.LocalityId;
                        response.serverResponseTime = System.DateTime.Now;
                        response.isSuccess = true;
                        return Request.CreateResponse(HttpStatusCode.OK, new { data, response });
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

        [HttpPost]
        public HttpResponseMessage DeleteAddress(AddressModel model)
        {
            try
            {
                if (model.AddressId > 0)
                {
                    var result = _repository.DeleteAddress(model.AddressId);
                    if (result.isSuccess == true)
                    {

                        return Request.CreateResponse(HttpStatusCode.OK, new { result });
                    }
                    else
                    {

                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { result });
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
