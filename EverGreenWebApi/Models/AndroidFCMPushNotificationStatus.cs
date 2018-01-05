using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EverGreenWebApi.Models
{
    public class AndroidFCMPushNotificationStatus
    {
        public bool Successful
        {
            get;
            set;
        }

        public object Response
        {
            get;
            set;
        }
        public Exception Error
        {
            get;
            set;
        }
    }  
}