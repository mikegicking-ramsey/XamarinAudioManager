using System;
using XamarinAudioManager;

namespace MediaManager.Playback
{
    public class MediaControls : IMediaControls
    {
        private IAudioPlayer _audioManager = AudioManager.Current;

        public MediaControls()
        {
        }

        #region Command Overrides

        public Action SkipBackwardImpl { get; set; }

        public Action SkipForwardImpl { get; set; }

        public Action SeekBackwardImpl { get; set; }

        public Action SeekForwardImpl { get; set; }

        public Action StopImpl { get; set; }

        public Action PauseImpl { get; set; }

        public Action PlayImpl { get; set; }

        public Action PlayPauseImpl { get; set; }

        public Action PreviousImpl { get; set; }

        public Action NextImpl { get; set; }

        public Action ShuffleImpl { get; set; }

        public Action RepeatImpl { get; set; }

        public Func<object, bool> MediaButtonEventImpl { get; set; }

        #endregion

        #region Baked-In Overrides

        public bool SeekInsteadOfSkip
        {
            get
            {
                return SeekInsteadOfSkip;
            }
            set
            {
                if (value)
                {
                    PreviousImpl = new Action(() =>
                    {
                        _audioManager.SeekForward();
                    });
                    NextImpl = new Action(() =>
                    {
                        _audioManager.SeekBackward();
                    });
                }
                else
                {
                    PreviousImpl = null;
                    NextImpl = null;
                }
            }
        }

        #endregion
    }
}
