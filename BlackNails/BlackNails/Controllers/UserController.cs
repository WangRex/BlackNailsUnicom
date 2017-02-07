using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlackNails.Models;
using BlackNails.DAL;
using BlackNails.CommonClass;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using BlackNails.Config;
using System.Collections;
using System.IO;

namespace BlackNails.User.Controllers
{
    /// <summary>
    /// 用户控制器
    /// </summary>
    /// 
    public class UserController : Controller
    {
        private UserServices _userServiecs = new UserServices();

        private Fetch ipsearch = new Fetch();

        private string ViewBagTitle = string.Empty;

        public ActionResult login()
        {
            ViewBag.Title = "登录";
            return View();
        }


        [HttpPost]
        public ActionResult login(UserModel loginVM)
        {
            string remember = Request["remember"];
            var _user = _userServiecs.FindByUserNickName(loginVM.UserNickName);
            if (_user == null)
            {
                ViewData["message"] = "账号不存在";
                return View("/Views/User/login.cshtml");
            }
            else if (Security.Sha256(loginVM.Password) != _user.Password)
            {
                ViewData["message"] = "密码不正确";
                return View("/Views/User/login.cshtml");
            }
            else
            {
                ViewData["message"] = "";
                _user.RegistrationTime = DateTime.Now;
                _user.LoginTime = DateTime.Now;
                _user.LoginIP = Request.UserHostAddress.Replace(".", ":") + "[" + ipsearch.GetAddressByIp(Request.UserHostAddress) + "]";
                _userServiecs.Update(_user);
                Session.Add("UserNickName", _user.UserNickName);
                Session.Add("UserName", _user.UserName);
                Session.Add("UserID", _user.UserID);
                //Session.Add("RoleNum", _roleManager.GetRoleNum(_user.Role));
                //Session.Add("RoleDescription", _roleManager.GetRoleDescription(_user.Role));
                Session.Add("RoleName", _user.Role);

                var _identity = _userServiecs.CreateIdentity(_user, DefaultAuthenticationTypes.ApplicationCookie);
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, _identity);
                return RedirectToAction("Index", "Home");
            }
        }


        public JsonResult passwordReset(ChangePasswordViewModel user)
        {
            ajaxResponse response = new ajaxResponse();
            var _user = _userServiecs.FindByUserName(user.UserName);
            if (_user == null)
            {
                response.info = "账号不存在";
            }
            else if (Security.Sha256(user.OriginalPassword) != _user.Password)
            {
                response.info = "密码不正确";
            }
            else
            {
                _user.Password = user.Password;
                _userServiecs.Update(_user);
                response.info = "密码修改成功,请重新登录";
            }
            return Json(response);
        }
        
        public JsonResult Register(UserModel register)
        {
            ajaxResponse response = new ajaxResponse();
            if (_userServiecs.FindByUserName(register.UserName) == null)
            {
                register.LoginIP = Request.UserHostAddress.Replace(".", ":") + "[" + ipsearch.GetAddressByIp(Request.UserHostAddress) + "]";
                register.RegistrationTime = DateTime.Now;
                register.LoginTime = DateTime.Now;
                if (register.UserNickName.Equals("横刀"))
                {
                    register.Role = "超级管理员";
                }
                else
                {
                    register.Role = "普通用户";
                }
                register.Status = "新注册用户";
                _userServiecs.Add(register);
                response.info = "注册成功，请返回登录画面登录";
            }
            else
            {
                response.info = "该用户已注册";
            }
            return Json(response);
        }

        public JsonResult UserNameCheck()
        {
            string UserName = Request["param"];
            ajaxResponse response = new ajaxResponse();
            if (_userServiecs.FindByUserName(UserName) == null)
            {
                response.status = "y";
                response.info = "通过验证";
                return Json(response);
            }
            else
            {
                response.status = "n";
                response.info = "该用户已注册请用新邮箱或手机号注册";
                return Json(response);
            }
        }

        public JsonResult UserNickNameCheck()
        {
            string UserNickName = Request["param"];
            ajaxResponse response = new ajaxResponse();
            if (_userServiecs.FindByUserNickName(UserNickName) == null)
            {
                response.status = "y";
                response.info = "通过验证";
                return Json(response);
            }
            else
            {
                response.status = "n";
                response.info = "该名字已经有人使用请换一个再试";
                return Json(response);
            }
        }

        /// <summary>
        /// 我的资料
        /// </summary>
        /// <returns></returns>
        public ActionResult UserProfile()
        {
            return View();
        }



        public ActionResult Calendar()
        {
            return View();
        }


        public ActionResult menu()
        {
            return PartialView("UserModifyMenuPartial");
        }

