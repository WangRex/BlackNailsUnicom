using BlackNails.CommonClass;
using BlackNails.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlackNails.DAL
{
    public class AssessmentServices : BaseManager<AssessmentModel>
    {
        public AssessmentModel getAssessment(int Assessment_ID)
        {
            AssessmentModel _AssessmentModel = base.Repository.Find(Assessment_ID);
            if(null == _AssessmentModel || !StringUtil.IsNotNullOrEmpty(_AssessmentModel.Content))
            {
                _AssessmentModel = new AssessmentModel();
                _AssessmentModel.Content = "暂无评价";
                _AssessmentModel.Star = 0;
            }
            return _AssessmentModel;
        }
    }
}