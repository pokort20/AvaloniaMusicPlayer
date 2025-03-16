using Avalonia.Controls;
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
        public Playlist(int id, string name, byte[] icon)
        {
            this.id = id;
            this.name = name;
            this.icon = icon;
        }
        public int id { get; set; }
        public string name { get; set; }
        public byte[] icon { get; set; }

        public List<SongPlaylist> SongPlaylists { get; set; }
        public List<AccountPlaylist> AccountPlaylists { get; set; }

        public override string ToString()
        {
            return "Playlist: " + name;
        }
    }
}
