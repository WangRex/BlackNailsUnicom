using BlackNails.CommonClass;
using BlackNails.Controllers;
using BlackNails.DAL;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace BlackNails.WebAPI
{
    public class UserController : WebAPI2BaseController
    {

        [HttpGet]
        public HttpResponseMessage sendCode(string mobile)
        {
            var response = new Response();

            Random r = new Random();
            int i = r.Next(10000, 99999);
            string Random = i.ToString();

            TimeSpan ts = DateTime.Now - DateTime.Parse("1970-1-1");
            string Timestamp = Convert.ToInt32(ts.TotalSeconds).ToString();

            //以字节方式存储
            byte[] data = Encoding.Default.GetBytes(Constant.APP_SECRET + Random + Timestamp);
            System.Security.Cryptography.SHA1 sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            //得到哈希值
            byte[] result = sha1.ComputeHash(data);
            //转换成为字符串的显示
            string Signature = BitConverter.ToString(result).Replace("-", "");

            WebRequest request = WebRequest.Create("http://api.sms.ronghub.com/sendCode.json");
            request.Method = "POST";
            string postData = "mobile=" + mobile + "&templateId=" + Constant.TEMPLATE_REGISTER + "&region=86";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            request.Headers.Add("App-Key", Constant.APP_KEY);
            request.Headers.Add("Nonce", Random);
            request.Headers.Add("Timestamp", Timestamp);
            request.Headers.Add("Signature", Signature);
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse resp = request.GetResponse();
            dataStream = resp.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            //var responseFromServer = "{\"code\":200,\"sessionId\":\"7jVyzHqjAV19w0P4e389TG\"}";
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            ReturnJson _ReturnJson = jsonSerializer.Deserialize<ReturnJson>(responseFromServer);
            var code = _ReturnJson.code;
            var sessionId = _ReturnJson.sessionId;
            if(code == 200)
            {
                response.Code = 0;
                response.Message = "发送验证码成功！";
                context.Cache.Insert(mobile, sessionId);
            } else
            {
                response.Code = 1;
                response.Message = "发送验证码失败！";
                context.Cache.Insert(mobile, "");
            }
            reader.Close();
            dataStream.Close();
            resp.Close();

            response.Data = null;
            return toJson(response);
        }

        [HttpGet]
        public HttpResponseMessage verifyCode(string mobile, string code)
        {
            var response = new Response();

            Random r = new Random();
            int i = r.Next(10000, 99999);
            string Random = i.ToString();

            TimeSpan ts = DateTime.Now - DateTime.Parse("1970-1-1");
            string Timestamp = Convert.ToInt32(ts.TotalSeconds).ToString();

            //以字节方式存储
            byte[] data = Encoding.Default.GetBytes(Constant.APP_SECRET + Random + Timestamp);
            System.Security.Cryptography.SHA1 sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            //得到哈希值
            byte[] result = sha1.ComputeHash(data);
            //转换成为字符串的显示
            string Signature = BitConverter.ToString(result).Replace("-", "");

            WebRequest request = WebRequest.Create("http://api.sms.ronghub.com/verifyCode.json");
            request.Method = "POST";
            string postData = "sessionId="+ context.Cache[mobile] +"&code=" + code;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers.Add("App-Key", Constant.APP_KEY);
            request.Headers.Add("Nonce", Random);
            request.Headers.Add("Timestamp", Timestamp);
            request.Headers.Add("Signature", Signature);
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse resp = request.GetResponse();
            dataStream = resp.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            ReturnJson _ReturnJson = jsonSerializer.Deserialize<ReturnJson>(responseFromServer);
            var rtnCode = _ReturnJson.code;
            var rtnSuccess = _ReturnJson.success;
            if (rtnCode == 200)
            {
                response.Code = 0;
                response.Message = "手机号验证成功！";
            }
            else
            {
                response.Code = 1;
                response.Message = "手机号验证失败！";
            }
            reader.Close();
            dataStream.Close();
            resp.Close();

            response.Data = rtnSuccess;
            return toJson(response);
        }
    }
}