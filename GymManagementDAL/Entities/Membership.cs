using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Membership : BaseEntity
    {
        public DateTime EndDate { get; set; }
        public string Status
        {
            get
            {
                if (EndDate >= DateTime.Now)
                    return "Active";
                else
                    return "Expired";
            }
        }

        #region Relationships

        #region Member
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;
        #endregion

        #region Plan
        public int PlanId { get; set; }
        public Plan Plan { get; set; } = null!;
        #endregion


        #endregion
    }
}
