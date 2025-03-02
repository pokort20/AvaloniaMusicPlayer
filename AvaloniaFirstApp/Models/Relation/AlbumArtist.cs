using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaFirstApp.Models
{
    public class AlbumArtist
    {
        public int albumid { get; set; }  // Foreign key to Song
        public Album Album { get; set; }  // Navigation property to Song

        public int artistid { get; set; }  // Foreign key to Artist
        public Artist Artist { get; set; }  // Navigation property to Artist
    }
}
