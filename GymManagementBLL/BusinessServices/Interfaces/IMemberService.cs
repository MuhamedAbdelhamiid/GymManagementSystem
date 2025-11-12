using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementBLL.ViewModels.MemberViewModels;

namespace GymManagementBLL.BusinessServices.Interfaces
{
    public interface IMemberService
    {
        public IEnumerable<MemberViewModel> GetAllMembers();
        bool CreateMember(CreateMemberViewModel createMember);

        MemberViewModel? GetMemberDetails(int memberId);

        HealthRecordViewModel? GetMemberHealthRecord(int memberId);
        MemberToUpdateViewModel? GetMemberDetailsToUpdate(int memberId);

        bool UpdateMember(int id, MemberToUpdateViewModel memberToUpdate);

        bool DeleteMember(int memberId);
    }
}
