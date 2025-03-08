using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaFirstApp.Models
{
    public class SongQueue
    {
        private Queue<Song> Queue { get; set; }
        private Song? _currentPlayingSong;
        public Song? CurrentPlayingSong
        {
            get => _currentPlayingSong;
            set
            {
                if(value != null && _currentPlayingSong != value)
                {
                    if(Queue.Contains(value))
                    {
                        Song tmp;
                        do
                        {
                            tmp = Queue.Dequeue();
                        }
                        while (tmp != value);
                    }
                    _currentPlayingSong = value;
                }
            }
        }

        private Stack<Song> PlayedSongs { get; set; }
        public SongQueue()
        {
            Queue = new Queue<Song>();
            PlayedSongs = new Stack<Song>();
            CurrentPlayingSong = null;
        }
        public void ClearQueue()
        {
            Queue.Clear();
            PlayedSongs.Clear();
            CurrentPlayingSong = null;
        }
        public void ShuffleQueue(bool includeCurrentPlayingSong)
        {
            if (CurrentPlayingSong == null || Queue.Count <= 0) return;
            Random rand = new Random();
            if(includeCurrentPlayingSong)Queue.Enqueue(CurrentPlayingSong);
            List<Song> tmp = Queue.ToList();
            for(int i = tmp.Count -1; i> 0; --i)
            {
                int j = rand.Next(i + 1);
                (tmp[i], tmp[j]) = (tmp[j], tmp[i]);
            }
            if (includeCurrentPlayingSong)
            {
                CurrentPlayingSong = tmp.First();
                tmp.RemoveAt(0);
            }
            Queue.Clear();
            foreach(Song s in tmp)
            {
                Queue.Enqueue(s);
            }
        }
        public List<Song> GetSongs()
        {
            Debug.WriteLine(string.Join(", ", Queue));
            return Queue.ToList();
        }
        public void Add(Song s)
        {
            if(CurrentPlayingSong == null)
            {
                CurrentPlayingSong = s;
                return;
            }
            if(!Queue.Contains(s)) Queue.Enqueue(s);
        }
        public Song? Next(bool repeat)
        {
            if (CurrentPlayingSong != null && Queue.Count > 0)
            {
                if(!repeat)
                {
                    Song s = Queue.Dequeue();
                    PlayedSongs.Push(CurrentPlayingSong);
                    CurrentPlayingSong = s;
                }
            }
            return CurrentPlayingSong;
        }
        public Song? Previous()
        {
            if (CurrentPlayingSong != null && PlayedSongs.Count > 0)
            {
                Song s = PlayedSongs.Pop();
                List<Song> tmp = new List<Song>(Queue);
                tmp.Insert(0, CurrentPlayingSong);
                Queue = new Queue<Song>(tmp);
                CurrentPlayingSong = s;
            }
            return CurrentPlayingSong;
        }
        public Queue<Song> ChangePosition(Queue<Song> queue, Song item, int newIndex)
        {
            List<Song> list = new List<Song>(queue);

            int oldIndex = list.IndexOf(item);
            if (oldIndex == -1 || newIndex < 0 || newIndex >= list.Count || queue.Count == 0)
            {
                return queue;
            }

            // Remove and reinsert at new position
            list.RemoveAt(oldIndex);
            list.Insert(newIndex, item);

            return new Queue<Song>(list);
        }
    }
}
