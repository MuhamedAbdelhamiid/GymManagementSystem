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
    internal class TrainerConfigurations
        : GymUserConfigurations<Trainer>,
            IEntityTypeConfiguration<Trainer>
    {
        public new void Configure(EntityTypeBuilder<Trainer> builder)
        {
            base.Configure(builder);

            builder
                .Property(T => T.CreatedAt)
                .HasColumnName("HireDate")
                .HasDefaultValueSql("GetDate()");

            #region Trainer - Session
            builder
                .HasMany(T => T.Sessions)
                .WithOne(S => S.Trainer)
                .HasForeignKey(S => S.TrainerId);

            #endregion
        }
    }
}
