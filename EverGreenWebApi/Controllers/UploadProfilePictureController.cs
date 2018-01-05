using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using EverGreenWebApi.Interfaces;
using EverGreenWebApi.Models;
using EverGreenWebApi.Repository;

namespace EverGreenWebApi.Controllers
{

    public class UploadProfilePictureController : ApiController
    {
        static readonly IUploadProfilePictureRepository _repository = new UploadProfilePictureRepository();

        public byte[] stringToBase64ByteArray(String input)
        {
            byte[] ret = System.Text.Encoding.Unicode.GetBytes(input);
            string s = Convert.ToBase64String(ret);
            ret = System.Text.Encoding.Unicode.GetBytes(s);
            return ret;
        }

        [HttpPost]
        public HttpResponseMessage UploadProfilePicture(UploadPictureModel model)
        {
            ResponseStatus serverResponse = new ResponseStatus();
            //UserModel data = new UserModel();
            try
            {
                //string image64string = model.ImageUrl + "==";
                string path = "C:/inetpub/wwwroot/Data/SJB_Android/ProfilePicture/" + model.LoginID + ".jpeg";
                var bytess = Convert.FromBase64String(model.ImageUrl);
                using (var imageFile = new FileStream(path, FileMode.Create))
                {
                    imageFile.Write(bytess, 0, bytess.Length);
                    imageFile.Flush();
                }
                if (model.LoginID > 0 && model.ImageUrl != null)
                {
                   var data = _repository.UploadProfilePicture(model.LoginID);
                    if (data != null)
                    {
                        serverResponse.isSuccess = true;
                        serverResponse.serverResponseTime = System.DateTime.Now;
                        return Request.CreateResponse(HttpStatusCode.Created, new { data, serverResponse });
                    }
                    else
                    {
                        serverResponse.isSuccess = false;
                        serverResponse.serverResponseTime = System.DateTime.Now;
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { serverResponse });
                    }
                }
                else
                {
                    serverResponse.isSuccess = false;
                    serverResponse.serverResponseTime = System.DateTime.Now;
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { serverResponse });
                }
            }
            catch (Exception ex)
            {
                serverResponse.isSuccess = false;
                serverResponse.serverResponseTime = System.DateTime.Now;
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something Worng !", ex);
            }
        }
    }
}
