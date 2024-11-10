using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OT.Assessment.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Migrations.Configuration
{
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.HasKey(p => p.AccountId);

            builder.Property(p => p.Username)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(p => p.CountryCode)
                   .HasMaxLength(2);

            builder.HasIndex(p => p.Username)
                   .IsUnique();
        }
    }
}
