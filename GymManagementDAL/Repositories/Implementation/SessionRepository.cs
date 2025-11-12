using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementDAL.Repositories.Implementation
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _dbContext;

        public SessionRepository(GymDbContext dbContext)
            : base(dbContext) => _dbContext = dbContext;

        public int CountOfBookingSlots(int sessionId)
        {
            return _dbContext.MemberSessions.Count(S => S.SessionId == sessionId);
        }

        public IEnumerable<Session> GetAllWithCategoryAndTrainer() =>
            _dbContext.Sessions.Include(S => S.Category).Include(S => S.Trainer).ToList();

        public Session? GetSessionWithTrainerAndCategory(int sessionId)
        {
            return _dbContext
                .Sessions.Include(S => S.Trainer)
                .Include(S => S.Category)
                .FirstOrDefault(S => S.Id == sessionId);
        }
    }
}
