using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaFirstApp.Models
{
    public class AccountArtist
    {
        public int accountid { get; set; }
        public Account Account { get; set; }

        public int artistid { get; set; }
        public Artist Artist { get; set; }
    }
}
