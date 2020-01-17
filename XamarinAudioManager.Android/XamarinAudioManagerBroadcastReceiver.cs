using System;
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
                        case Keycode.MediaPlay:
                        case Keycode.MediaPlayPause:
                            AudioPlayer.SharedInstance.PlayPause();
                            break;
                        case Keycode.MediaNext:
                        case Keycode.MediaFastForward:
                        case Keycode.MediaSkipForward:
                        case Keycode.MediaStepForward:
                            AudioPlayer.SharedInstance.SeekForward();
                            break;
                        case Keycode.MediaPrevious:
                        case Keycode.MediaRecord:
                        case Keycode.MediaSkipBackward:
                        case Keycode.MediaStepBackward:
                            AudioPlayer.SharedInstance.SeekBackward();
                            break;

                    }
                }
            }
            else
            {
                Console.WriteLine("OnReceive hit!");
            }
        }
    }
}
