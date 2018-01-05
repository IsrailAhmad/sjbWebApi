using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EverGreenWebApi.Interfaces;
using EverGreenWebApi.Models;
using System.Net;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace EverGreenWebApi.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public AndroidFCMPushNotificationStatus SendNotification(string strdeviceId)
        {
            AndroidFCMPushNotificationStatus result = new AndroidFCMPushNotificationStatus();

            try
            {
                var applicationID = "AAAA2rG7ES8:APA91bHodgOIwfVE3hOy6H8hivDQKNGVQFt79qqKgQa18ygSf1yZYeyr6Ng6VxwrFqeR0MeajeCAHuTKVpb6V0LVyaXGEZn3Lpoa5W9e_FXcfPVPftolsyGazG1P1V4fE3sn5VGcdn2q";
                var senderId = "939284697391";
                string deviceId = strdeviceId;
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    to = deviceId,
                    notification = new
                    {
                        body = "This is the message",
                        title = "This is the title",
                        icon = "myicon"
                    },
                    priority = "high"
                };

                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(data);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentLength = byteArray.Length;

                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        using (StreamReader tReader = new StreamReader(dataStreamResponse))
                        {
                            String sResponseFromServer = tReader.ReadToEnd();
                            JavaScriptSerializer j = new JavaScriptSerializer();
                            object a = j.Deserialize(sResponseFromServer, typeof(object));
                            result.Response = a;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Successful = false;
                result.Response = null;
                result.Error = ex;
            }
            return result;
        }

        //public dynamic SendPush(string deviceId)
        //{
        //    var data = new
        //    {
        //        to = deviceId,
        //        notification = new
        //        {
        //            body = "This is the message",
        //            title = "This is the title",
        //            icon = "myicon"
        //        }
        //    };

        //    string FireBase_URL = "https://fcm.googleapis.com/fcm/send";
        //    string key_server = "AAAA2rG7ES8:APA91bHodgOIwfVE3hOy6H8hivDQKNGVQFt79qqKgQa18ygSf1yZYeyr6Ng6VxwrFqeR0MeajeCAHuTKVpb6V0LVyaXGEZn3Lpoa5W9e_FXcfPVPftolsyGazG1P1V4fE3sn5VGcdn2q";
        //    string sender_id = "939284697391";
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(FireBase_URL);
        //    request.Method = "POST";
        //    request.Headers.Add("Authorization", "key=" + key_server);
        //    request.Headers.Add("Sender", "id=" + sender_id);
        //    request.ContentType = "application/json";
        //    string json = JsonConvert.SerializeObject(data);
        //    byte[] byteArray = Encoding.UTF8.GetBytes(json);
        //    request.ContentLength = byteArray.Length;
        //    Stream dataStream = request.GetRequestStream();
        //    dataStream.Write(byteArray, 0, byteArray.Length);
        //    dataStream.Close();
        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //    if (response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
        //    {
        //        StreamReader read = new StreamReader(response.GetResponseStream());
        //        String result = read.ReadToEnd();
        //        read.Close();
        //        response.Close();
        //        dynamic stuff = JsonConvert.DeserializeObject(result);
        //        return stuff;
        //    }
        //    else
        //    {
        //        throw new Exception("Ocurrio un error al obtener la respuesta del servidor: " + response.StatusCode);
        //    }

        //}
    }
}