using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaloniaFirstApp.Support;
using NAudio.Wave;

namespace AvaloniaFirstApp.Models
{
    static class AudioPlayer
    {
        private static WaveOutEvent waveOutEvent;
        private static Mp3FileReader mp3FileReader;
        private static Random r = new Random();
        public static bool isInitiated = false;
        /// <summary>
        /// Unused method for playing audio
        /// </summary>
        /// <param name="path"></param>
        public static void Play(string path)
        {
            Cleanup();
            mp3FileReader = new Mp3FileReader(path);
            waveOutEvent = new WaveOutEvent();
            waveOutEvent.Init(mp3FileReader);
            waveOutEvent.Play();
            isInitiated = true;
        }
        public static void Play()
        {
            Cleanup();
            mp3FileReader = new Mp3FileReader("../../../../AvaloniaFirstApp/Assets/Sounds/" + r.Next(3).ToString() + ".mp3");
            waveOutEvent = new WaveOutEvent();
            waveOutEvent.Init(mp3FileReader);
            waveOutEvent.Play();
            isInitiated = true;
        }
        public static void Pause()
        {
            if (waveOutEvent == null) return;
            waveOutEvent.Pause();
        }
        public static void Resume()
        {
            if (waveOutEvent == null) return;
            if(waveOutEvent.PlaybackState == PlaybackState.Paused)
            {
                waveOutEvent.Play();
            }
        }
        public static void JumpTo(double time)
        {
            if (mp3FileReader == null) return;
            TimeSpan jumpTime = TimeSpan.FromSeconds(Utils.MapToRange(time, 0, 100, 0, mp3FileReader.TotalTime.TotalSeconds));
            //only do the jump when the change is bigger than 1 second
            if (Math.Abs(jumpTime.TotalSeconds - mp3FileReader.CurrentTime.TotalSeconds) > 1.0) mp3FileReader.CurrentTime = jumpTime;
        }
        public static TimeSpan GetTotalTime()
        {
            if (mp3FileReader == null) return new TimeSpan();
            return mp3FileReader.TotalTime;
        }
        public static TimeSpan GetTimeLeft()
        {
            if (mp3FileReader == null) return new TimeSpan();
            return mp3FileReader.TotalTime - mp3FileReader.CurrentTime;
        }
        public static TimeSpan GetTimeElapsed()
        {
            if (mp3FileReader == null) return new TimeSpan();
            return mp3FileReader.CurrentTime;
        }
        public static void ChangeVolume(float volume)
        {
            if (waveOutEvent == null) return;
            waveOutEvent.Volume = volume;
        }
        private static void Cleanup()
        {
            if (waveOutEvent != null)
            {
                waveOutEvent.Dispose();
                waveOutEvent = null;
            }

            if (mp3FileReader != null)
            {
                mp3FileReader.Dispose();
                mp3FileReader = null;
            }
            isInitiated = false;
        }
        public static bool IsInitiated()
        {
            return isInitiated;
        }
    }
}
