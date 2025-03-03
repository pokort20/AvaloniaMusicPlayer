using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace AvaloniaFirstApp.Models
{
    static class AudioPlayer
    {
        private static WaveOutEvent waveOutEvent;
        private static Mp3FileReader mp3FileReader;
        private static Random r = new Random();
        public static void Play()
        {
            Cleanup();
            mp3FileReader = new Mp3FileReader("../../../../AvaloniaFirstApp/Assets/Sounds/" + r.Next(3).ToString() + ".mp3");
            waveOutEvent = new WaveOutEvent();
            waveOutEvent.Init(mp3FileReader);
            waveOutEvent.Play();
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
            mp3FileReader.CurrentTime = TimeSpan.FromSeconds(time);
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
        }

    }
}
