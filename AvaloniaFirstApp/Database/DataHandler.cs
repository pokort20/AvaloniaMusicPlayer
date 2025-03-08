using AvaloniaFirstApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaFirstApp.Database
{
    public class DataHandler
    {
        public async Task<Account> GetUserAccount(int accountid)
        {
            using var db = new DatabaseContext();
            return await db.Accounts.FirstOrDefaultAsync(account => account.id == accountid);
        }

        public async Task<List<Song>> TrendingSongs(int n)
        {
            Debug.WriteLine("TrendingSongs db access");
            using var db = new DatabaseContext();
            return await db.Songs
                .OrderBy(s => EF.Functions.Random())
                .Take(n)
                .Include(s => s.SongArtists)
                .ThenInclude(sa => sa.Artist)
                .ToListAsync();
        }

        public async Task<List<Artist>> TrendingArtists(int n)
        {
            using var db = new DatabaseContext();
            return await db.Artists
                .OrderBy(a => EF.Functions.Random())
                .Take(n)
                .ToListAsync();
        }

        public async Task<List<Song>> SuggestedSongs(int n)
        {
            using var db = new DatabaseContext();
            return await db.Songs
                .OrderBy(s => EF.Functions.Random())
                .Take(n)
                .Include(s => s.SongArtists)
                .ThenInclude(sa => sa.Artist)
                .ToListAsync();
        }

        public async Task<List<Artist>> SuggestedArtists(int n)
        {
            using var db = new DatabaseContext();
            return await db.Artists
                .OrderBy(a => EF.Functions.Random())
                .Take(n)
                .ToListAsync();
        }

        public async Task<List<Song>> SearchSongs(string searchterm)
        {
            using var db = new DatabaseContext();
            return await db.Songs
                .Where(s => s.name.Contains(searchterm))
                .Include(s => s.SongArtists)
                .ThenInclude(sa => sa.Artist)
                .ToListAsync();
        }

        public async Task<List<Song>> SearchSongs(Artist artist)
        {
            using var db = new DatabaseContext();
            return await db.Songs
                .Include(s => s.SongArtists)
                .ThenInclude(sa => sa.Artist)
                .Where(sa => sa.SongArtists.Any(sa => sa.Artist.id == artist.id))
                .ToListAsync();
        }

        public async Task<List<Song>> SearchSongs(Playlist playlist)
        {
            using var db = new DatabaseContext();
            return await db.Songs
                .Include(s => s.SongPlaylists)
                    .ThenInclude(sp => sp.Playlist)
                .Include(s => s.SongArtists)
                    .ThenInclude(sa => sa.Artist)
                .Where(sp => sp.SongPlaylists.Any(sp => sp.Playlist.id == playlist.id))
                .ToListAsync();
        }

        public async Task<List<Album>> SearchAlbums(string searchterm)
        {
            using var db = new DatabaseContext();
            return await db.Albums
                .Where(a => a.name.Contains(searchterm))
                .Include(a => a.AlbumArtists)
                .ThenInclude(aa => aa.Artist)
                .ToListAsync();
        }

        public async Task<List<Podcast>> SearchPodcasts(string searchterm)
        {
            using var db = new DatabaseContext();
            return await db.Podcasts
                .Where(p => p.name.Contains(searchterm))
                .Include(p => p.PodcastArtists)
                .ThenInclude(pa => pa.Artist)
                .ToListAsync();
        }

        public async Task<List<T>> SearchItems<T>(string searchTerm) where T : class
        {
            using var db = new DatabaseContext();
            return await db.Set<T>()
                .Where(e => EF.Property<string>(e, "name").Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<bool> AddToPlaylist(Song s, Playlist p)
        {
            Debug.WriteLine("Adding song " + s.name + " to playlist " + p.name);
            return false;
        }

        public async Task<List<Playlist>> GetFavouritePlaylists(Account account)
        {
            Debug.WriteLine("favourite playlists db access");
            using var db = new DatabaseContext();
            return await db.Playlists
                .Where(p => p.AccountPlaylists.Any(ap => ap.accountid == account.id))
                .ToListAsync();
        }

        //public async Task<List<T>> GetAccountFavourite<T>(Account account) where T : class
        //{
        //    using var db = new DatabaseContext();
        //    string foreignKeyName = typeof(T) switch
        //    {
        //        var t when t == typeof(Playlist) => "accountid",
        //        var t when t == typeof(Artist) => "accountid",
        //        var t when t == typeof(Album) => "accountid",
        //        var t when t == typeof(Podcast) => "accountid",
        //        var t when t == typeof(Song) => "accountid",
        //        _ => throw new ArgumentException("Unsupported entity type.")
        //    };

        //    return await db.Set<T>()
        //        .Where(e => EF.Property<int>(e, foreignKeyName) == account.id)
        //        .ToListAsync();
        //}

        //public async Task<List<Playlist>> GetPlaylistsForAccount(Account account)
        //{
        //    using var db = new DatabaseContext();
        //    return await db.AccountPlaylists
        //        .Where(ap => ap.accountid == account.id)
        //        .Select(ap => ap.Playlist)
        //        .ToListAsync();
        //}
    }
}
