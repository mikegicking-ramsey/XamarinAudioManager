using System;
namespace XamarinAudioManager.Interfaces
{
    public interface IMediaItem
    {
        string MediaUri { get; set; }
        string Id { get; set; }
        string Title { get; set; }
        string Author { get; set; }
        string Album { get; set; }
    }
}
