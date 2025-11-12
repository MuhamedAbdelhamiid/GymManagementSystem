using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementDAL.Data.Configurations
{
    internal class SessionConfigurations : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(T =>
            {
                T.HasCheckConstraint("CapacityConstraint", "Capacity between 1 and 25");
                T.HasCheckConstraint("EndDateConstraint", "EndDate > StartDate");
                // Make sure that is the entered EndDate is greater than StartDate
            });
        }
    }
}
