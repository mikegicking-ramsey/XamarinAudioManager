using System;
namespace XamarinAudioManager.Interfaces
{
    public interface IAudioManager
    {
        IBluetoothManager BluetoothManager { get; set; }
        IAudioPlayer AudioPlayer { get; set; }
        //TODO: Add separate notification controls?
    }
}
