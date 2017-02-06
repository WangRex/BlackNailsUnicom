using BlackNails.Config;
using LinqToExcel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlackNails.Controllers
{
    public class CustomerController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
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
            HttpPostedFileBase _postFile = Request.Files["excelFile"];
            if (_postFile == null) return Json(new { error = '1', message = "请选择文件" });
            _fileName = _postFile.FileName;
            _fileExt = Path.GetExtension(_fileName).ToLower();
            //文件类型
            _dirName = Request.QueryString["dir"];
            if (string.IsNullOrEmpty(_dirName))
            {
                _dirName = "file";
            }
            if (!extTable.ContainsKey(_dirName)) return Json(new { error = 1, message = "目录类型不存在" });
            //文件大小
            if (_postFile.InputStream == null || _postFile.InputStream.Length > _maxSize) return Json(new { error = 1, message = "文件大小超过限制" });
            //检查扩展名
            _fileParth = _dirName;
            _savePath = Server.MapPath(_fileParth);
            //检查上传目录
            if (!Directory.Exists(_savePath)) Directory.CreateDirectory(_savePath);
            string _newFileName = DateTime.Now.ToString("yyyyMMdd_hhmmss") + _fileExt;
            _savePath += _newFileName;
            _fileParth += _newFileName;
            //保存文件
            _postFile.SaveAs(_savePath);

            var excelFile = new ExcelQueryFactory(_savePath);
            //SheetName
            var sheetList = excelFile.GetWorksheetNames();//IEnumberable
            foreach (var sheet in sheetList)
            {
                //获得sheet对应的数据
                var data = excelFile.WorksheetNoHeader(sheet).ToList();
                //判断信息是否齐全
                if (data[1][2].Value.ToString() == "")
                {
                    
                }
            }

            //_postFile.ContentLength
            //保存数据库记录
            return Json(new { error = 0, url = Url.Content(_fileParth) });
        }
    }
}