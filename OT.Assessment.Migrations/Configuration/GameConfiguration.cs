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
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.HasKey(g => g.GameId);

            builder.Property(g => g.GameName)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(g => g.Theme)
                   .HasMaxLength(50);

            builder.HasIndex(g => new { g.ProviderId, g.GameName })
                   .IsUnique();

            builder.HasOne(g => g.Provider)
                   .WithMany(p => p.Games)
                   .HasForeignKey(g => g.ProviderId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
