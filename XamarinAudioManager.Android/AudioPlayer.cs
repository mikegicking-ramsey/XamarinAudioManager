using System;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Media;
using XamarinAudioManager.Interfaces;
using XamarinAudioManager.Models;

namespace XamarinAudioManager
{
    public class AudioPlayer : IAudioPlayer
    {
        private static Lazy<IAudioPlayer> instance = new Lazy<IAudioPlayer>(() => new AudioPlayer());
        public static IAudioPlayer SharedInstance => instance.Value;

        protected MediaPlayer mediaPlayer;
        public Android.Media.AudioManager audioManager;
        public PlayerConfig Configuration { get; set; }

        #region Bluetooth Controls

        private XamarinAudioManagerBroadcastReceiver receiver;

        private IntentFilter intentFilter;
        private string[] intents = new string[]
        {
            Intent.ActionMediaButton,
            "this.is.a.TEST",
            BluetoothAdapter.ActionConnectionStateChanged,
            "music-controls-media-button",
            BluetoothHeadset.ActionVendorSpecificHeadsetEvent,
        };

        #endregion

        private AudioPlayer()
        {
        }


        public void Dispose()
        {
            mediaPlayer.Release();
        }

        public void Initialize()
        {
            audioManager = (Android.Media.AudioManager)Application.Context.GetSystemService(Context.AudioService);
            audioManager.RegisterMediaButtonEventReceiver(PendingIntent.GetBroadcast(Application.Context, 0, new Intent("music-controls-media-button"), PendingIntentFlags.UpdateCurrent));

            receiver = new XamarinAudioManagerBroadcastReceiver();
            intentFilter = new IntentFilter();
            foreach (var intent in intents)
            {
                intentFilter.AddAction(intent);
            }

            Application.Context.RegisterReceiver(receiver, intentFilter);
            throw new NotImplementedException();
        }

        #region Playback methods

        public void Play()
        {
            if(mediaPlayer != null)
            {
                mediaPlayer.Start();
            }
        }

        public void Play(string url)
        {
            if (mediaPlayer == null)
            {
                mediaPlayer = new MediaPlayer();
            }
            else
            {
                mediaPlayer.Reset();
            }

            mediaPlayer.SetDataSource(url);
            mediaPlayer.Prepare();
            mediaPlayer.Start();
        }

        public void Play(IMediaItem mediaItem)
        {
            throw new NotImplementedException();
            //TODO: Implement playback from a MediaItem object
            //if(mediaPlayer == null)
            //{
            //    mediaPlayer = new MediaPlayer();
            //}
            //else
            //{
            //    mediaPlayer.Reset();
            //}

            //mediaPlayer.SetDataSource();
            //mediaPlayer.Prepare();
            //mediaPlayer.Start();
        }

        public void Pause()
        {
            if (mediaPlayer != null)
            {
                mediaPlayer.Pause();
            }
        }

        public void PlayPause()
        {
            if (mediaPlayer != null)
            {
                if (mediaPlayer.IsPlaying)
                {
                    mediaPlayer.Pause();
                }
                else
                {
                    mediaPlayer.Start();
                }
            }
        }

        public void FastForward()
        {
            if (mediaPlayer != null)
            {
                StepForward();
            }
        }

        public void SeekForward()
        {
            if (mediaPlayer != null)
            {
                StepForward();
            }
        }

        public void Next()
        {
            if (mediaPlayer != null)
            {
                // TODO[Issue:3]: Change this from jump forward to progress through queue when queue implementation is added
                StepForward();

                //if (Configuration.SeekInsteadOfSkip)
                //{
                //    StepForward();
                //}
                //else
                //{
                //    PlayNext();
                //}
            }
        }

        public void Rewind()
        {
            if (mediaPlayer != null)
            {
                StepBackward();
            }
        }

        public void SeekBackward()
        {
            if (mediaPlayer != null)
            {
                StepBackward();
            }
        }

        public void Previous()
        {
            if (mediaPlayer != null)
            {
                // TODO[Issue:3]: Change this from jump backward to regress through queue when queue implementation is added
                StepBackward();

                //if (Configuration.SeekInsteadOfSkip)
                //{
                //    StepBackward();
                //}
                //else
                //{
                //    PlayPrevious();
                //}
            }
        }

        public void SeekTo(int location)
        {
            if (mediaPlayer != null)
            {
                mediaPlayer.SeekTo(1000 * location);
            }
        }

        #endregion

        #region Playback Helpers

        private void StepForward()
        {
            var secondsRemaining = 1000 * (mediaPlayer.Duration - mediaPlayer.CurrentPosition);
            var seekAmount = Math.Min(1000 * Configuration.StepSize, secondsRemaining);
            mediaPlayer.SeekTo(mediaPlayer.CurrentPosition + seekAmount);
        }

        private void StepBackward()
        {
            var seekAmount = Math.Min(1000 * Configuration.StepSize, 1000 * mediaPlayer.CurrentPosition);
            mediaPlayer.SeekTo(mediaPlayer.CurrentPosition - seekAmount);
        }

        #endregion

        #region Lifecycle Methods

        public EventHandler PositionChanged { get; set ; }
        public EventHandler BeforePlaying { get; set; }
        public EventHandler AfterPlaying { get; set; }
        public EventHandler OnBufferChanged { get; set; }
        public EventHandler MediaItemChanged { get; set; }
        public EventHandler MediaItemFinished { get; set; }
        public EventHandler AudioPlaybackStateChanged { get; set; }

        #endregion
    }
}
