using BlackNails.Controllers;
using BlackNails.DAL;
using BlackNails.Models;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BlackNails.WebAPI
{
    /// <summary>  
    /// 外线员信息  
    /// </summary>  
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class OutsideTroubleManController : WebAPI2BaseController
    {
        private OutsideTroubleManServices _OutsideTroubleManServices = new OutsideTroubleManServices();
        log4net.ILog log = log4net.LogManager.GetLogger("testApp.Logging");

        /// <summary>
        /// 外线员登陆
        /// </summary>
        /// <param name="EmployeeNo">员工号</param>
        /// <param name="Password">密码</param>
        [HttpGet]
        public HttpResponseMessage login(string EmployeeNo, string Password)
        {
            log.Debug("OutsideTroubleManController.login() Start!");
            log.Debug("EmployeeNo is " + EmployeeNo + ", Password is " + Password);
            var response = new Response();
            response.Code = 0;
            response.Message = "外线员登陆成功！";
            OutsideTroubleManModel _OutsideTroubleManModel = _OutsideTroubleManServices.login(EmployeeNo, Password);
            if (_OutsideTroubleManModel.OutsideTroubleMan_ID == 0)
            {
                response.Code = 1;
                response.Message = "员工号或者密码错误！";
                response.Data = null;
            }
            else
            {
                response.Data = _OutsideTroubleManModel;
            }
            return toJson(response);
        }

        /// <summary>
        /// 获取外线员详情
        /// </summary>
        /// <param name="OTM_ID">外线员ID</param>
        [HttpGet]
        public HttpResponseMessage GetOTM(int OTM_ID)
        {
            log.Debug("OutsideTroubleManController.GetOTM() Start!");
            log.Debug("OTM_ID is " + OTM_ID);
            var response = new Response();
            response.Code = 0;
            response.Message = "获取外线员详情成功！";
            OutsideTroubleManModel _OutsideTroubleManModel = _OutsideTroubleManServices.Find(OTM_ID);
            if (_OutsideTroubleManModel.OutsideTroubleMan_ID == 0)
            {
                response.Code = 1;
                response.Message = "获取外线员详情失败！";
                response.Data = null;
            }
            else
            {
                response.Data = _OutsideTroubleManModel;
            }
            return toJson(response);
        }

    }
}
