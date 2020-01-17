using System;
namespace XamarinAudioManager.Interfaces
{
    public interface IAudioPlayer
    {
        void Initialize();

        void Play();

        void Play(string url);

        void Play(IMediaItem mediaItem);

        void Pause();

        void PlayPause();

        void FastForward();

        void SeekForward();

        void Next();

        void Rewind();

        void SeekBackward();

        void Previous();

        void SeekTo(int location);

        #region Lifecycle Hooks

        EventHandler PositionChanged { get; set; }

        EventHandler BeforePlaying { get; set; }

        EventHandler AfterPlaying { get; set; }

        EventHandler OnBufferChanged { get; set; }

        EventHandler MediaItemChanged { get; set; }

        EventHandler MediaItemFinished { get; set; }

        EventHandler AudioPlaybackStateChanged { get; set; }

        #endregion
    }
}
