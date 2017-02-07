using System;
using System.Collections.Generic;
using BlackNails.Models;
using BlackNails.DAL;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;
using System.Text;

namespace BlackNails.DAL
{
    public class UserServices : BaseManager<UserModel>
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="admin">管理员实体</param>
        /// <returns></returns>
        public override Response Add(UserModel admin)
        {
            Response _resp = new Response();
            if (HasAccounts(admin.UserName))
            {
                _resp.Code = 0;
                _resp.Message = "帐号已存在";
            }
            else _resp = base.Add(admin);
            return _resp;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="administratorID">主键</param>
        /// <returns></returns>
        public override Response Delete(int UserID)
        {
            Response _resp = new Response();
            _resp = base.Delete(UserID);
            return _resp;
        }

        /// <summary>
        /// 删除【批量】返回值Code：1-成功，2-部分删除，0-失败
        /// </summary>
        /// <param name="UserIDList"></param>
        /// <returns></returns>
        public Response Delete(List<int> UserIDList)
        {
            Response _resp = new Response();
            int _totalDel = UserIDList.Count;
            int _totalUser = Count();
            foreach (int i in UserIDList)
            {
                base.Repository.Delete(new UserModel() { UserID = i }, false);
                _totalUser--;
            }
            _resp.Data = base.Repository.Save();
            if (_resp.Data == _totalDel)
            {
                _resp.Code = 1;
                _resp.Message = "成功删除" + _resp.Data + "名用户";
            }
            else if (_resp.Data > 0)
            {
                _resp.Code = 2;
                _resp.Message = "成功删除" + _resp.Data + "名用户";
            }
            else
            {
                _resp.Code = 0;
                _resp.Message = "删除失败";
            }
            return _resp;
        }




        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="administratorID">主键</param>
        /// <param name="password">新密码【密文】</param>
        /// <returns></returns>
        public Response ChangePassword(int UserID, string password)
        {
            Response _resp = new Response();
            var _admin = Find(UserID);
            if (_admin == null)
            {
                _resp.Code = 0;
                _resp.Message = "该主键的用户不存在";
            }
            else
            {
                _admin.Password = password;
                _resp = Update(_admin);
            }
            return _resp;
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="accounts">帐号</param>
        /// <returns></returns>
        public UserModel Find(string UserName)
        {
            return base.Repository.Find(a => a.UserName == UserName);
        }

        /// <summary>
        /// 帐号是否存在
        /// </summary>
        /// <param name="accounts">帐号</param>
        /// <returns></returns>
        public bool HasAccounts(string UserName)
        {
            return base.Repository.IsContains(a => a.UserName.ToUpper() == UserName.ToUpper());
        }

        /// <summary>
        /// 更新登录信息
        /// </summary>
        /// <param name="administratorID">主键</param>
        /// <param name="ip">IP地址</param>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public Response UpadateLoginInfo(int UserID, string ip, DateTime time)
        {
            Response _resp = new Response();
            var _admin = Find(UserID);
            if (_admin == null)
            {
                _resp.Code = 0;
                _resp.Message = "该主键的用户不存在";
            }
            else
            {
                _admin.LoginIP = ip;
                _admin.LoginTime = time;
                _resp = Update(_admin);
            }
            return _resp;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="accounts">帐号</param>
        /// <param name="password">密码【密文】</param>
        /// <returns>Code:1-成功;2-帐号不存在;3-密码错误</returns>
        public Response Verify(string UserName, string password)
        {
            Response _resp = new Response();
            var _admin = base.Repository.Find(a => a.UserName == UserName);
            if (_admin == null)
            {
                _resp.Code = 2;
                _resp.Message = "帐号为:【" + UserName + "】的用户不存在";
            }
            else if (_admin.Password == password)
            {
                _resp.Code = 1;
                _resp.Message = "验证通过";
            }
            else
            {
                _resp.Code = 3;
                _resp.Message = "帐号密码错误";
            }
            return _resp;
        }

        /// <summary>
        /// 记录是否存在
        /// </summary>
        /// <param name="keyname">keyname</param>
        /// <returns></returns>
        public bool HasRecord(string keyname)
        {
            return base.Repository.IsContains(a => a.UserName.ToUpper() == keyname.ToUpper());
        }

        public IQueryable<UserModel> FindPageList(out int totalRecord, string search1, string group, Nullable<DateTime> fromDate, Nullable<DateTime> toDate, int pageIndex, int pageSize, string sort, string order)
        {
            //获取实体列表
            IQueryable<UserModel> _Users = base.Repository.FindList();
            if (!string.IsNullOrEmpty(search1)) _Users = _Users.Where(cm => cm.UserName.Contains(search1));
            if (!string.IsNullOrEmpty(group) && (group != "ALL")) _Users = _Users.Where(cm => cm.Role == group);
            if (fromDate != null) _Users = _Users.Where(cm => cm.LoginTime >= fromDate);
            if (toDate != null)
            {
                toDate = DateTime.Parse(toDate.ToString()).AddDays(1);
                _Users = _Users.Where(cm => cm.LoginTime < toDate);
            }
            _Users = Order(_Users, sort, order);
            totalRecord = _Users == null ? 0 : _Users.Count();
            return PageList(_Users, pageIndex, pageSize).AsQueryable();
        }

        public IQueryable<UserModel> Order(IQueryable<UserModel> entitys, string sort, string order)
        {
            string sortOrder = string.Empty;
            if (sort == null || order == null)
            {

            }
            else
            {
                sortOrder = sort + ":" + order;
            }
            switch (sortOrder)
            {
                case "UserName:asc":
                    entitys = entitys.OrderBy(w => w.UserName);
                    break;
                case "UserName:desc":
                    entitys = entitys.OrderByDescending(w => w.UserName);
                    break;
                case "Role:asc":
                    entitys = entitys.OrderBy(w => w.Role);
                    break;
                case "Role:desc":
                    entitys = entitys.OrderByDescending(w => w.Role);
                    break;
                default:
                    entitys = entitys.OrderBy(w => w.UserID);
                    break;
            }
            return entitys;
        }

        public UserModel FindByUserName(string UserName)
        {
            return base.Repository.Find(a => a.UserName == UserName);
        }

        public UserModel FindByUserNickName(string UserNickName)
        {
            return base.Repository.Find(a => a.UserNickName == UserNickName);
        }

        public ClaimsIdentity CreateIdentity(UserModel user, string authenticationType)
        {
            ClaimsIdentity _identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
            _identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            _identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()));
            _identity.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString()));
            _identity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity"));
            _identity.AddClaim(new Claim("UserNickName", user.UserNickName));
            return _identity;
        }

    }
    public class OutsideTroubleManServices : BaseManager<OutsideTroubleManModel>
    {
        public IQueryable<OutsideTroubleManModel> FindPageList(out int totalRecord, string search1, string group, Nullable<DateTime> fromDate, Nullable<DateTime> toDate, int pageIndex, int pageSize, string sort, string order)
        {
            //获取实体列表
            IQueryable<OutsideTroubleManModel> _OutsideTroubleMans = base.Repository.FindList();
            totalRecord = _OutsideTroubleMans == null ? 0 : _OutsideTroubleMans.Count();
            return PageList(_OutsideTroubleMans, pageIndex, pageSize).AsQueryable();
        }

        public string getOTMName(int OutsideTroubleMan_ID)
        {
            string str = "暂无外线员";
            if(OutsideTroubleMan_ID != 0)
            {
                OutsideTroubleManModel _OutsideTroubleManModel = base.Repository.Find(OutsideTroubleMan_ID);
                str = _OutsideTroubleManModel.Name;
            }
            return str;
        }

    }

    public class CustomerServices : BaseManager<CustomerModel>
    {

    }
}