using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaFirstApp.Models
{
    public class Artist
    {
        //public Artist()
        //{
        //    //empty constructor
        //}
        //public Artist(int id, string name, byte[] icon)
        //{
        //    this.id = id;
        //    this.name = name;
        //    this.icon = icon;
        //}
        public int id { get; set; }
        public string name { get; set; }
        public byte[]? icon { get; set; }

        public List<SongArtist> SongArtists { get; set; }
        public List<AlbumArtist> AlbumArtists { get; set; }
        public List<PodcastArtist> PodcastArtists { get; set; }
        public List<AccountArtist> AccountArtists { get; set; }
    }
}
