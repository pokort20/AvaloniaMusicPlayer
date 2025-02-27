using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaFirstApp.Models
{
    public class SongArtist
    {
        public int songid { get; set; }  // Foreign key to Song
        public Song Song { get; set; }  // Navigation property to Song

        public int artistid { get; set; }  // Foreign key to Artist
        public Artist Artist { get; set; }  // Navigation property to Artist
    }
}
