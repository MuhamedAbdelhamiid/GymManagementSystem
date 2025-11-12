using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementBLL.BusinessServices.Interfaces;
using GymManagementBLL.ViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Unit_Of_Work;

namespace GymManagementBLL.BusinessServices.Implementation
{
    public class AnaltyicalService : IAnaltyicalService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnaltyicalService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public HomeAnaltyicalViewModel GetHomeAnaltyicalService()
        {
            var sessions = _unitOfWork.GetRepository<Session>().GetAll();
            return new HomeAnaltyicalViewModel()
            {
                TotalMembers = _unitOfWork.GetRepository<Member>().GetAll().Count(),
                TotalTrainers = _unitOfWork.GetRepository<Trainer>().GetAll().Count(),
                ActiveMembers = _unitOfWork
                    .GetRepository<Membership>()
                    .GetAll(ms => ms.Status == "Active")
                    .Count(),
                UpcomingSessions = sessions.Count(S => S.StartDate > DateTime.Now),
                OngoingSessions = sessions.Count(S =>
                    S.StartDate <= DateTime.Now && S.EndDate >= DateTime.Now
                ),
                CompletedSessions = sessions.Count(S => S.EndDate < DateTime.Now),
            };
        }
    }
}
