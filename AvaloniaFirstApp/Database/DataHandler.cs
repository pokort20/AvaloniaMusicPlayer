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
            await db.SongArtists.ToListAsync();
            return await db.Songs
                .Where(s => s.name.Contains(searchterm))
                .Include(s => s.SongArtists)
                .ThenInclude(sa => sa.Artist)
                .ToListAsync();
        }
    }
}
