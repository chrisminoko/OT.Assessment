using Microsoft.EntityFrameworkCore;
using OT.Assessment.Migrations.Configuration;
using OT.Assessment.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Migrations
{
    public class AppDbContext : DbContext
    {
        public AppDbContext( DbContextOptions<AppDbContext> options)
            :base(options)
        {
                
        }

        public DbSet<Player> Players { get; set; } 
        public DbSet<Provider> Providers { get; set; } 
        public DbSet<Game> Games { get; set; } 
        public DbSet<CasinoWager> CasinoWagers { get; set; }
        public DbSet<PlayerStats> PlayerStats { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PlayerConfiguration());
            modelBuilder.ApplyConfiguration(new ProviderConfiguration());
            modelBuilder.ApplyConfiguration(new GameConfiguration());
            modelBuilder.ApplyConfiguration(new CasinoWagerConfiguration());
            modelBuilder.ApplyConfiguration(new PlayerStatsConfiguration());
        }

    }
}
