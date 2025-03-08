using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaFirstApp.Models
{
    public class AccountSong
    {
        public int accountid { get; set; }
        public Account Account { get; set; }

        public int songid { get; set; }
        public Song Song { get; set; }
    }
}
