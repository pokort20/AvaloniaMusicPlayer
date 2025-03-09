using AvaloniaFirstApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaFirstApp.Support
{
    public class AddToPlaylistParameter
    {
        public Song Song { get; set; }
        public Playlist Playlist { get; set; }

        public AddToPlaylistParameter(Song song, Playlist playlist)
        {
            Song = song;
            Playlist = playlist;
        }
    }

}
