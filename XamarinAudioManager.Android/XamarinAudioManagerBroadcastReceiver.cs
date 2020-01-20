using System;
using Android.Bluetooth;
using Android.Content;
using Android.Views;

namespace XamarinAudioManager
{
    [BroadcastReceiver(Enabled = true, Exported = true)]
    public class XamarinAudioManagerBroadcastReceiver : BroadcastReceiver
    {
        public XamarinAudioManagerBroadcastReceiver()
        {
        }

        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action == "music-controls-media-button")
            {
                var buttonPressed = (KeyEvent)intent.GetParcelableExtra(Intent.ExtraKeyEvent);
                if (buttonPressed.Action == KeyEventActions.Up)
                {
                    switch (buttonPressed.KeyCode)
                    {
                        case Keycode.MediaPause:
                            AudioPlayer.SharedInstance.Pause();
                            break;
                        case Keycode.MediaPlay:
                            AudioPlayer.SharedInstance.Play();
                            break;
                        case Keycode.MediaPlayPause:
                            AudioPlayer.SharedInstance.PlayPause();
                            break;
                        case Keycode.MediaNext:
                            AudioPlayer.SharedInstance.Next();
                            break;
                        case Keycode.MediaFastForward:
                            AudioPlayer.SharedInstance.FastForward();
                            break;
                        case Keycode.MediaSkipForward:
                        case Keycode.MediaStepForward:
                            AudioPlayer.SharedInstance.SeekForward();
                            break;
                        case Keycode.MediaPrevious:
                            AudioPlayer.SharedInstance.Previous();
                            break;
                        case Keycode.MediaRewind:
                            AudioPlayer.SharedInstance.Rewind();
                            break;
                        case Keycode.MediaSkipBackward:
                        case Keycode.MediaStepBackward:
                            AudioPlayer.SharedInstance.SeekBackward();
                            break;

                    }
                }
            }
            else if(intent.Action == BluetoothDevice.ActionAclDisconnected)
            {
                // Pause playback if headset disconnected
                AudioPlayer.SharedInstance.Pause();
            }
        }
    }
}
