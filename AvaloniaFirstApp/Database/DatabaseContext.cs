using AvaloniaFirstApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaFirstApp.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Song> Song { get; set; }
        public DbSet<Artist> Artist { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql("Host=localhost;Username=postgres;Password=admin;Database=MusicPlayer");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensure the table name is correct, case-sensitive for PostgreSQL
            modelBuilder.Entity<Song>().ToTable("song");  // The table name in PostgreSQL is lowercase "song"
        }
    }
}
