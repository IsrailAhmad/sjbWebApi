using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EverGreenWebApi.Models;

namespace EverGreenWebApi.Interfaces
{
    public interface IUploadProfilePictureRepository:IDisposable
    {
        UserModel UploadProfilePicture(int loginid);
    }
}