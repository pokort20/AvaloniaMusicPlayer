using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaFirstApp.Models
{
    public class Album
    {
        public Album()
        {
            //empty constructor
        }
        public Album(int id, string name, byte[] data, int year)
        {
            this.id = id;
            this.name = name;
            this.data = data;
            this.year = year;
        }
        public int id { get; set; }
        public string name { get; set; }
        public byte[] data { get; set; }
        public int year { get; set; }

        public List<AlbumArtist> AlbumArtists { get; set; }
        public List<AccountAlbum> AccountAlbums { get; set; }

        public string AlbumArtistsNames
        {
            get
            {
                string retVal = string.Empty;
                for (int i = 0; i < AlbumArtists.Count; i++)
                {
                    retVal += AlbumArtists[i].Artist.name;

                    if (i < AlbumArtists.Count - 1)
                    {
                        retVal += ", ";
                    }
                }
                return retVal;
            }
        }
    }
}
