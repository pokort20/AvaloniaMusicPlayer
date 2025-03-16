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
        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Podcast> Podcasts { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        #region Relations
        public DbSet<SongArtist> SongArtists { get; set; }
        public DbSet<PodcastArtist> AlbumArtists { get; set; }
        public DbSet<PodcastArtist> PodcastArtists { get; set; }
        public DbSet<SongPlaylist> SongPlaylists { get; set; }

        public DbSet<AccountPlaylist> AccountPlaylists { get; set; }
        public DbSet<AccountArtist> AccountArtists { get; set; }
        public DbSet<AccountAlbum> AccountAlbums { get; set; }
        public DbSet<AccountPodcast> AccountPodcasts { get; set; }
        public DbSet<AccountSong> AccountSongs { get; set; }

        #endregion
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql("Host=localhost;Username=postgres;Password=admin;Database=MusicPlayer");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //table names
            modelBuilder.Entity<Song>().ToTable("song");
            modelBuilder.Entity<Album>().ToTable("album");
            modelBuilder.Entity<Artist>().ToTable("artist");
            modelBuilder.Entity<Podcast>().ToTable("podcast");
            modelBuilder.Entity<Playlist>().ToTable("playlist");
            modelBuilder.Entity<Account>().ToTable("account");

            modelBuilder.Entity<SongArtist>().ToTable("song_artist");
            modelBuilder.Entity<AlbumArtist>().ToTable("album_artist");
            modelBuilder.Entity<SongPlaylist>().ToTable("song_playlist");
            modelBuilder.Entity<PodcastArtist>().ToTable("podcast_artist");

            modelBuilder.Entity<AccountPlaylist>().ToTable("account_playlist");
            modelBuilder.Entity<AccountArtist>().ToTable("account_artist");
            modelBuilder.Entity<AccountAlbum>().ToTable("account_album");
            modelBuilder.Entity<AccountPodcast>().ToTable("account_podcast");
            modelBuilder.Entity<AccountSong>().ToTable("account_song");



            //primary keys of relation tables
            modelBuilder.Entity<SongArtist>().HasKey(sa => new { sa.songid, sa.artistid });
            modelBuilder.Entity<AlbumArtist>().HasKey(aa => new { aa.albumid, aa.artistid });
            modelBuilder.Entity<PodcastArtist>().HasKey(pa => new { pa.podcastid, pa.artistid });
            modelBuilder.Entity<SongPlaylist>().HasKey(sp => new { sp.songid, sp.playlistid });

            modelBuilder.Entity<AccountPlaylist>().HasKey(ap => new { ap.accountid, ap.playlistid });
            modelBuilder.Entity<AccountArtist>().HasKey(aa => new { aa.accountid, aa.artistid });
            modelBuilder.Entity<AccountAlbum>().HasKey(aa => new { aa.accountid, aa.albumid });
            modelBuilder.Entity<AccountPodcast>().HasKey(ap => new { ap.accountid, ap.podcastid });
            modelBuilder.Entity<AccountSong>().HasKey(asg => new { asg.accountid, asg.songid });


            //Relationships
            modelBuilder.Entity<SongArtist>()
                .HasOne(sa => sa.Song)
                .WithMany(s => s.SongArtists)
                .HasForeignKey(sa => sa.songid);
            modelBuilder.Entity<SongArtist>()
                .HasOne(sa => sa.Artist)
                .WithMany(a => a.SongArtists)
                .HasForeignKey(sa => sa.artistid);

            modelBuilder.Entity<AlbumArtist>()
                .HasOne(aa => aa.Album)
                .WithMany(al => al.AlbumArtists)
                .HasForeignKey(aa => aa.artistid);
            modelBuilder.Entity<AlbumArtist>()
               .HasOne(aa => aa.Artist)
               .WithMany(ar => ar.AlbumArtists)
               .HasForeignKey(aa => aa.albumid);

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

            // Relationships for account relation tables

            modelBuilder.Entity<AccountPlaylist>()
                .HasOne(ap => ap.Account)
                .WithMany(a => a.AccountPlaylists)
                .HasForeignKey(ap => ap.accountid);

            modelBuilder.Entity<AccountPlaylist>()
                .HasOne(ap => ap.Playlist)
                .WithMany(p => p.AccountPlaylists)
                .HasForeignKey(ap => ap.playlistid);

            modelBuilder.Entity<AccountArtist>()
                .HasOne(aa => aa.Account)
                .WithMany(a => a.AccountArtists)
                .HasForeignKey(aa => aa.accountid);

            modelBuilder.Entity<AccountArtist>()
                .HasOne(aa => aa.Artist)
                .WithMany(ar => ar.AccountArtists)
                .HasForeignKey(aa => aa.artistid);

            modelBuilder.Entity<AccountAlbum>()
                .HasOne(aa => aa.Account)
                .WithMany(a => a.AccountAlbums)
                .HasForeignKey(aa => aa.accountid);

            modelBuilder.Entity<AccountAlbum>()
                .HasOne(aa => aa.Album)
                .WithMany(al => al.AccountAlbums)
                .HasForeignKey(aa => aa.albumid);

            modelBuilder.Entity<AccountPodcast>()
                .HasOne(ap => ap.Account)
                .WithMany(a => a.AccountPodcasts)
                .HasForeignKey(ap => ap.accountid);

            modelBuilder.Entity<AccountPodcast>()
                .HasOne(ap => ap.Podcast)
                .WithMany(p => p.AccountPodcasts)
                .HasForeignKey(ap => ap.podcastid);

            modelBuilder.Entity<AccountSong>()
                .HasOne(asg => asg.Account)
                .WithMany(a => a.AccountSongs)
                .HasForeignKey(asg => asg.accountid);

            modelBuilder.Entity<AccountSong>()
                .HasOne(asg => asg.Song)
                .WithMany(s => s.AccountSongs)
                .HasForeignKey(asg => asg.songid);


        }
    }
}
