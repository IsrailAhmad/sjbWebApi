using EverGreenWebApi.Interfaces;
using EverGreenWebApi.Models;
using EverGreenWebApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EverGreenWebApi.Controllers
{
    public class NotificationController : ApiController
    {
        static readonly INotificationRepository _repository = new NotificationRepository();

        [HttpPost]
        public HttpResponseMessage SendNotification(NotificationModel model)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                if (model.DeviceId != null)
                {
                    var data = _repository.SendNotification(model.DeviceId);

                    return Request.CreateResponse(HttpStatusCode.OK, new { data });

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
