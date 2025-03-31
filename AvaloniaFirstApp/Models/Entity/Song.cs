using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaFirstApp.Models
{
    public class Song
    {
        public Song()
        {
            //empty constructor
        }
        public Song(int id, string name, byte[] icon, byte[] data, TimeSpan duration)
        {
            this.id = id;
            this.name = name;
            this.icon = icon;
            this.data = data;
            this.duration = duration;
        }
        public int id { get; set; }
        public string name { get; set; }
        public byte[]? icon { get; set; }
        public byte[]? data { get; set; }
        public TimeSpan duration { get; set; }

        public List<SongArtist> SongArtists { get; set; }
        public List<SongPlaylist> SongPlaylists { get; set; }
        public List<AccountSong> AccountSongs { get; set; }
        public string SongArtistsNames
        {
            get
            {
                string retVal = string.Empty;
                for (int i = 0; i < SongArtists.Count; i++)
                {
                    retVal += SongArtists[i].Artist.name;

                    if (i < SongArtists.Count - 1)
                    {
                        retVal += ", ";
                    }
                }
                return retVal;
            }
        }
        public override string ToString()
        {
            return "Song: " + name;
        }
    }
}
