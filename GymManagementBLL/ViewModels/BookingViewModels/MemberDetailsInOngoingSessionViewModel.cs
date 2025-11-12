using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.BookingViewModels
{
    public class MemberDetailsInOngoingSessionViewModel
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; } = null!;
        public bool IsAttended { get; set; }
    }
}
