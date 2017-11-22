using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace domain.MessageModify
{
    class Meeting
    {
  
        private UInt32 GetStamp(DateTime dt)
        {
            TimeSpan ts = dt - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            UInt32 uiStamp = Convert.ToUInt32(ts.TotalSeconds);
            return uiStamp;
        }

        private UInt32 GetStamp()
        {
            return GetStamp(DateTime.Now);
        }


        private String GetToken()
        {

            var request = (HttpWebRequest)WebRequest.Create("https://testcloudb.domain.com/ucopenapi/auth/token/create");
            request.ContentType = "application/json";
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = "{\"role\":0," +
                   "\"username\":\"admin@ucb20151104.domain.com\", " +
                   "\"password\":\"11111111\", " +
                   "\"appId\":0" +
                    "}";

                System.Console.WriteLine("请求Token: ");
                System.Console.WriteLine(json);

                streamWriter.Write(json);
            }

            var response = (HttpWebResponse)request.GetResponse();
            string result;
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
                System.Console.WriteLine("请求Token响应：");
                System.Console.WriteLine(result);
            }

            try
            {
                var jObject = JObject.Parse(result);

                if (jObject != null)
                {

                    var data = jObject["data"];
                    string token = data["token"].ToString();
                    Console.WriteLine("Token :" + data["token"].ToString());
                    return token;
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }

        }

        public void Create()
        {

            System.Console.WriteLine(DateTime.Now.Ticks);
            System.Console.WriteLine("End");

            var request = (HttpWebRequest)WebRequest.Create("https://testcloudb.domain.com/ucopenapi/calendar/create");
            request.ContentType = "application/json";
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = "{\"username\":\"admin@ucb20151104.domain.com\"," +
                   "\"token\":\"" + GetToken() + "\", " +
                   "\"data\":{" +
                   "\"title\":\"meeting test\", " +
                   "\"location\":\"meeting room\", " +
                   "\"summary\":\"meeting summary\", " +
                   "\"owner\":\"testb_sanguo20@domain.com\", " +
                   "\"members\":[" + "\"admin@ucb20151104.domain.com\", \"test_yunpan3@bee.com\"" + "]," +
                   "\"startTime\":" + (GetStamp() + 3600 * 3) + ", " +
                   "\"duration\":20, " +
                   "\"priority\":1 " +
                    "}}";
                System.Console.WriteLine("请求创建日程： ");
                System.Console.WriteLine(json);

                streamWriter.Write(json);
            }

            var response = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                System.Console.WriteLine("请求创建日程响应：");
                System.Console.WriteLine(result.ToString());

                System.Console.WriteLine("End");
            }
        }
    }
}
