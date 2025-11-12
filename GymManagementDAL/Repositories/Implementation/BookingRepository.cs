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
    public class BookingRepository : GenericRepository<MemberSession>, IBookingRepository
    {
        private readonly GymDbContext _dbContext;

        public BookingRepository(GymDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<MemberSession> GetAllBookingBySessionIdLoadedWithMembers(int sessionId)
        {
            return _dbContext
                .MemberSessions.Include(M => M.Member)
                .Where(M => M.SessionId == sessionId)
                .ToList();
        }
    }
}
