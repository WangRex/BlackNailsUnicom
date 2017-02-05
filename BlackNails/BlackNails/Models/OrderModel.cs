﻿using System.ComponentModel.DataAnnotations;

namespace BlackNails.Models
{
    public class OrderModel : BaseModels
    {
        [Key]
        public int Order_ID { get; set; }

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
        /// </summary>
        [Display(Name = "订单状态")]
        public string Status { get; set; }

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