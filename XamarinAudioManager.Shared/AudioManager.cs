using System;
using XamarinAudioManager.Interfaces;

namespace XamarinAudioManager.Models
{
    public class AudioManager
    {
        private static Lazy<IAudioPlayer> implementation = new Lazy<IAudioPlayer>(() => InstantiateAudioPlayer(), System.Threading.LazyThreadSafetyMode.PublicationOnly);
        public static IAudioPlayer Current
        {
            get
            {
                var ret = implementation.Value;
                if (ret == null)
                {
                    throw new NotImplementedException();
                }
                return ret;
            }
        }

        public AudioManager()
        {
        }

        internal static IAudioPlayer InstantiateAudioPlayer()
        {
#if ANDROID
            return new XamarinAudioManager.Android.AudioPlayer();
#elif COCOA
            return new XamarinAudioManager.iOS.AppleAudioPlayer();
#else
            return null;
#endif
        }
    }
}
