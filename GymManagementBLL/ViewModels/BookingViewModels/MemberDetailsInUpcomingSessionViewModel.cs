using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.BookingViewModels
{
    public class MemberDetailsInUpcomingSessionViewModel
    {
        public int MemberId { get; set; }
        public int SessionId { get; set; }
        public string MemberName { get; set; } = null!;

        public DateTime StartDate { get; set; }

        #region Computed Properties
        public string BookingDate => $"{StartDate.ToShortDateString()} {StartDate: hh:mm:ss tt}";
        #endregion
    }
}
