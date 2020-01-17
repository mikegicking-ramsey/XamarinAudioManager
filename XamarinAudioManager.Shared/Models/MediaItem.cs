using System;
using XamarinAudioManager.Interfaces;

namespace XamarinAudioManager.Models
{
    public class MediaItem : IMediaItem
    {
        public string Id { get; set; }
        public string MediaUri { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Album { get; set; }
    }
}
