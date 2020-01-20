using System;
using System.Linq;
using AVFoundation;
using CoreMedia;
using Foundation;
using XamarinAudioManager.Enums;
using XamarinAudioManager.Interfaces;
using XamarinAudioManager.Models;

namespace XamarinAudioManager
{
    public class AppleAudioPlayer : AudioPlayerBase, IAudioPlayer
    {
        private static Lazy<IAudioPlayer> _audioPlayer = new Lazy<IAudioPlayer>(() => new AppleAudioPlayer());
        public static IAudioPlayer SharedInstance => _audioPlayer.Value;

        public PlayerConfig Configuration { get; set; }
        public IMediaControls MediaControls { get; set; }

        public int TimeScale = 60;
        public double AudioIncrement { get; set; } = 15;
        public float PlaybackSpeed { get; set; } = 1.0f;
        public AudioAction LastAudioAction { get; set; } = AudioAction.None;

        private NSObject didFinishPlayingObserver;
        private IDisposable loadedTimeRangesToken;

        private AVPlayer audioPlayer;
        public AppleAudioPlayer()
        {
            MediaControls = new MediaControls();
            Configuration = new PlayerConfig();
        }

        public event EventHandler BeforePlaying;
        public event EventHandler AfterPlaying;

        public override TimeSpan Position => audioPlayer == null ? TimeSpan.Zero : TimeSpan.FromSeconds(audioPlayer.CurrentTime.Seconds);
        public override TimeSpan Duration => audioPlayer == null ? TimeSpan.Zero : TimeSpan.FromSeconds(audioPlayer.CurrentItem.Duration.Seconds);

        public void Initialize()
        {
            var options = NSKeyValueObservingOptions.Initial | NSKeyValueObservingOptions.New;
            loadedTimeRangesToken = audioPlayer.AddObserver("currentItem.loadedTimeRanges", options, LoadedTimeRangesChanged);
            didFinishPlayingObserver = NSNotificationCenter.DefaultCenter.AddObserver(AVPlayerItem.DidPlayToEndTimeNotification, DidFinishPlaying);
        }

        protected virtual void LoadedTimeRangesChanged(NSObservedChange obj)
        {
            var buffered = TimeSpan.Zero;
            if (audioPlayer?.CurrentItem != null && audioPlayer.CurrentItem.LoadedTimeRanges.Any())
            {
                buffered =
                    TimeSpan.FromSeconds(
                        audioPlayer.CurrentItem.LoadedTimeRanges.Select(
                            tr => tr.CMTimeRangeValue.Start.Seconds + tr.CMTimeRangeValue.Duration.Seconds).Max());

                Buffered = buffered;
            }
        }

        protected virtual void DidFinishPlaying(NSNotification obj)
        {
            OnMediaItemFinished(this, new EventArgs());
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
                MediaControls.Pause.Invoke();
            }
            else
            {
                audioPlayer.Pause();
            }

            LastAudioAction = AudioAction.Pause;
        }

        public void Play()
        {
            BeforePlaying?.Invoke(this, new EventArgs());

            if (MediaControls.Play != null)
            {
                MediaControls.Play.Invoke();
            }
            else
            {
                audioPlayer.Play();
            }

            LastAudioAction = AudioAction.Play;

            AfterPlaying?.Invoke(this, new EventArgs());
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
                MediaControls.PlayPause.Invoke();
            }
            else
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
                SeekBackward();
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
            Configuration.PlaybackSpeed = rate;
            SetRate();
        }

        public void SetRate()
        {
            audioPlayer.SetRate(Configuration.PlaybackSpeed, CMTime.Zero, CMTime.Zero);
        }
    }
}
