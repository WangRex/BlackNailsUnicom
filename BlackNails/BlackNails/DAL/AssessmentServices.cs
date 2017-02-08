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
                _AssessmentModel.QuestionStar = 0;
                _AssessmentModel.AttitudeStar = 0;
                _AssessmentModel.ComprehensiveStar = 0;
            }
            return _AssessmentModel;
        }
        public int getGoodAssessment(int OTM_ID)
        {
            //获取实体列表
            IQueryable<AssessmentModel> _Assessments = base.Repository.FindList().Where(am => am.OTM_ID == OTM_ID);
            var starCount = 0;
            var rate = 0;
            foreach(AssessmentModel _AssessmentModel in _Assessments)
            {
                starCount = starCount + _AssessmentModel.ComprehensiveStar;
            }
            if(_Assessments.Count() != 0)
            {
                rate = starCount / (_Assessments.Count() * 5) * 100;
            }
            return rate;
        }
    }
}