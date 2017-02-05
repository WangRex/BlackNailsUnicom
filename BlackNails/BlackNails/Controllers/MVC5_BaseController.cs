using BlackNails.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using BlackNails.Config;
using BlackNails.DAL;

namespace BlackNails.Controllers
{
    [UserAuthorizeAttribute]
    public abstract class MVC5_BaseController : Controller
    {
        public List<T> GetParametersToModelList<T>()
        {
            string result = Request.Form[0];
            List<T> ResponseList = new List<T>();
            try
            {
                ResponseList = BlackNails.CommonClass.SerializeHelper.JsonListDeserialize<T>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ResponseList;
        }

        public T GetParametersToObject<T>()
        {
            string result = Request.Form[0];
            try
            {
                return BlackNails.CommonClass.SerializeHelper.JsonDeserialize<T>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
