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
    internal class HealthRecordConfigurations : IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            builder.ToTable("Members");
            builder.Property(h => h.Height).HasPrecision(5, 2);
            builder.Property(h => h.Weight).HasPrecision(5, 2);
            #region Member - HealthRecord

            builder
                .HasOne<Member>()
                .WithOne(H => H.HealthRecord)
                .HasForeignKey<HealthRecord>(H => H.Id);
            builder.Ignore(H => H.CreatedAt);
            #endregion
        }
    }
}
