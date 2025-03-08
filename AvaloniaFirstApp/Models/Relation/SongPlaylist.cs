using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaFirstApp.Models
{
    public class SongPlaylist
    {
        public int songid { get; set; }  
        public Song Song { get; set; } 

        public int playlistid { get; set; } 
        public Playlist Playlist { get; set; } 
    }
}
