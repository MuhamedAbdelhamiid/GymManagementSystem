using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Member : GymUser
    {
        // Change CreatedAt to JoinDate in FluentAPI
        public string Photo { get; set; } = null!;

        #region Relationships

        #region Member Has HealthRecord
        public HealthRecord HealthRecord { get; set; } = null!;
        #endregion

        #region Member Has Plans

        public ICollection<Membership> Memberships { get; set; } = null!;

        #endregion

        #region Member Has Sessions
        public ICollection<MemberSession> MemberSessions { get; set; } = null!;
        #endregion

        #endregion
    }
}
