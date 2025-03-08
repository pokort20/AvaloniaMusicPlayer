using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaFirstApp.Models
{
    public class PodcastArtist
    {
        public int podcastid { get; set; } 
        public Podcast Podcast { get; set; }  

        public int artistid { get; set; }  
        public Artist Artist { get; set; }  
    }
}
