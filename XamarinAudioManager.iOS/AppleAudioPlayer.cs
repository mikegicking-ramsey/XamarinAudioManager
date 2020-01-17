using System;
using AVFoundation;
using CoreMedia;
using Foundation;
using XamarinAudioManager.Interfaces;
using XamarinAudioManager.Models;

namespace XamarinAudioManager
{
    public class AppleAudioPlayer : IAudioPlayer
    {
        

        public IMediaControls MediaControls { get; set; }

        public int TimeScale = 60;
        public double AudioIncrement { get; set; } = 15;
        public float PlaybackSpeed { get; set; } = 1.0f;

        private AVPlayer audioPlayer;
        public AppleAudioPlayer()
        {
            MediaControls = new MediaControls();
        }

        public EventHandler PositionChanged { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public EventHandler BeforePlaying { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public EventHandler AfterPlaying { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public EventHandler OnBufferChanged { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public EventHandler MediaItemChanged { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public EventHandler MediaItemFinished { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public EventHandler AudioPlaybackStateChanged { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Initialize()
        {
            //TODO: Set up event Handlers
        }

        public void FastForward()
        {
            if (MediaControls.SeekForward != null)
            {
                MediaControls.SeekForward.Invoke();
            }
            else
            {
                SeekForward();
            }
        }

        public void Next()
        {
            if (MediaControls.Next != null)
            {
                MediaControls.Next.Invoke();
            }
            else
            {
                MediaControls.SeekForward();
            }
        }

        public void Pause()
        {
            if (MediaControls.Pause != null)
            {

            }
            else
            {
                audioPlayer.Pause();
            }    
        }

        public void Play()
        {
            if (MediaControls.Play != null)
            {
                MediaControls.Play.Invoke();
            }
            else
            {
                audioPlayer.Play();
            }
        }

        public void Play(string url)
        {
            audioPlayer = new AVPlayer(new NSUrl(url));
            Play();
        }

        public void Play(IMediaItem mediaItem)
        {
            audioPlayer = new AVPlayer(new NSUrl(mediaItem.MediaUri));
            Play();
        }

        public void PlayPause()
        {
            if (MediaControls.PlayPause != null)
            {
                if (audioPlayer.Rate >= 1.0f)
                {
                    Pause();
                }
                else
                {
                    Play();
                }
            }
        }

        public void Previous()
        {
            if (MediaControls.Previous != null)
            {
                MediaControls.Previous.Invoke();
            }
            else
            {
                SeekForward();
            }
        }

        public void Rewind()
        {
            if (MediaControls.SeekBackward != null)
            {
                MediaControls.SeekBackward.Invoke();
            }
            else
            {
                SeekBackward();
            }
            
        }

        public void SeekBackward()
        {
            if (MediaControls.SeekBackward != null)
            {
                MediaControls.SeekBackward.Invoke();
            }
            else
            {
                var targetTime = audioPlayer.CurrentTime - CMTime.FromSeconds(AudioIncrement, TimeScale);
                audioPlayer.SeekAsync(targetTime, CMTime.Zero, CMTime.Zero);
            }
        }

        public void SeekForward()
        {
            if (MediaControls.SeekForward != null)
            {
                MediaControls.SeekForward.Invoke();
            }
            else
            {
                var targetTime = audioPlayer.CurrentTime + CMTime.FromSeconds(AudioIncrement, TimeScale);
                audioPlayer.SeekAsync(targetTime, CMTime.Zero, CMTime.Zero);
            }
            
        }

        public void SeekTo(int seconds)
        {
            var targetTime = CMTime.FromSeconds(seconds, TimeScale);
            audioPlayer.SeekAsync(targetTime, CMTime.Zero, CMTime.Zero);
        }

        public void SetRate(float rate)
        {
            PlaybackSpeed = rate;
            SetRate();
        }

        public void SetRate()
        {
            audioPlayer.SetRate(PlaybackSpeed, CMTime.Zero, CMTime.Zero);
        }
    }
}
