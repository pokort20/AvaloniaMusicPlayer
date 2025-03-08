using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaFirstApp.Models
{
    public class Playlist
    {
        public Playlist()
        {
            //empty constructor
        }
        public Playlist(int id, string name, byte[] data)
        {
            this.id = id;
            this.name = name;
            this.data = data;
        }
        public int id { get; set; }
        public string name { get; set; }
        public byte[] data { get; set; }

        public List<SongPlaylist> SongPlaylists { get; set; }
        public List<AccountPlaylist> AccountPlaylists { get; set; }

    }
}
