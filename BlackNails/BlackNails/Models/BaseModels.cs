using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BlackNails.Models
{
    public class BaseModels
    {
        //------------------------每个表必须有的项目-----------------------------------------
        [ScaffoldColumn(false)]
        [Display(Name = "创建时间")]
        public DateTime CreateTime { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "创建人")]
        public string CreatePerson { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "编辑时间")]
        public DateTime UpdateTime { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "编辑人")]
        public string UpdatePerson { get; set; }
    }
}