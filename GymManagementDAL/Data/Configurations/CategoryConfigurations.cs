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
    internal class CategoryConfigurations : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(C => C.CategoryName).HasColumnType("varchar(20)");

            #region Category - Session

            builder
                .HasMany(C => C.Sessions)
                .WithOne(S => S.Category)
                .HasForeignKey(S => S.CategoryId);

            #endregion
        }
    }
}
