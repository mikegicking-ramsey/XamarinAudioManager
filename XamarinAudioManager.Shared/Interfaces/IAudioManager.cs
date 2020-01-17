using System;
namespace XamarinAudioManager.Interfaces
{
    public interface IAudioManager
    {
        IBluetoothControls BluetoothControls { get; set; }
        IAudioPlayer AudioPlayer { get; set; }
        //TODO: Add separate notification controls?
    }
}
