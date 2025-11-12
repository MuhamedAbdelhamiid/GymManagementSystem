using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymManagementBLL.BusinessServices.Interfaces;
using GymManagementBLL.ViewModels.BookingViewModels;
using GymManagementBLL.ViewModels.MembershipViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Unit_Of_Work;

namespace GymManagementBLL.BusinessServices.Implementation
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<SessionViewModel> GetAvailableSessionsForBooking()
        {
            var sessionRepository = _unitOfWork.SessionRepository;
            var availableSessions = sessionRepository.GetAllWithCategoryAndTrainer();
            if (availableSessions is null || !availableSessions.Any())
                return [];

            return _mapper.Map<IEnumerable<SessionViewModel>>(
                availableSessions,
                opt =>
                    opt.AfterMap(
                        (src, dest) =>
                        {
                            foreach (var session in dest)
                            {
                                session.AvailableSlots = sessionRepository.CountOfBookingSlots(
                                    session.Id
                                );
                            }
                        }
                    )
            );
        }

        public IEnumerable<MemberSelectViewModel> GetMembersToDropDown()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll();
            if (members is null || !members.Any())
                return [];

            return _mapper.Map<IEnumerable<MemberSelectViewModel>>(members);
        }

        public IEnumerable<MemberDetailsInUpcomingSessionViewModel> GetUpcomingSessionDetails(
            int sessionId
        )
        {
            var session = _unitOfWork.SessionRepository.GetById(sessionId);
            if (session is null)
                return [];

            var isSessionUpcoming = session.StartDate > DateTime.Now;
            if (!isSessionUpcoming)
                return [];

            var bookingDetailsOnSession =
                _unitOfWork.BookingRepository.GetAllBookingBySessionIdLoadedWithMembers(sessionId);

            if (bookingDetailsOnSession is null || !bookingDetailsOnSession.Any())
                return [];

            return _mapper.Map<IEnumerable<MemberDetailsInUpcomingSessionViewModel>>(
                bookingDetailsOnSession
            );
        }

        public IEnumerable<MemberDetailsInOngoingSessionViewModel> GetOngoingSessionDetails(
            int sessionId
        )
        {
            var session = _unitOfWork.SessionRepository.GetById(sessionId);
            if (session is null)
                return [];

            // if session not ongoing
            var isSessionOngoing =
                DateTime.Now >= session.StartDate && session.EndDate >= DateTime.Now;
            if (!isSessionOngoing)
                return [];

            var bookingDetailsOnSession =
                _unitOfWork.BookingRepository.GetAllBookingBySessionIdLoadedWithMembers(sessionId);

            if (bookingDetailsOnSession is null || !bookingDetailsOnSession.Any())
                return [];

            return _mapper.Map<IEnumerable<MemberDetailsInOngoingSessionViewModel>>(
                bookingDetailsOnSession
            );
        }

        public bool CreateBooking(CreateBookingViewModel booking)
        {
            var sessionRepository = _unitOfWork.SessionRepository;
            var session = sessionRepository.GetById(booking.SessionId);
            if (!IsSessionAvailableForBooking(session!))
                return false;

            var member = _unitOfWork.GetRepository<Member>().GetById(booking.MemberId);
            if (!IsMemberCanBook(member!, session!.Id))
                return false;

            var newBooking = _mapper.Map<MemberSession>(booking);
            // Is Attend = false now
            newBooking.IsAttended = false;
            try
            {
                _unitOfWork.BookingRepository.Add(newBooking);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool MarkAsAttended(int sessionId, int memberId)
        {
            if (sessionId <= 0 || memberId <= 0)
                return false;
            var bookingRepository = _unitOfWork.BookingRepository;

            var booking = bookingRepository
                .GetAll(B => B.SessionId == sessionId && B.MemberId == memberId)
                .FirstOrDefault();

            if (booking is null)
                return false;

            booking.IsAttended = true;
            booking.UpdatedAt = DateTime.Now;

            try
            {
                bookingRepository.Update(booking);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CancelBooking(int sessionId, int memberId)
        {
            if (sessionId <= 0 || memberId <= 0)
                return false;
            var bookingRepository = _unitOfWork.BookingRepository;

            // if not future session
            var session = _unitOfWork.SessionRepository.GetById(sessionId);
            if (session is null || !(session.StartDate > DateTime.Now))
                return false;

            var booking = bookingRepository
                .GetAll(B => B.SessionId == sessionId && B.MemberId == memberId)
                .FirstOrDefault();

            if (booking is null)
                return false;

            try
            {
                bookingRepository.Delete(booking);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region Helper Methods
        bool IsSessionAvailableForBooking(Session session)
        {
            // session exists ?
            if (session is null)
                return false;
            // and session has available capacity?
            var sessionHasAvailableCapacity =
                _unitOfWork.SessionRepository.CountOfBookingSlots(session.Id) < session.Capacity;

            if (!sessionHasAvailableCapacity)
                return false;

            // is this session upcoming?

            if (session.StartDate <= DateTime.Now)
                return false;

            return true;
        }

        bool IsMemberCanBook(Member member, int sessionId)
        {
            if (member is null)
                return false;

            var memberMemberships = _unitOfWork.MembershipRepository.GetAll(M =>
                M.MemberId == member.Id
            );
            if (memberMemberships is null || !memberMemberships.Any())
                return false;

            //member has active membership ?
            var memberActiveMembership = memberMemberships.FirstOrDefault(M =>
                M.Status == "Active"
            );
            if (memberActiveMembership is null)
                return false;

            //member book this session
            var memberAlreadyBookedThisSession = _unitOfWork
                .BookingRepository.GetAllBookingBySessionIdLoadedWithMembers(sessionId)
                .Any(M => M.MemberId == member.Id);

            if (memberAlreadyBookedThisSession)
                return false;

            return true;
        }

        #endregion
    }
}
