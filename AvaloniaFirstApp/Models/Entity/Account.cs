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
        public Account(int id, string name, byte[] icon)
        {
            this.id = id;
            this.username = name;
            this.icon = icon;
        }
        public int id { get; set; }
        public string username { get; set; }
        public byte[]? icon { get; set; }

        public List<AccountPlaylist> AccountPlaylists { get; set; }
        public List<AccountArtist> AccountArtists { get; set; }
        public List<AccountAlbum> AccountAlbums { get; set; }
        public List<AccountPodcast> AccountPodcasts { get; set; }
        public List<AccountSong> AccountSongs { get; set; }
    }
}
