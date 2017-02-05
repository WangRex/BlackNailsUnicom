using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlackNails.Models
{
    public class DepartmentModel
    {

        [Key]
        public int Department_ID { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [Display(Name = "部门名称")]
        public string Name { get; set; }
    }
}