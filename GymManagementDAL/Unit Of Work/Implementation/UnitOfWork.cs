using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Implementation;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementDAL.Unit_Of_Work
{
    public class UnitOfWork : IUnitOfWork
    {
        private Dictionary<Type, object> _repositories = new();
        private readonly GymDbContext _dbContext;

        public ISessionRepository SessionRepository { get; }

        public IMembershipRepository MembershipRepository { get; }
        public IBookingRepository BookingRepository { get; }

        public UnitOfWork(
            GymDbContext dbContext,
            ISessionRepository sessionRepository,
            IMembershipRepository membershipRepository,
            IBookingRepository bookingRepository
        )
        {
            _dbContext = dbContext;
            SessionRepository = sessionRepository;
            MembershipRepository = membershipRepository;
            BookingRepository = bookingRepository;
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>()
            where TEntity : BaseEntity, new()
        {
            var entityType = typeof(TEntity);

            if (_repositories.TryGetValue(entityType, out var repository))
                return (IGenericRepository<TEntity>)repository;

            var newRepository = new GenericRepository<TEntity>(_dbContext);
            _repositories.Add(entityType, newRepository);
            return newRepository;
        }

        public int SaveChanges() => _dbContext.SaveChanges();
    }
}
