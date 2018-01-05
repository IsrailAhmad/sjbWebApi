using EverGreenWebApi.Interfaces;
using EverGreenWebApi.Repository;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using System;
using EverGreenWebApi.Models;
using System.IO;
using System.Drawing;

namespace EverGreenWebApi.Controllers
{
    public class LoginController : ApiController
    {
        // GET: OtpGeneration
        //public ActionResult Index()
        //{
        //    return View();
        //}

        static readonly IUserRepository _repository = new UserRepository();

        // GET api/Login?phoneNumber=9555740041
        [HttpPost]
        public HttpResponseMessage Login(UserModel user)
        {
            ResponseStatus response = new ResponseStatus();
            UserModel data = new UserModel();
            try
            {
                if (Convert.ToInt64(user.PhoneNumber) >= 0 && user.PhoneNumber != null)
                {
                    var result = _repository.Login(user.PhoneNumber, user.DeviceId);
                    if (result != null)
                    {
                        data.LoginID = result.LoginID;
                        data.Name = result.Name;
                        data.PhoneNumber = result.PhoneNumber;
                        data.EmailID = result.EmailID;
                        data.Otp = result.Otp;
                        data.Role = result.Role;
                        data.ProfilePictureUrl = result.ProfilePictureUrl;
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
        public HttpResponseMessage ResendOTP(UserModel user)
        {
            try
            {
                if (Convert.ToInt64(user.PhoneNumber) >= 0 && (user.PhoneNumber != null || user.LoginID != 0))
                {
                    var response = _repository.Login(user.PhoneNumber, user.LoginID);
                    if (response != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.Created, new { response });
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !");
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
        public HttpResponseMessage VerifyOTP(UserModel user)
        {
            var data = new UserModel();
            var response = new ResponseStatus();
            if (Convert.ToInt64(user.PhoneNumber) >= 0 && user.PhoneNumber != null)
            {
                var result = _repository.Login(user.PhoneNumber, Convert.ToString(user.Otp), user.LoginID);
                if (result.PhoneNumber != null)
                {
                    data.LoginID = result.LoginID;
                    data.Name = result.Name;
                    data.PhoneNumber = result.PhoneNumber;
                    data.EmailID = result.EmailID;
                    data.Otp = result.Otp;
                    data.Role = result.Role;
                    data.ProfilePictureUrl = result.ProfilePictureUrl;
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

        [HttpPost]
        public HttpResponseMessage UpdateProfile(UserModel user)
        {
            //var data = new UserModel();
            var response = new ResponseStatus();
            try
            {
                //string path = "C:/inetpub/wwwroot/Data/SJB_Android/ProfilePicture/" + user.LoginID + ".jpeg";
                //var bytess = Convert.FromBase64String(user.ProfilePictureUrl);
                //using (var imageFile = new FileStream(path, FileMode.Create))
                //{
                //    imageFile.Write(bytess, 0, bytess.Length);
                //    imageFile.Flush();
                //}

                if (ModelState.IsValid)
                {
                    var data = _repository.UpdateProfile(user);
                    if (data != null)
                    {
                        response.isSuccess = true;
                        response.serverResponseTime = System.DateTime.Now;
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
        public HttpResponseMessage GetUserProfile(UserModel user)
        {
            //var data = new UserModel();
            var response = new ResponseStatus();
            try
            {
                if (user.LoginID > 0)
                {
                    var data = _repository.GetUserProfile(user.LoginID);
                    if (data != null)
                    {
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
        public HttpResponseMessage IsMobileVerified(UserModel user)
        {
            var data = new UserModel();
            var response = new ResponseStatus();
            try
            {
                if (user.LoginID > 0)
                {
                    var result = _repository.IsMobileVerified(user.LoginID);
                    if (result != null)
                    {
                        data.LoginID = result.LoginID;
                        data.Name = result.Name;
                        data.PhoneNumber = result.PhoneNumber;
                        data.EmailID = result.EmailID;
                        data.ProfilePictureUrl = result.ProfilePictureUrl;
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
        public HttpResponseMessage SocialUserLogin(UserModel user)
        {
            ResponseStatus response = new ResponseStatus();
            //UserModel data = new UserModel();
            try
            {

                var data = _repository.SocialUserLogin(user);
                if (data != null)
                {
                   
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
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !", ex);
            }
        }
    }
}