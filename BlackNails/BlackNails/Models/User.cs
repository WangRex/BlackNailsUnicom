
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace BlackNails.Models
{

    /// <summary>
    /// 用户模型
    /// <remarks>
    /// 创建：2016.02.02<br />
    /// 修改：2016.02.05
    /// </remarks>
    /// </summary>
    public class UserModel
    {
        [Key]
        public int UserID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "{1}到{0}个字符")]
        [Display(Name = "手机号或邮箱")]
        public string UserName { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "{1}到{0}个字符")]
        [Display(Name = "用户昵称")]
        public string UserNickName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// 用户状态<br />
        /// 0正常，1锁定，2未通过邮件验证，3未通过管理员
        /// </summary>
        [Display(Name = "用户状态")]
        public string Status { get; set; }

        /// <summary>
        /// 用户权限
        /// </summary>
        [Display(Name = "部门")]
        public string Role { get; set; }

        [Display(Name = "年龄")]
        public Nullable<int> Age { get; set; }

        [Display(Name = "电话")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "号码格式不正确")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        [Display(Name = "注册时间")]
        public DateTime RegistrationTime { get; set; }

        /// <summary>
        /// 上次登陆时间
        /// </summary>
        [Display(Name = "上次登陆时间")]
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// 上次登陆IP
        /// </summary>
        [Display(Name = "上次登陆IP")]
        public string LoginIP { get; set; }

    }

    /// <summary>
    /// 修改密码视图模型
    /// <remarks>创建：2016年3月2日</remarks>
    /// </summary>
    public class ChangePasswordViewModel
    {

        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "{2}到{1}个字符")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        /// <summary>
        /// 原密码
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [Display(Name = "密码")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "{2}到{1}个字符")]
        [DataType(DataType.Password)]
        public string OriginalPassword { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [Display(Name = "新密码")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "{2}到{1}个字符")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [Compare("Password", ErrorMessage = "两次输入的密码不一致")]
        [Display(Name = "确认密码")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }


    /// <summary>
    /// 外线员模型
    /// <remarks>
    /// 创建：2017.02.03<br />
    /// </remarks>
    /// </summary>
    public class OutsideTroubleManModel : BaseModels
    {
        [Key]
        public int OutsideTroubleMan_ID { get; set; }

        /// <summary>
        /// 外线员姓名
        /// </summary>
        [Display(Name = "外线员姓名")]
        public string Name { get; set; }

        /// <summary>
        /// 外线员电话
        /// </summary>
        [Display(Name = "外线员电话")]
        public string Phone { get; set; }

        /// <summary>
        /// 外线员工龄
        /// </summary>
        [Display(Name = "外线员工龄")]
        public string WorkYear { get; set; }

        /// <summary>
        /// 外线员工号
        /// </summary>
        [Display(Name = "外线员工号")]
        public string WorkNo { get; set; }

        /// <summary>
        /// 外线员所属部门
        /// </summary>
        [Display(Name = "外线员所属部门")]
        public string Department { get; set; }

        /// <summary>
        /// 外线员负责区域简介
        /// </summary>
        [Display(Name = "外线员负责区域简介")]
        public string ResponsibleAreaBrief { get; set; }

        /// <summary>
        /// 外线员状态
        /// 1: 可工作; 2: 不可工作;
        /// </summary>
        [Display(Name = "外线员状态")]
        public string Status { get; set; }

        /// <summary>
        /// 入职时间
        /// </summary>
        [Display(Name = "入职时间")]
        public string OnboardTime { get; set; }

        /// <summary>
        /// 离职时间
        /// </summary>
        [Display(Name = "离职时间")]
        public string OffboardTime { get; set; }

        /// <summary>
        /// 外线员备注
        /// </summary>
        [Display(Name = "外线员备注")]
        public string Remark { get; set; }

    }
}
