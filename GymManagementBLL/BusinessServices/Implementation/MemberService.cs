using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using AutoMapper;
using GymManagementBLL.BusinessServices.Interfaces;
using GymManagementBLL.Helper;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Unit_Of_Work;

namespace GymManagementBLL.BusinessServices.Implementation
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachmentService;

        public MemberService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IAttachmentService attachmentService
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attachmentService = attachmentService;
        }

        public bool CreateMember(CreateMemberViewModel createMember)
        {

            if (IsEmailExist(createMember.Email) || IsPhoneExist(createMember.Phone))
                return false;


       

            var photoName = _attachmentService.Upload("Members", createMember.Photo);
            if (string.IsNullOrEmpty(photoName))
                return false;

            var member = _mapper.Map<CreateMemberViewModel, Member>(createMember);
            member.Photo = photoName;
            _unitOfWork.GetRepository<Member>().Add(member);

            var isCreated = _unitOfWork.SaveChanges() > 0;
            if (!isCreated)
            {
                _attachmentService.Delete("Members", photoName);
                return false;
            }
            return true;
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll();

            if (members is null || !members.Any())
                return [];

        


          

            return _mapper.Map<IEnumerable<MemberViewModel>>(members);
        }

        public MemberViewModel? GetMemberDetails(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if (member is null)
                return null;

       

            var memberViewModel = _mapper.Map<MemberViewModel>(member);
            //Active Membership

            var memberShip = _unitOfWork
                .GetRepository<Membership>()
                .GetAll(X => X.MemberId == memberId && X.Status == "Active")
                .FirstOrDefault();

            if (memberShip is not null)
            {
                memberViewModel.MembershipStartDate = memberShip.CreatedAt.ToShortDateString();
                memberViewModel.MembershipEndDate = memberShip.EndDate.ToShortDateString();

                var plan = _unitOfWork.GetRepository<Plan>().GetById(memberShip.PlanId);

                memberViewModel.PlanName = plan?.Name;
            }

            return memberViewModel;
        }

        public MemberToUpdateViewModel? GetMemberDetailsToUpdate(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);

            if (member is null)
                return null;



            return _mapper.Map<MemberToUpdateViewModel>(member);
        }

        public HealthRecordViewModel? GetMemberHealthRecord(int memberId)
        {
            var memberHealthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(memberId);

            if (memberHealthRecord is null)
                return null;
   

            return _mapper.Map<HealthRecordViewModel>(memberHealthRecord);
        }

        public bool DeleteMember(int memberId)
        {
            try
            {
                var memberRepo = _unitOfWork.GetRepository<Member>();
                var member = memberRepo.GetById(memberId);
                if (member is null)
                    return false;

                var memberSessionsIds = _unitOfWork
                    .GetRepository<MemberSession>()
                    .GetAll(X => X.MemberId == memberId)
                    .Select(X => X.SessionId); // 1 5 8

                var hasFutureSessions = _unitOfWork
                    .GetRepository<Session>()
                    .GetAll(S => memberSessionsIds.Contains(S.Id) && S.StartDate > DateTime.Now)
                    .Any();

                if (hasFutureSessions)
                    return false;

                var membershipRepo = _unitOfWork.GetRepository<Membership>();
                var memberShips = membershipRepo.GetAll(X => X.MemberId == memberId);

                if (memberShips.Any())
                {
                    foreach (var membership in memberShips)
                    {
                        membershipRepo.Delete(membership); 
                    }
                }

                memberRepo.Delete(member); 

                var isDeleted = _unitOfWork.SaveChanges() > 0;
                if (isDeleted)
                {
                    _attachmentService.Delete("Members", member.Photo);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateMember(int memberId, MemberToUpdateViewModel memberToUpdate)
        {
            try
            {
                var memberRepo = _unitOfWork.GetRepository<Member>();

                var emailExist = memberRepo
                    .GetAll(M => M.Email == memberToUpdate.Email && M.Id != memberId)
                    .Any();
                var phoneExist = memberRepo
                    .GetAll(M => M.Phone == memberToUpdate.Phone && M.Id != memberId)
                    .Any();

                if (emailExist && phoneExist)
                    return false;

                var member = memberRepo.GetById(memberId);

                if (member is null)
                    return false;
               

                _mapper.Map(memberToUpdate, member);

                memberRepo.Update(member);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool IsEmailExist(string email)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(X => X.Email == email).Any();
        }

        private bool IsPhoneExist(string phone)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(X => X.Phone == phone).Any();
        }
    }
}
