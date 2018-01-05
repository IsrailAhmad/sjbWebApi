using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EverGreenWebApi.DBHelper;
using EverGreenWebApi.Interfaces;
using EverGreenWebApi.Models;
using System.Drawing;

namespace EverGreenWebApi.Repository
{
    public class UploadProfilePictureRepository : IUploadProfilePictureRepository
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public UserModel UploadProfilePicture(int loginid)
        {
            UserModel users = new UserModel();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                registrationmaster u = new registrationmaster();

                var data = context.registrationmasters.Where(x => x.LoginID == loginid).FirstOrDefault();
                if (data != null)
                {

                    users.LoginID = Convert.ToInt32(data.LoginID);
                    users.Name = data.Name;
                    users.PhoneNumber = data.PhoneNumber;
                    users.Otp = Convert.ToInt32(data.OTP);
                    users.Role = Convert.ToInt32(data.Role);
                    users.EmailID = data.EmailID;
                    users.ProfilePictureUrl = "http://103.233.79.234/Data/SJB_Android/ProfilePicture/"+ loginid + ".jpg";
                    //users.ProfilePictureUrl = "http:/localhost:51673/Data/SJB_Android/ProfilePicture/" + loginid + ".jpg";

                }
            }
            return users;
        }

       
            public static Image LoadFromAspNetUrl(string url)
            {
                if (HttpContext.Current == null)
                {
                    throw new ApplicationException("Can't use HttpContext.Current in non-ASP.NET context");
                }
                return Image.FromFile(HttpContext.Current.Server.MapPath(url));
            }
       


    }
}