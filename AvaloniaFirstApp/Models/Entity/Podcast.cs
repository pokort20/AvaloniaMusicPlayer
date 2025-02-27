using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaFirstApp.Models
{
    public class Podcast
    {
        public Podcast()
        {
            //empty constructor
        }
        public Podcast(int id, string name, string description, byte[] data)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.data = data;
        }
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public byte[] data { get; set; }

        public List<PodcastArtist> PodcastArtists { get; set; }
    }
}
