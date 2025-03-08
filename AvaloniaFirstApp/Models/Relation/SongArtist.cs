using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaFirstApp.Models
{
    public class SongArtist
    {
        public int songid { get; set; } 
        public Song Song { get; set; }  

        public int artistid { get; set; }  
        public Artist Artist { get; set; } 
    }
}
