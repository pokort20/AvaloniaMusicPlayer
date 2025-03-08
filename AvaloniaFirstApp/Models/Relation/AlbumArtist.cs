using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaFirstApp.Models
{
    public class AlbumArtist
    {
        public int albumid { get; set; } 
        public Album Album { get; set; } 

        public int artistid { get; set; } 
        public Artist Artist { get; set; } 
    }
}
