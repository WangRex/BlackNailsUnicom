using System.ComponentModel.DataAnnotations;

namespace BlackNails.Models
{
    public class OrderModel : BaseModels
    {
        [Key]
        public int Order_ID { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [Display(Name = "用户姓名")]
        public string Name { get; set; }

        /// <summary>
        /// 所在省市
        /// </summary>
        [Display(Name = "所在省市")]
        public string Province { get; set; }

        /// <summary>
        /// 所在城市
        /// </summary>
        [Display(Name = "所在城市")]
        public string City { get; set; }

        /// <summary>
        /// 所在辖区
        /// </summary>
        [Display(Name = "所在辖区")]
        public string Area { get; set; }

        /// <summary>
        /// 订单时间
        /// </summary>
        [Display(Name = "订单时间")]
        public string Time { get; set; }

        /// <summary>
        /// 订单地址
        /// </summary>
        [Display(Name = "订单地址")]
        public string Address { get; set; }

        /// <summary>
        /// 订单匹配地址
        /// </summary>
        [Display(Name = "订单匹配地址")]
        public string MatchingAddress { get; set; }

        /// <summary>
        /// 订单手机号
        /// </summary>
        [Display(Name = "订单手机号")]
        public string Phone { get; set; }

        /// <summary>
        /// 订单座机号
        /// </summary>
        [Display(Name = "订单座机号")]
        public string Tel { get; set; }

        /// <summary>
        /// 订单维修类型
        /// </summary>
        [Display(Name = "订单维修类型")]
        public string Type { get; set; }

        /// <summary>
        /// 订单状态
        /// 新订单/待接单/处理中/待评价/已完成
        /// </summary>
        [Display(Name = "订单状态")]
        public string Status { get; set; }

        /// <summary>
        /// 订单描述
        /// </summary>
        [Display(Name = "订单描述")]
        public string Description { get; set; }

        /// <summary>
        /// 外线员ID
        /// </summary>
        [Display(Name = "外线员ID")]
        public int OTM_ID { get; set; }

        /// <summary>
        /// 评价ID
        /// </summary>
        [Display(Name = "评价ID")]
        public int Assessment_ID { get; set; }
    }
    public class OrderHistoryModel : BaseModels
    {
        [Key]
        public int OrderHistory_ID { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        [Display(Name = "订单ID")]
        public int Order_ID { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        [Display(Name = "订单状态")]
        public string Status { get; set; }
    }
}