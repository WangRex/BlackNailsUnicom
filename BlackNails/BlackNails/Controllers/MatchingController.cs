using BlackNails.CommonClass;
using BlackNails.DAL;
using BlackNails.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BlackNails.Controllers
{
    public class MatchingController : MVC5_BaseController
    {
        private MatchingServices _MatchingServices = new MatchingServices();
        private OutsideTroubleManServices _OutsideTroubleManServices = new OutsideTroubleManServices();
        public ActionResult Index(int OTM_ID)
        {
            ViewBag.Title = "匹配列表";
            TempData.Add("OTM_ID", OTM_ID);
            return View();
        }

        [HttpGet]
        public ActionResult MyJsonList(int OTM_ID)
        {
            var Role = Session["RoleName"].ToString();
            var MatchingJson = _MatchingServices.FindList().ToList();
            if(OTM_ID == 0)
            {
                MatchingJson = MatchingJson.Where(mm => mm.OTM_ID == 0).ToList();
            } else if(OTM_ID == -1)
            {
                MatchingJson = MatchingJson.Where(mm => mm.OTM_ID != 0).ToList();
            } else
            {
                MatchingJson = MatchingJson.Where(mm => mm.OTM_ID == OTM_ID).ToList();
            }

            var list = new List<object>();
            foreach (MatchingModel _MatchingModel in MatchingJson)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("Matching_ID", _MatchingModel.Matching_ID);
                dic.Add("Address", _MatchingModel.Address);
                dic.Add("OTM_ID", _MatchingModel.OTM_ID);
                dic.Add("OTMName", _OutsideTroubleManServices.getOTMName(_MatchingModel.OTM_ID));
                dic.Add("Role", Role);
                list.Add(dic);
            }
            var resonse = new Response();
            resonse.Code = 0;
            resonse.Message = "获取匹配列表成功！";
            resonse.Data = list;
            return Json(resonse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Bind(int OTM_ID, string matchingIds)
        {
            var Role = Session["RoleName"].ToString();
            var matchingIdList = matchingIds.Split(',');

            foreach(var matchingId in matchingIdList)
            {
                if(StringUtil.IsNotNullOrEmpty(matchingId))
                {
                    MatchingModel _MatchingModel = _MatchingServices.Find(Convert.ToInt32(matchingId));
                    _MatchingModel.OTM_ID = OTM_ID;
                    _MatchingModel.UpdatePerson = Session["UserName"].ToString();
                    _MatchingModel.UpdateTime = DateTime.Now;
                    _MatchingServices.Update(_MatchingModel);
                }
            }
            var resonse = new Response();
            resonse.Code = 0;
            resonse.Message = "匹配成功！";
            resonse.Data = null;
            return Json(resonse, JsonRequestBehavior.AllowGet);
        }
    }
}