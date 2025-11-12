using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MembershipViewModels
{
    public class MembershipViewModel
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; } = null!;
        public string PlanName { get; set; } = null!;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        #region Computed Properties
        public string StartDateDisplay => $"{StartDate:MMM dd, yyyy}";
        public string EndDateDisplay => $"{EndDate:MMM dd, yyyy}";
        #endregion
    }
}
