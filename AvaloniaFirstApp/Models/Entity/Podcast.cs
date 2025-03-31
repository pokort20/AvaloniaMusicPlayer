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
        public Podcast(int id, string name, string description, byte[] icon, byte[] data, TimeSpan duration)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.icon = icon;
            this.data = data;
            this.duration = duration;
        }
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public byte[]? icon { get; set; }
        public byte[]? data { get; set; }
        public TimeSpan duration { get; set; }
        public List<PodcastArtist> PodcastArtists { get; set; }
        public List<AccountPodcast> AccountPodcasts { get; set; }
    }
}
