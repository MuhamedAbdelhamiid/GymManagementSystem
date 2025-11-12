using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementDAL.Unit_Of_Work
{
    public interface IUnitOfWork
    {
        ISessionRepository SessionRepository { get; }
        IMembershipRepository MembershipRepository { get; }
        IBookingRepository BookingRepository { get; }
        public IGenericRepository<TEntity> GetRepository<TEntity>()
            where TEntity : BaseEntity, new();

        public int SaveChanges();
    }
}
