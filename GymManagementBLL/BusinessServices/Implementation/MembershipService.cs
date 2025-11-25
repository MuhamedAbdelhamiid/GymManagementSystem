using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymManagementBLL.BusinessServices.Interfaces;
using GymManagementBLL.ViewModels.MembershipViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Unit_Of_Work;

namespace GymManagementBLL.BusinessServices.Implementation
{
    public class MembershipService : IMembershipService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MembershipService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<MembershipViewModel> GetAllMemberships()
        {
            var memberships = _unitOfWork.MembershipRepository.GetAllMembershipsLoaded();
            if (memberships is null || !memberships.Any())
                return [];
            var activeMemberships = memberships.Where(M => M.Status == "Active");
            return _mapper.Map<IEnumerable<MembershipViewModel>>(activeMemberships);
        }

        public bool CreateMembership(MembershipToCreateViewModel membership)
        {
            var membershipRepository = _unitOfWork.MembershipRepository;
            var member = _unitOfWork.GetRepository<Member>().GetById(membership.MemberId);
            if (member is null)
                return false;
            var plan = _unitOfWork.GetRepository<Plan>().GetById(membership.PlanId);
            if (plan is null || !plan.IsActive)
                return false;

            var memberHasActiveMemberships = membershipRepository
                .GetAll(M => M.MemberId == membership.MemberId && M.Status == "Active")
                .Any();

            if (memberHasActiveMemberships)
                return false;

            var newMembership = _mapper.Map<Membership>(membership);
            newMembership.EndDate = DateTime.Today.AddDays(plan.DurationDays);

            try
            {
                membershipRepository.Add(newMembership);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CancelMembership(int memberId)
        {
            var memberMemberships = _unitOfWork.MembershipRepository.GetAll(M =>
                M.MemberId == memberId
            );

            if (memberMemberships is null || !memberMemberships.Any())
                return false;

            var memberActiveMembership = memberMemberships.FirstOrDefault(M =>
                M.Status == "Active"
            );

            if (memberActiveMembership is null)
                return false;

            memberActiveMembership.EndDate = DateTime.Now.AddHours(-1);
            memberActiveMembership.UpdatedAt = DateTime.Now;

            try
            {
                _unitOfWork.MembershipRepository.Update(memberActiveMembership);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<MemberSelectViewModel> GetAllAvialableMembers()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll();
            if (members is null || !members.Any())
                return [];

            return _mapper.Map<IEnumerable<MemberSelectViewModel>>(members);
        }

        public IEnumerable<PlanSelectViewModel> GetAllAvialablePlans()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll();
            if (plans is null || !plans.Any())
                return [];

            return _mapper.Map<IEnumerable<PlanSelectViewModel>>(plans);
        }
    }
}
