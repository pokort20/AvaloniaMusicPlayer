using AvaloniaFirstApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaFirstApp.Database
{
    public class DataHandler
    {
        private DatabaseContext db;
        public DataHandler()
        {
            db = new DatabaseContext();
        }
        public async Task<Account> GetUserAccount(int accountid)
        {
            return await db.Accounts.FirstOrDefaultAsync(account => account.id == accountid);
        }
        public async Task<List<Song>> SearchSongs(string searchterm)
        {
            return await db.Songs
                .Where(s => s.name.Contains(searchterm))
                .Include(s => s.SongArtists)
                .ThenInclude(sa => sa.Artist)
                .ToListAsync();
        }
        public async Task<List<Song>> SearchSongs(Artist artist)
        {
            return await db.Songs
                .Include(s => s.SongArtists)
                .ThenInclude(sa => sa.Artist)
                .Where(sa => sa.SongArtists.Any(sa => sa.Artist.id == artist.id))
                .ToListAsync();
        }
        public async Task<List<Song>> SearchSongs(Playlist playlist)
        {
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
            return await db.Albums
                .Where(a => a.name.Contains(searchterm))
                .Include(a => a.AlbumArtists)
                .ThenInclude(aa => aa.Artist)
                .ToListAsync();
        }
        //public async Task<List<Playlist>> SearchPlaylists(string searchterm)
        //{
        //    return await db.Playlists
        //        .Where(p => p.name.Contains(searchterm))
        //        .ToListAsync();
        //}
        public async Task<List<Podcast>> SearchPodcasts(string searchterm)
        {
            return await db.Podcasts
                .Where(p => p.name.Contains(searchterm))
                .Include(p => p.PodcastArtists)
                .ThenInclude(pa => pa.Artist)
                .ToListAsync();
        }
        public async Task<List<T>> SearchItems<T>(string searchTerm) where T : class
        {
            return await db.Set<T>()
                .Where(e => EF.Property<string>(e, "name").Contains(searchTerm))
                .ToListAsync();
        }
    }
}
