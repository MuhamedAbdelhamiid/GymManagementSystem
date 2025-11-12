using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementBLL.ViewModels.MembershipViewModels;

namespace GymManagementBLL.BusinessServices.Interfaces
{
    public interface IMembershipService
    {
        IEnumerable<MembershipViewModel> GetAllMemberships();

        bool CreateMembership(MembershipToCreateViewModel membership);

        bool CancelMembership(int memberId);

        IEnumerable<MemberSelectViewModel> GetAllAvialableMembers();
        IEnumerable<PlanSelectViewModel> GetAllAvialablePlans();
    }
}
