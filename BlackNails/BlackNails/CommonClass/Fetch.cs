using System;
using System.Configuration;
using System.Web;
using System.Collections.Generic;
using System.Net;
using System.IO;

namespace BlackNails.CommonClass
{
	/// <summary>
    /// Fetch 的摘要说明。
	/// </summary>
	public class Fetch
	{
		/// <summary>
		/// 获取Url后面的值
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
        public static string Get(string name)
		{
			string text1 = HttpContext.Current.Request.QueryString[name];
			return ((text1 == null) ? "" : text1.Trim());
		}

		/// <summary>
		/// 获取表单Post过来的值
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
        public static string Post(string name)
		{
			string text1 = HttpContext.Current.Request.Form[name];
			return ((text1 == null) ? "" : text1.Trim());
		}

        /// <summary>
        /// 获取Url后面的值，如.....aspx?productid=2将获取到"2"
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int GetQueryId(string name)
        {
            int id = 0;
            int.TryParse(Get(name), out id);
            return id;
        }

        /// <summary>
        /// 获取表单Post过来的值，如表单checkboxlist传ids:2,3,5过来，将是int[]{2,3,4}
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int[] GetIds(string name) 
        {
            var ids = Post(name);
            List<int> result = new List<int>();
            int id = 0;
            var array = ids.Split(',');
            foreach (var a in array)
                if (int.TryParse(a.Trim(), out id))
                    result.Add(id);

            return result.ToArray();
        }

        /// <summary>
        /// 获取Url过来的值，如.....aspx?productid=2&productid=3，将是int[]{2,3}
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int[] GetQueryIds(string name)
        {
            var ids = Get(name);
            List<int> result = new List<int>();
            int id = 0;
            var array = ids.Split(',');
            foreach (var a in array)
                if (int.TryParse(a.Trim(), out id))
                    result.Add(id);

            return result.ToArray();
        }

        /// <summary>
        /// 获取当前页面的Url
        /// </summary>
        public static string CurrentUrl
		{
			get
			{
				return HttpContext.Current.Request.Url.ToString();
			}
		}

        /// <summary>
        /// 获取当前页面的主域，如www.GMS.com主域是GMS.com
        /// </summary>
        public static string ServerDomain
        {
            get
            {
                string urlHost = HttpContext.Current.Request.Url.Host.ToLower();
                string[] urlHostArray = urlHost.Split(new char[] { '.' });
                if ((urlHostArray.Length < 3) || RegExp.IsIp(urlHost))
                {
                    return urlHost;
                }
                string urlHost2 = urlHost.Remove(0, urlHost.IndexOf(".") + 1);
                if ((urlHost2.StartsWith("com.") || urlHost2.StartsWith("net.")) || (urlHost2.StartsWith("org.") || urlHost2.StartsWith("gov.")))
                {
                    return urlHost;
                }
                return urlHost2;
            }
        }

        /// <summary>
        /// 获取访问用户的IP
        /// </summary>
        public static string UserIp
        {
            get
            {
                string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                switch (result)
                {
                    case null:
                    case "":
                        result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                        break;
                }
                if (!RegExp.IsIp(result))
                {
                    return "Unknown";
                }
                return result;
            }
        }

        /// <summary>
        /// 根据IP获取省市
        /// </summary>
        public string GetAddressByIp(string ip)
        {
            string PostUrl = "http://int.dpool.sina.com.cn/iplookup/iplookup.php?ip=" + ip;
            string res = GetDataByPost(PostUrl);//该条请求返回的数据为：res=1\t115.193.210.0\t115.194.201.255\t中国\t浙江\t杭州\t电信
            return getAreaInfoList(res);
        }

        /// <summary>
        /// Post请求数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetDataByPost(string url)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                string s = "anything";
                byte[] requestBytes = System.Text.Encoding.ASCII.GetBytes(s);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = requestBytes.Length;
                Stream requestStream = req.GetRequestStream();
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();

                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(res.GetResponseStream(), System.Text.Encoding.Default);
                string backstr = sr.ReadToEnd();
                sr.Close();
                res.Close();
                return backstr;
            }
            catch (Exception)
            {
                return "未知IP";
            }

        }

        /// <summary>
        /// 处理所要的数据
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string getAreaInfoList(string ipData)
        {
            try
            {
                //1\t115.193.210.0\t115.194.201.255\t中国\t浙江\t杭州\t电信
                string[] areaArr = new string[10];
                    //取所要的数据，这里只取省市
                    areaArr = ipData.Split('\t');
                    return areaArr[3] + areaArr[4] + areaArr[5];
            }
            catch (Exception)
            {
                return "未知IP";
            }

        }

         public string Getkuaidi100()
        {
            string ApiKey=string.Empty ;
            string typeCom = string.Empty;
            string nu = string.Empty;
            WebClient client = new WebClient();
            string url = string.Format("http://www.kuaidi100.com/applyurl?key={0}&com={1}&nu={2}", ApiKey, typeCom, nu);
            Byte[] pageData = client.DownloadData(url);
            string pageHtml = System.Text.Encoding.ASCII.GetString(pageData);
            return pageHtml;
        }

	}
}
