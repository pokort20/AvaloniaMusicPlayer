using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaFirstApp.Models
{
    public class PodcastArtist
    {
        public int podcastid { get; set; }  // Foreign key to Song
        public Podcast Podcast { get; set; }  // Navigation property to Song

        public int artistid { get; set; }  // Foreign key to Artist
        public Artist Artist { get; set; }  // Navigation property to Artist
    }
}
