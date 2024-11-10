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
    public class PlayerStatsConfiguration : IEntityTypeConfiguration<PlayerStats>
    {
        public void Configure(EntityTypeBuilder<PlayerStats> builder)
        {
            builder.HasKey(ps => ps.AccountId);

            builder.Property(ps => ps.TotalAmount)
                   .HasPrecision(18, 4);

            builder.HasIndex(ps => ps.TotalAmount);

            builder.HasOne(ps => ps.Player)
                   .WithOne(p => p.PlayerStats)
                   .HasForeignKey<PlayerStats>(ps => ps.AccountId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
