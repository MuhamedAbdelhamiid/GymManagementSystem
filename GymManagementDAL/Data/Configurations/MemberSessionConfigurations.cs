using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementDAL.Data.Configurations
{
    internal class MemberSessionConfigurations : IEntityTypeConfiguration<MemberSession>
    {
        public void Configure(EntityTypeBuilder<MemberSession> builder)
        {
            builder.HasKey(MS => new { MS.SessionId, MS.MemberId });

            builder.Ignore(MS => MS.Id);

            builder
                .Property(MS => MS.CreatedAt)
                .HasColumnName("BookingDate")
                .HasDefaultValueSql("GetDate()");
            #region Session
            builder
                .HasOne(MS => MS.Session)
                .WithMany(S => S.MemberSessions)
                .HasForeignKey(MS => MS.SessionId);

            #endregion

            #region Member

            builder
                .HasOne(MS => MS.Member)
                .WithMany(M => M.MemberSessions)
                .HasForeignKey(MS => MS.MemberId);

            #endregion
        }
    }
}
