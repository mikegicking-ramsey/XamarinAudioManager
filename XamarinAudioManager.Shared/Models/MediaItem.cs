using System;
namespace XamarinAudioManager.Models
{
    public class MediaItem : IObservable<MediaItem>
    {
        public MediaItem()
        {
        }

        public IDisposable Subscribe(IObserver<MediaItem> observer)
        {
            throw new NotImplementedException();
        }
    }
}
