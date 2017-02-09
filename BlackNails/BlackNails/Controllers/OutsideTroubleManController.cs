using BlackNails.CommonClass;
using BlackNails.Config;
using BlackNails.DAL;
using BlackNails.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlackNails.Controllers
{
    public class OutsideTroubleManController : MVC5_BaseController
    {
        private OutsideTroubleManServices _OutsideTroubleManServices = new OutsideTroubleManServices();
        private OrderServices _OrderServices = new OrderServices();
        private AssessmentServices _AssessmentServices = new AssessmentServices();
        
        public ActionResult index()
        {
            ViewBag.Title = "外线员列表";
            return View();
        }

        [HttpGet]
        public ActionResult MyJsonList()
        {
            var Role = Session["RoleName"].ToString();
            var OutsideTroubleManJson = _OutsideTroubleManServices.FindList().ToList();

            var list = new List<object>();
            foreach (OutsideTroubleManModel _OutsideTroubleManModel in OutsideTroubleManJson)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("OutsideTroubleMan_ID", _OutsideTroubleManModel.OutsideTroubleMan_ID);
                dic.Add("Name", _OutsideTroubleManModel.Name);
                dic.Add("Phone", _OutsideTroubleManModel.Phone);
                dic.Add("WorkYear", _OutsideTroubleManModel.WorkYear);
                dic.Add("WorkNo", _OutsideTroubleManModel.WorkNo);
                dic.Add("ResponsibleAreaBrief", _OutsideTroubleManModel.ResponsibleAreaBrief);
                dic.Add("Status", _OutsideTroubleManModel.Status);
                dic.Add("ServiceNum", _OrderServices.getOTMServiceNum(_OutsideTroubleManModel.OutsideTroubleMan_ID));
                dic.Add("GoodRaty", _AssessmentServices.getGoodAssessment(_OutsideTroubleManModel.OutsideTroubleMan_ID));
                dic.Add("Role", Role);
                list.Add(dic);
            }
            var resonse = new Response();
            resonse.Code = 0;
            resonse.Message = "获取外线员列表成功！";
            resonse.Data = list;
            return Json(resonse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            ViewBag.Title = "添加外线员";
            return View();
        }

        [HttpPost]
        public ActionResult Add(OutsideTroubleManModel outsideTroubleManModel)
        {
            outsideTroubleManModel.CreatePerson = Session["UserName"].ToString();
            outsideTroubleManModel.CreateTime = DateTime.Now;
            outsideTroubleManModel.UpdateTime = DateTime.Now;
            outsideTroubleManModel.Status = "可工作";
            var resonse = _OutsideTroubleManServices.Add(outsideTroubleManModel);
            return RedirectToAction("index", "OutsideTroubleMan");
        }

        public ActionResult ViewPage(int OutsideTroubleMan_ID)
        {
            ViewBag.Title = "查看外线员";
            TempData.Add("OutsideTroubleMan_ID", OutsideTroubleMan_ID);
            return View();
        }

        [HttpGet]
        public ActionResult MyJsonOTM(int OutsideTroubleMan_ID)
        {
            var Role = Session["RoleName"].ToString();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            OutsideTroubleManModel outsideTroubleManModel = _OutsideTroubleManServices.Find(OutsideTroubleMan_ID);
            var resonse = new Response();
            resonse.Code = 0;
            resonse.Message = "获取外线员成功！";
            resonse.Data = outsideTroubleManModel;
            return Json(resonse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult MyJsonListSelect()
        {
            var OutsideTroubleManJson = _OutsideTroubleManServices.FindList().Select(otm => new
            {
                OutsideTroubleMan_ID = otm.OutsideTroubleMan_ID,
                Name = otm.Name,
            }).ToList();
            var resonse = new Response();
            resonse.Code = 0;
            resonse.Message = "获取外线员列表成功！";
            resonse.Data = OutsideTroubleManJson;
            return Json(resonse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Upload()
        {

            var _uploadConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~").GetSection("UploadConfig") as UploadConfig;
            //文件最大限制
            int _maxSize = _uploadConfig.MaxSize;
            //保存路径
            string _savePath;
            //文件路径
            string _fileParth = "~/" + _uploadConfig.Path + "/";
            //文件名
            string _fileName;
            //扩展名
            string _fileExt;
            //文件类型
            string _dirName;
            //允许上传的类型
            Hashtable extTable = new Hashtable();
            extTable.Add("image", _uploadConfig.ImageExt);
            extTable.Add("flash", _uploadConfig.FileExt);
            extTable.Add("media", _uploadConfig.MediaExt);
            extTable.Add("file", _uploadConfig.FileExt);
            //上传的文件
            HttpPostedFileBase _postFile = Request.Files["PhotoFile"];
            if (_postFile == null) return Json(new { error = '1', message = "请选择文件" });
            _fileName = _postFile.FileName;
            _fileExt = Path.GetExtension(_fileName).ToLower();
            //文件类型
            _dirName = Request.QueryString["dir"];
            if (string.IsNullOrEmpty(_dirName))
            {
                _dirName = "image";
            }
            if (!extTable.ContainsKey(_dirName)) return Json(new { error = 1, message = "目录类型不存在" });
            //文件大小
            if (_postFile.InputStream == null || _postFile.InputStream.Length > _maxSize) return Json(new { error = 1, message = "文件大小超过限制" });
            //检查扩展名
            _fileParth += _dirName + "/";
            _savePath = Server.MapPath(_fileParth);
            //检查上传目录
            if (!Directory.Exists(_savePath)) Directory.CreateDirectory(_savePath);
            string _newFileName = DateTime.Now.ToString("yyyyMMdd_hhmmss") + _fileExt;
            _savePath += _newFileName;
            _fileParth += _newFileName;
            //保存文件
            _postFile.SaveAs(_savePath);
            TempData.Add("FileName", _fileParth.Substring(2));
            return Json(_fileParth);
        }

        [HttpGet]
        public ActionResult getFileName() {
            if(null == TempData["FileName"]) {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            return Json(TempData["FileName"].ToString(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult removeFileName()
        {
            if (null != TempData["FileName"])
            {
                TempData.Add("FileName", null);
                return Json("", JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}