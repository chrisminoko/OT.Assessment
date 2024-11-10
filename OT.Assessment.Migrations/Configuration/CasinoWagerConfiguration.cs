using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OT.Assessment.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Migrations.Configuration
{
    public class CasinoWagerConfiguration : IEntityTypeConfiguration<CasinoWager>
    {
        public void Configure(EntityTypeBuilder<CasinoWager> builder)
        {
            builder.HasKey(w => w.WagerId);

            builder.Property(w => w.Amount)
                   .HasPrecision(18, 4);

            builder.HasIndex(w => new { w.AccountId, w.CreatedDate });
            builder.HasIndex(w => w.Amount);
            builder.HasIndex(w => w.TransactionId);

            builder.HasOne(w => w.Player)
                   .WithMany(p => p.Wagers)
                   .HasForeignKey(w => w.AccountId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(w => w.Game)
                   .WithMany(g => g.Wagers)
                   .HasForeignKey(w => w.GameId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
