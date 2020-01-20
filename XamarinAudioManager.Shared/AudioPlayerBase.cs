using System;
using System.Timers;
using XamarinAudioManager.Interfaces;

namespace XamarinAudioManager
{
    public abstract class AudioPlayerBase
    {
        public event EventHandler PositionChanged;
        public event EventHandler BufferedChanged;
        public event EventHandler MediaItemChanged;
        public event EventHandler MediaItemFinished;
        public event EventHandler AudioPlaybackStateChanged;

        public abstract TimeSpan Position { get; }
        public abstract TimeSpan Duration { get; }

        public static double TimerInterval { get; set; } = 1000;
        public Timer PlaybackTimer { get; private set; } = new Timer(TimerInterval);

        public AudioPlayerBase()
        {
            PlaybackTimer.AutoReset = true;
            PlaybackTimer.Elapsed += OnPlaybackTimer_Elapsed;
            PlaybackTimer.Start();
        }

        public virtual void Dispose()
        {
            PlaybackTimer.Elapsed -= OnPlaybackTimer_Elapsed;
            PlaybackTimer.Dispose();
        }

        private void OnPlaybackTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            PreviousPosition = Position;
        }

        private TimeSpan buffered;
        public TimeSpan Buffered
        {
            get => buffered;
            internal set
            {
                buffered = value;
                BufferedChanged?.Invoke(this, new EventArgs()); //TODO: update event args
            }
        }

        private TimeSpan previousPosition = new TimeSpan();
        protected TimeSpan PreviousPosition
        {
            get
            {
                return previousPosition;
            }
            set
            {
                previousPosition = value;
                PositionChanged?.Invoke(this, new EventArgs()); //TODO: Implement new event args
            }
        }

        private IMediaItem currentItem;
        public IMediaItem CurrentItem
        {
            get
            {
                return currentItem;
            }
            set
            {
                currentItem = value;
                MediaItemChanged?.Invoke(this, new EventArgs()); //TODO: Implement new event args
            }
        }

        internal void OnMediaItemFinished(object sender, EventArgs e)
        {
            MediaItemFinished?.Invoke(sender, e);
        }
    }
}
