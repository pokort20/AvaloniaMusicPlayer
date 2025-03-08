using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaFirstApp.Models
{
    public class Account : ReactiveObject
    {
        public Account()
        {
            //empty constructor
        }
        public Account(int id, string name, byte[] data)
        {
            this.id = id;
            this.username = name;
            this.data = data;
        }
        public int id { get; set; }
        public string username { get; set; }
        public byte[] data { get; set; }

        public List<AccountPlaylist> AccountPlaylists { get; set; }
        public List<AccountArtist> AccountArtists { get; set; }
        public List<AccountAlbum> AccountAlbums { get; set; }
        public List<AccountPodcast> AccountPodcasts { get; set; }
        public List<AccountSong> AccountSongs { get; set; }
    }
}
