using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaFirstApp.Models
{
    public class AccountAlbum
    {
        public int accountid { get; set; }
        public Account Account { get; set; }

        public int albumid { get; set; }
        public Album Album { get; set; }
    }
}