        /// <summary>
        /// 添加【分部视图】
        /// </summary>
        /// <returns></returns>
        public PartialViewResult AddPartialView()
        {
            return PartialView();
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
        //===================================================管理员模式下调用======================================================

        /// <summary>
        /// 删除 
        /// Response.Code:1-成功，2-部分删除，0-失败
        /// Response.Data:删除的数量
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteJson(List<int> ids)
        {
            int _total = ids.Count();
            Response _res = new Response();
            int _currentAdminID = int.Parse(Session["UserID"].ToString());
            if (ids.Contains(_currentAdminID))
            {
                ids.Remove(_currentAdminID);
            }
            _res = _userServiecs.Delete(ids);
            if (_res.Code == 1 && _res.Data < _total)
            {
                _res.Code = 2;
                _res.Message = "共提交删除" + _total + "名用户,实际删除" + _res.Data + "名用户。\n原因：不能删除当前登录的账号";
            }
            else if (_res.Code == 2)
            {
                _res.Message = "共提交删除" + _total + "名用户,实际删除" + _res.Data + "名用户。";
            }
            return Json(_res);
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        public JsonResult ListJson()
        {
            return Json(_userServiecs.FindList());
        }

        [HttpPost]
        public ActionResult CreatByGrid()
        {
            string result = Request.Form[0];
            //后台拿到字符串时直接反序列化。根据需要自己处理
            List<UserModel> datagridList = new List<UserModel>();
            try
            { datagridList = JsonConvert.DeserializeObject<List<UserModel>>(result); }
            catch (Exception)
            {
                Response refData = new Response();
                refData.Code = 0;
                refData.Message = "输入数据类型错误，请点撤销后重新输入";
                return Json(refData);
            }
            foreach (UserModel _User in datagridList)
            {
                Response refFlg = new Response();
                if (_User.UserID > 0)
                {
                    var _user = _userServiecs.Find(_User.UserID);
                    _user.UserID = _User.UserID;
                    _user.UserName = _User.UserName;
                    _user.UserNickName = _User.UserNickName;
                    _user.Role = _User.Role;
                    _user.Password = _User.Password;
                    _user.LoginTime = System.DateTime.Now;
                    _user.LoginIP = Request.UserHostAddress.Replace(".", ":") + "[" + ipsearch.GetAddressByIp(Request.UserHostAddress) + "]";
                    refFlg = _userServiecs.Update(_user);
                    if (refFlg.Code != 1)
                    {
                        return Json(refFlg);
                    }
                }
                else
                {
                    if (_userServiecs.HasRecord(_User.UserName) == false && !string.IsNullOrEmpty(_User.UserName))
                    {
                        _User.LoginTime = DateTime.Now;
                        _User.RegistrationTime = DateTime.Now;
                        refFlg = _userServiecs.Add(_User);
                        if (refFlg.Code != 1)
                        {
                            return Json(refFlg);
                        }
                    }
                    else
                    {

                        refFlg.Code = 0;
                        refFlg.Message = "[" + _User.UserName + "]" + "该数据已经存在";
                        return Json(refFlg);
                    }
                }
            }
            return Json(new Response());
        }

        public ActionResult List()
        {
            return View();
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public JsonResult Delete(int id)
        {
            Response _resp = _userServiecs.Delete(id);
            return Json(_resp);
        }

        #region 属性
        private IAuthenticationManager AuthenticationManager { get { return HttpContext.GetOwinContext().Authentication; } }
        #endregion


        public ActionResult Upload()
        {
            return View();
        }

        public ActionResult DropzoneUpload()
        {
            return View();
        }

        public ActionResult fileUpload(string ti)
        {
            var _uploadConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~").GetSection("UploadConfig") as UploadConfig;
            //文件最大限制
            int _maxSize = _uploadConfig.MaxSize;
            //保存路径
            string _savePath = "~/" + _uploadConfig.Path + "/";
            //允许上传的类型
            Hashtable extTable = new Hashtable();
            //上传的文件
            int fileCount = Request.Files.Count;
            HttpPostedFileBase _postFile = Request.Files[0];
            String UpFilename = _postFile.FileName;
            _savePath = Server.MapPath(_savePath) + Path.GetExtension(UpFilename).Substring(1);
            //检查上传目录
            if (!Directory.Exists(_savePath)) Directory.CreateDirectory(_savePath);
            _savePath += DateTime.Now.ToString("yyyyMMdd_hhmmss");
            //保存文件
            _postFile.SaveAs(_savePath);
            if (_postFile == null)
                return Json(new { error = '1', message = "请选择文件" });
            else return Json(new { error = 0, url = Url.Content(_savePath) });


        }

        [HttpPost]
        public ActionResult BatchUpload()
        {
            bool isSavedSuccessfully = true;
            int count = 0;
            string msg = "";

            string fileName = "";
            string fileExtension = "";
            string filePath = "";
            string fileNewName = "";

            //这里是获取隐藏域中的数据
            //int albumId = string.IsNullOrEmpty(Request.Params["hidAlbumId"]) ?
            //    0 : int.Parse(Request.Params["hidAlbumId"]);

            try
            {
                string directoryPath = Server.MapPath("~/Content/photos");
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                foreach (string f in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[f];

                    if (file != null && file.ContentLength > 0)
                    {
                        fileName = file.FileName;
                        fileExtension = Path.GetExtension(fileName);
                        fileNewName = Guid.NewGuid().ToString() + fileExtension;
                        filePath = Path.Combine(directoryPath, fileNewName);
                        file.SaveAs(filePath);

                        count++;
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                isSavedSuccessfully = false;
            }

            return Json(new
            {
                Result = isSavedSuccessfully,
                Count = count,
                Message = msg
            });
        }
    }
}