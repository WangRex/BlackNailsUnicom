using System;
using System.Web.Http;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Text;
using System.Web;

namespace BlackNails.Controllers
{
    public abstract class WebAPI2BaseController : ApiController
    {
        public HttpContext context = HttpContext.Current;

        public static HttpResponseMessage toJson(Object obj)
        {
            String str;
            if (obj is String || obj is Char)
            {
                str = obj.ToString();
            }
            else
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                str = serializer.Serialize(obj);
            }
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }
    }
}
