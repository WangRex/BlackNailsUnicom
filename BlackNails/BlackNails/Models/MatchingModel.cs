using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlackNails.Models
{
    public class MatchingModel : BaseModels
    {
        [Key]
        public int Matching_ID { get; set; }

        /// <summary>
        /// 匹配地址
        /// </summary>
        [Display(Name = "匹配地址")]
        public string Address { get; set; }

        /// <summary>
        /// 外线员ID
        /// </summary>
        [Display(Name = "外线员ID")]
        public int OTM_ID { get; set; }
    }
}