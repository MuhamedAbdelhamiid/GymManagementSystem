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
    internal class MembershipConfigurations : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> builder)
        {
            builder.HasKey(MS => new { MS.PlanId, MS.MemberId });

            builder
                .Property(MS => MS.CreatedAt)
                .HasColumnName("StartDate")
                .HasDefaultValueSql("GetDate()");

            builder.Ignore(MS => MS.Id);
            builder.Ignore(MS => MS.Status);

            #region Plan
            builder
                .HasOne(MS => MS.Plan)
                .WithMany(P => P.Memberships)
                .HasForeignKey(MS => MS.PlanId);

            #endregion

            #region Member

            builder
                .HasOne(MS => MS.Member)
                .WithMany(M => M.Memberships)
                .HasForeignKey(MS => MS.MemberId);

            #endregion
        }
    }
}
