using BlackNails.Models;
using System.Linq;

namespace BlackNails.DAL
{
    public class MatchingServices : BaseManager<MatchingModel>
    {

        public int getOTM_IDByAddress(string matchingAddress) {
            int OTM_ID = 0;
            IQueryable<MatchingModel> _Matchings = base.Repository.FindList().Where(mm => mm.Address == matchingAddress);
            if(_Matchings.Count() > 0)
            {
                OTM_ID = _Matchings.First().OTM_ID;
            }
            return OTM_ID;
        }
    }
}