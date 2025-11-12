using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
    //Can be applied on any entity that will be converted as table in database
    public interface IGenericRepository<TEntity>
        where TEntity : BaseEntity, new()
    {
        public IEnumerable<TEntity> GetAll(Func<TEntity, bool>? condition = null);
        public TEntity? GetById(int id);
        public void Delete(TEntity entity);
        public void Add(TEntity entity);
        public void Update(TEntity entity);
    }
}
