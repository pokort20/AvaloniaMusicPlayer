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
        public DbSet<Song> Songs { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Podcast> Podcasts { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<SongArtist> SongArtists { get; set; }
        public DbSet<SongPlaylist> SongPlaylists { get; set; }
        public DbSet<PodcastArtist> PodcastArtists { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql("Host=localhost;Username=postgres;Password=admin;Database=MusicPlayer");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //table names
            modelBuilder.Entity<Song>().ToTable("song");
            modelBuilder.Entity<Artist>().ToTable("artist");
            modelBuilder.Entity<Podcast>().ToTable("podcast");
            modelBuilder.Entity<Playlist>().ToTable("playlists");

            modelBuilder.Entity<SongArtist>().ToTable("song_artist");
            modelBuilder.Entity<SongPlaylist>().ToTable("song_playlist");
            modelBuilder.Entity<PodcastArtist>().ToTable("podcast_artist");

            //primary keys of relation tables
            modelBuilder.Entity<SongArtist>().HasKey(sa => new { sa.songid, sa.artistid });
            modelBuilder.Entity<SongPlaylist>().HasKey(sp => new { sp.songid, sp.playlistid });
            modelBuilder.Entity<PodcastArtist>().HasKey(pa => new { pa.podcastid, pa.artistid });


            //Relationships
            modelBuilder.Entity<SongArtist>()
                .HasOne(sa => sa.Song)
                .WithMany(s => s.SongArtists)
                .HasForeignKey(sa => sa.songid);
            modelBuilder.Entity<SongArtist>()
                .HasOne(sa => sa.Artist)
                .WithMany(a => a.SongArtists)
                .HasForeignKey(sa => sa.artistid);

            modelBuilder.Entity<SongPlaylist>()
                .HasOne(sp => sp.Song)
                .WithMany(s => s.SongPlaylists)
                .HasForeignKey(sp => sp.songid);
            modelBuilder.Entity<SongPlaylist>()
                .HasOne(sp => sp.Playlist)
                .WithMany(p => p.SongPlaylists)
                .HasForeignKey(sp => sp.playlistid);

            modelBuilder.Entity<PodcastArtist>()
                .HasOne(pa => pa.Podcast)
                .WithMany(p => p.PodcastArtists)
                .HasForeignKey(pa => pa.podcastid);
            modelBuilder.Entity<PodcastArtist>()
                .HasOne(pa => pa.Artist)
                .WithMany(a => a.PodcastArtists)
                .HasForeignKey(pa => pa.artistid);

        }
    }
}
