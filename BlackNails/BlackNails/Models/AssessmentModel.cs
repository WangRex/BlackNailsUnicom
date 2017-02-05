using System.ComponentModel.DataAnnotations;

namespace BlackNails.Models
{
    public class AssessmentModel : BaseModels
    {
        [Key]
        public int Assessment_ID { get; set; }

        /// <summary>
        /// 评价内容
        /// </summary>
        [Display(Name = "评价内容")]
        public string Content { get; set; }

        /// <summary>
        /// 评价星级
        /// </summary>
        [Display(Name = "评价星级")]
        public int Star { get; set; }
    }
}