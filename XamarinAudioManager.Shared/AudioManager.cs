using System;
using XamarinAudioManager.Interfaces;

namespace XamarinAudioManager.Models
{
    public class AudioManager : IAudioManager
    {
        public IBluetoothManager BluetoothManager { get; set; }
        public IAudioPlayer AudioPlayer { get; set; }

        public AudioManager()
        {

        }

        private static Lazy<IAudioManager> implementation = new Lazy<IAudioManager>(() => InstantiateAudioPlayer(), System.Threading.LazyThreadSafetyMode.PublicationOnly);
        public static IAudioManager Current
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

        internal static IAudioManager InstantiateAudioPlayer()
        {
            return null;
        }
    }
}
