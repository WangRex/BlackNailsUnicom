using BlackNails.CommonClass;
using BlackNails.Config;
using BlackNails.DAL;
using BlackNails.Models;
using LinqToExcel;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlackNails.Controllers
{
    public class CustomerController : MVC5_BaseController
    {
        private CustomerServices _CustomerServices = new CustomerServices();
        private MatchingServices _MatchingServices = new MatchingServices();

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
            _fileParth += "/" + _dirName;
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
            //IEnumberable
            var sheetList = excelFile.GetWorksheetNames();
            foreach (var sheet in sheetList)
            {
                //获得sheet对应的数据
                var data = excelFile.WorksheetNoHeader(sheet).ToList();
                for(var i = 3; i < data.Count; i++)
                {
                    var Tel = data[i][4];
                    var Name = data[i][11];
                    var Address = data[i][12];
                    var ONU = data[i][18];
                    if(StringUtil.IsNotNullOrEmpty(Address))
                    {
                        CustomerModel _CustomerModel = new CustomerModel();
                        _CustomerModel.CreatePerson = "System";
                        _CustomerModel.CreateTime = DateTime.Now;
                        _CustomerModel.Name = Name;
                        _CustomerModel.Tel = Tel;
                        _CustomerModel.Address = Address;
                        _CustomerModel.ONU = ONU;
                        _CustomerModel.UpdateTime = DateTime.Now;
                        _CustomerServices.Add(_CustomerModel);

                        var matchingAddress = Address.ToString().Substring(0, Address.ToString().IndexOf("号") + 1);

                        var matchingList = _MatchingServices.FindList().Where(mm => mm.Address == matchingAddress).ToList();
                        if (matchingList.Count == 0)
                        {
                            MatchingModel _MatchingModel = new MatchingModel();
                            _MatchingModel.CreatePerson = "System";
                            _MatchingModel.CreateTime = DateTime.Now;
                            _MatchingModel.Address = matchingAddress;
                            _MatchingModel.OTM_ID = 0;
                            _MatchingModel.UpdateTime = DateTime.Now;
                            _MatchingServices.Add(_MatchingModel);
                        }
                    }
                }
            }

            return RedirectToAction("Index", "Matching");
        }
    }
}