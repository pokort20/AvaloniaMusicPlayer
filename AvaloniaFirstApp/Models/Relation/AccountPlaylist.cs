using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaFirstApp.Models
{
    public class AccountPlaylist
    {
        public int accountid { get; set; } 
        public Account Account { get; set; } 

        public int playlistid { get; set; } 
        public Playlist Playlist { get; set; } 
    }
}
