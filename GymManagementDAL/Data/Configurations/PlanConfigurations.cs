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
    internal class PlanConfigurations : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(P => P.Name).HasColumnType("varchar(50)");

            builder.Property(P => P.Description).HasColumnType("varchar(200)");

            builder.Property(P => P.Price).HasPrecision(10, 2);

            builder.ToTable(T =>
                T.HasCheckConstraint("DurationDaysConstraint", "DurationDays between 1 and 365")
            );
        }
    }
}
