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
    internal class GymUserConfigurations<T> : IEntityTypeConfiguration<T>
        where T : GymUser, new()
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(GU => GU.Name).HasColumnType("varchar(50)");

            builder.Property(GU => GU.Email).HasColumnType("varchar(100)");

            builder.ToTable(T =>
                T.HasCheckConstraint("EmailValidConstraintFormat", "Email like '_%@_%._%'")
            );

            builder.HasIndex(T => T.Email).IsUnique();

            builder.Property(GU => GU.Phone).HasColumnType("varchar(11)");

            builder.ToTable(T =>
                T.HasCheckConstraint(
                    "EgyptianPhoneValidConstraint",
                    "Phone LIKE '01[0,1,2,5][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'"
                )
            );

            builder.HasIndex(T => T.Email).IsUnique();

            builder.OwnsOne(
                GU => GU.Address,
                AddressBuilder =>
                {
                    AddressBuilder.Property(A => A.Street).HasColumnType("varchar(30)");

                    AddressBuilder.Property(A => A.City).HasColumnType("varchar(30)");
                }
            );
        }
    }
}
