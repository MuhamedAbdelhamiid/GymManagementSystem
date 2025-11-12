using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        IEnumerable<Session> GetAllWithCategoryAndTrainer();

        int CountOfBookingSlots(int sessionId);

        Session? GetSessionWithTrainerAndCategory(int sessionId);
    }
}
