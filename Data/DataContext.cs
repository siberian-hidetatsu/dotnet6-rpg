using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>().HasData(
                new Skill{Id=1,Name="Fireball",Damege=30},
                new Skill{Id=2,Name="Frenzy",Damege=20},
                new Skill{Id=3,Name="Blizzard",Damege=50},
                new Skill{Id=4,Name="NekoPunch",Damege=100}
            );
        }

        //public DbSet<Character> Characters => Set<Character>();
        public DbSet<Character> Characters { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        public DbSet<Skill> Skills { get; set; }
    }
}