using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Implementation;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementBLL.BusinessServices.Implementation
{
    public class MembershipRepository : GenericRepository<Membership>, IMembershipRepository
    {
        private readonly GymDbContext _dbContext;

        public MembershipRepository(GymDbContext dbContext)
            : base(dbContext) => _dbContext = dbContext;

        public IEnumerable<Membership> GetAllMembershipsLoaded() =>
            _dbContext.Memberships.Include(M => M.Member).Include(M => M.Plan).ToList();
    }
}
