using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GymManagementBLL.BusinessServices.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Implementation;
using GymManagementDAL.Repositories.Interfaces;
using GymManagementDAL.Unit_Of_Work;

namespace GymManagementBLL.BusinessServices.Implementation
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateSession(CreateSessionViewModel session)
        {
            try
            {
                if (
                    !IsTrainerExist(session.TrainerId)
                    || !IsCategoryExist(session.CategoryId)
                    || !IsDateTimeValid(session.StartDate, session.EndDate)
                )
                    return false;
                // Business Rule
                if (session.Capacity > 25 || session.Capacity < 0)
                    return false;

                // Info to be noted
                var mappedSession = _mapper.Map<Session>(session);

                _unitOfWork.SessionRepository.Add(mappedSession);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<SessionViewModel> GetAllSession()
        {
            var sessionRepository = _unitOfWork.SessionRepository;
            var sessions = sessionRepository.GetAllWithCategoryAndTrainer();
            if (sessions is null)
                return [];

            var mappedSessions = _mapper.Map<IEnumerable<SessionViewModel>>(
                sessions,
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
            return mappedSessions;
        }

        public SessionViewModel? GetSessionDetails(int sessionId)
        {
            var sessionRepository = _unitOfWork.SessionRepository;
            var session = sessionRepository.GetSessionWithTrainerAndCategory(sessionId);
            if (session is null)
                return null;

            var mappedSession = _mapper.Map<Session, SessionViewModel>(session);
            mappedSession.AvailableSlots = sessionRepository.CountOfBookingSlots(sessionId);
            return mappedSession;
        }

        public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
        {
            var session = _unitOfWork.SessionRepository.GetById(sessionId);
            if (!IsSessionAvailabeToUpdate(session!))
                return null;

            var mappedSessionToUpdate = _mapper.Map<UpdateSessionViewModel>(session);
            return mappedSessionToUpdate;
        }

        public bool UpdateSession(int sessionId, UpdateSessionViewModel sessionToUpdate)
        {
            // We need to check if the session is able to update also
            // Avoiding logical error {check on another id from GetSessionToUpdate}

            var session = _unitOfWork.SessionRepository.GetById(sessionId);

            if (!IsSessionAvailabeToUpdate(session!))
                return false;

            if (!IsTrainerExist(sessionToUpdate.TrainerId))
                return false;

            if (!IsDateTimeValid(sessionToUpdate.StartDate, sessionToUpdate.EndDate))
                return false;

            try
            {
                _mapper.Map(sessionToUpdate, session);
                _unitOfWork.SessionRepository.Update(session!);
                _unitOfWork.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteSession(int sessionId)
        {
            var session = _unitOfWork.SessionRepository.GetById(sessionId);
            if (!IsSessionAbailableForRemoving(session!))
                return false;

            try
            {
                _unitOfWork.SessionRepository.Delete(session!);
                _unitOfWork.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<TrainerSelectViewModel> GetTrainersWithNameAndID()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();

            if (trainers is null || !trainers.Any())
                return [];

            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainers);
        }

        public IEnumerable<CategorySelectViewModel> GetCategoriesWithNameAndID()
        {
            var categories = _unitOfWork.GetRepository<Category>().GetAll();

            if (categories is null || !categories.Any())
                return [];

            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(categories);
        }

        #region Helper Methods
        private bool IsTrainerExist(int trainerId) =>
            _unitOfWork.GetRepository<Trainer>().GetById(trainerId) is not null;

        private bool IsCategoryExist(int categoryId) =>
            _unitOfWork.GetRepository<Category>().GetById(categoryId) is not null;

        private bool IsDateTimeValid(DateTime startDate, DateTime endDate) =>
            startDate < endDate && startDate > DateTime.Now;

        private bool IsSessionAvailabeToUpdate(Session session)
        {
            if (session is null)
                return false;

            var isSessionCompleted = DateTime.Now > session.EndDate;
            if (isSessionCompleted)
                return false;

            var isSessionOngoing =
                DateTime.Now >= session.StartDate && session.EndDate >= DateTime.Now;
            if (isSessionOngoing)
                return false;

            var isSessionHasActiveBooking =
                _unitOfWork.SessionRepository.CountOfBookingSlots(session.Id) > 0;
            if (isSessionHasActiveBooking)
                return false;
            return true;
        }

        private bool IsSessionAbailableForRemoving(Session session)
        {
            if (session is null)
                return false;

            // Session Ongoing
            if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now)
                return false;

            // Session is UpComing
            if (session.StartDate > DateTime.Now)
                return false;

            var isSessionHasActiveBooking =
                _unitOfWork.SessionRepository.CountOfBookingSlots(session.Id) > 0;
            if (isSessionHasActiveBooking)
                return false;
            return true;
        }

        #endregion
    }
}
