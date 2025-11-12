using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementBLL.ViewModels.BookingViewModels;
using GymManagementBLL.ViewModels.MembershipViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;

namespace GymManagementBLL.BusinessServices.Interfaces
{
    public interface IBookingService
    {
        IEnumerable<SessionViewModel> GetAvailableSessionsForBooking();

        IEnumerable<MemberDetailsInUpcomingSessionViewModel> GetUpcomingSessionDetails(
            int sessionId
        );
        IEnumerable<MemberDetailsInOngoingSessionViewModel> GetOngoingSessionDetails(int sessionId);

        IEnumerable<MemberSelectViewModel> GetMembersToDropDown();

        bool CreateBooking(CreateBookingViewModel booking);

        bool MarkAsAttended(int sessionId, int memberId);

        bool CancelBooking(int sessionId, int memberId);
    }
}
