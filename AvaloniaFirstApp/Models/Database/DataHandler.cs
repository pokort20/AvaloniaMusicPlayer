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
        #region trending
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
        #endregion

        #region suggested
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
        #endregion

        #region search
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
        #endregion

        #region favourite
        public async Task<bool> AddToFavourites<T>(Account account, T item) where T : class
        {
            using var db = new DatabaseContext();

            bool isAlreadyFavourite = item switch
            {
                Artist artist => await db.Set<AccountArtist>().AnyAsync(aa => aa.accountid == account.id && aa.artistid == artist.id),
                Album album => await db.Set<AccountAlbum>().AnyAsync(aa => aa.accountid == account.id && aa.albumid == album.id),
                Podcast podcast => await db.Set<AccountPodcast>().AnyAsync(ap => ap.accountid == account.id && ap.podcastid == podcast.id),
                _ => throw new ArgumentException($"Unsupported entity type: {typeof(T).Name}")
            };

            if (isAlreadyFavourite)
            {
                return false;
            }

            object relationEntry = item switch
            {
                Artist artist => new AccountArtist { accountid = account.id, artistid = artist.id },
                Album album => new AccountAlbum { accountid = account.id, albumid = album.id },
                Podcast podcast => new AccountPodcast { accountid = account.id, podcastid = podcast.id },
                _ => throw new ArgumentException($"Unsupported entity type: {typeof(T).Name}")
            };

            db.Add(relationEntry);
            await db.SaveChangesAsync();

            return true; 
        }

        public async Task<bool> RemoveFromFavourites<T>(Account account, T item) where T : class
        {
            using var db = new DatabaseContext();

            object relationEntry = item switch
            {
                Artist artist => await db.Set<AccountArtist>().FirstOrDefaultAsync(aa => aa.accountid == account.id && aa.artistid == (item as Artist).id),
                Album album => await db.Set<AccountAlbum>().FirstOrDefaultAsync(aa => aa.accountid == account.id && aa.albumid == (item as Album).id),
                Podcast podcast => await db.Set<AccountPodcast>().FirstOrDefaultAsync(ap => ap.accountid == account.id && ap.podcastid == (item as Podcast).id),
                _ => throw new ArgumentException("Unsupported entity type.")
            };

            if (relationEntry != null)
            {
                db.Remove(relationEntry);
                await db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<List<Playlist>> GetFavouritePlaylists(Account account)
        {
            Debug.WriteLine("favourite playlists db access");
            //return new List<Playlist>{ new Playlist(1, 1, "Chillec", new byte[1]) };
            using var db = new DatabaseContext();
            return await db.Playlists
                .Where(p => p.AccountPlaylists.Any(ap => ap.accountid == account.id))
                .ToListAsync();
        }
        public async Task<List<Artist>> GetFavouriteArtists(Account account)
        {
            Debug.WriteLine("favourite playlists db access, accountid: " + account.id);
            using var db = new DatabaseContext();
            return await db.Artists
                .Where(a => a.AccountArtists.Any(aa => aa.accountid == account.id))
                .ToListAsync();
        }
        public async Task<List<Album>> GetFavouriteAlbums(Account account)
        {
            Debug.WriteLine("Fetching favourite albums for account: " + account.id);
            using var db = new DatabaseContext();
            return await db.Albums
                .Where(a => a.AccountAlbums.Any(aa => aa.accountid == account.id))
                .Include(a => a.AlbumArtists)
                    .ThenInclude(aa => aa.Artist)
                .ToListAsync();
        }

        public async Task<List<Podcast>> GetFavouritePodcasts(Account account)
        {
            Debug.WriteLine("favourite playlists db access");
            using var db = new DatabaseContext();
            return await db.Podcasts
                .Where(p => p.AccountPodcasts.Any(ap => ap.accountid == account.id))
                .ToListAsync();
        }
        #endregion

        public async Task<bool> AddToPlaylist(Song song, Playlist playlist)
        {
            using var db = new DatabaseContext();

            bool exists = await db.SongPlaylists
                .AnyAsync(sp => sp.songid == song.id && sp.playlistid == playlist.id);

            if (!exists)
            {
                var songPlaylist = new SongPlaylist
                {
                    songid = song.id,
                    playlistid = playlist.id
                };

                db.SongPlaylists.Add(songPlaylist);
                await db.SaveChangesAsync();
                Debug.WriteLine($"Added song '{song.name}' to playlist '{playlist.name}'");
                return true;
            }

            Debug.WriteLine($"Song '{song.name}' is already in playlist '{playlist.name}'");
            return false;
        }
        public async Task<bool> AddPlaylist(Account account, Playlist playlist)
        {
            using var db = new DatabaseContext();
            try
            {
                db.Playlists.Add(playlist);
                await db.SaveChangesAsync();

                var accountPlaylist = new AccountPlaylist
                {
                    accountid = account.id,
                    playlistid = playlist.id
                };

                db.AccountPlaylists.Add(accountPlaylist);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding playlist: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> DeletePlaylist(Account account, Playlist playlist)
        {
            using var db = new DatabaseContext();
            try
            {
                var accountPlaylist = await db.AccountPlaylists
                    .FirstOrDefaultAsync(ap => ap.accountid == account.id && ap.playlistid == playlist.id);
                foreach(var songplaylist in await db.SongPlaylists.Where(sp => sp.playlistid == playlist.id).ToListAsync())
                {
                    db.SongPlaylists.Remove(songplaylist);
                }
                if (accountPlaylist != null)
                {
                    db.AccountPlaylists.Remove(accountPlaylist);
                }

                var existingPlaylist = await db.Playlists.FirstOrDefaultAsync(p => p.id == playlist.id);
                if (existingPlaylist != null)
                {
                    db.Playlists.Remove(existingPlaylist);
                }

                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error deleting playlist: " + ex.Message);
                return false;
            }
        }

    }
}
