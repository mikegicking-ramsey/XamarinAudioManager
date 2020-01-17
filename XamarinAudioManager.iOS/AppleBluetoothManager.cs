using System;
using AVFoundation;
using MediaPlayer;
using XamarinAudioManager.Enums;
using XamarinAudioManager.Interfaces;
using XamarinAudioManager.Models;

namespace XamarinAudioManager
{
    public class AppleBluetoothManager
    {
        private IAudioPlayer audioPlayer;

        private bool isInitialized = false;

        public IBluetoothControls BluetoothControls { get; set; }

        public AppleBluetoothManager()
        {
            BluetoothControls = new BluetoothControls();
        }

        public void Intialize()
        {
            if (isInitialized)
            {
                throw new Exception("Bluetooth Manager has already been initialized, aborting re-initializations");
            }

            audioPlayer = AppleAudioPlayer.SharedInstance;

            AVAudioSession.Notifications.ObserveInterruption(AudioInterruption);

            MPRemoteCommandCenter commandCenter = MPRemoteCommandCenter.Shared;
            commandCenter.ChangePlaybackPositionCommand.Enabled = true;
            commandCenter.ChangePlaybackPositionCommand.AddTarget(ChangePlaybackPosition);

            commandCenter.SeekForwardCommand.Enabled = true;
            commandCenter.SeekForwardCommand.AddTarget(FastForward);

            commandCenter.SkipForwardCommand.Enabled = true;
            commandCenter.SkipForwardCommand.AddTarget(FastForward);

            commandCenter.SkipBackwardCommand.Enabled = true;
            commandCenter.SkipBackwardCommand.AddTarget(Rewind);

            commandCenter.SeekBackwardCommand.Enabled = true;
            commandCenter.SeekBackwardCommand.AddTarget(Rewind);

            commandCenter.PauseCommand.Enabled = true;
            commandCenter.PauseCommand.AddTarget(PauseCommandHandler);

            commandCenter.PlayCommand.Enabled = true;
            commandCenter.PlayCommand.AddTarget(PlayCommandHandler);

            isInitialized = true;
        }

        private void AudioInterruption(object sender, AVAudioSessionInterruptionEventArgs e)
        {
            if (e.InterruptionType == AVAudioSessionInterruptionType.Ended && audioPlayer.LastAudioAction == AudioAction.Play)
            {
                audioPlayer.Play();
            }
        }

        private MPRemoteCommandHandlerStatus PlayCommandHandler(MPRemoteCommandEvent e)
        {

            if (BluetoothControls.Play != null)
            {
                BluetoothControls.Play.Invoke();
            }
            else
            {
                AppleAudioPlayer.SharedInstance.Play();
                AppleAudioPlayer.SharedInstance.SetRate();
            }
            
            return MPRemoteCommandHandlerStatus.Success;
        }

        private MPRemoteCommandHandlerStatus PauseCommandHandler(MPRemoteCommandEvent e)
        {
            if (BluetoothControls.Pause != null)
            {
                BluetoothControls.Pause.Invoke();
            }
            else
            {
                AppleAudioPlayer.SharedInstance.Pause();
            }
            
            return MPRemoteCommandHandlerStatus.Success;
        }

        private MPRemoteCommandHandlerStatus ChangePlaybackPosition(MPRemoteCommandEvent e)
        {
            var playbackPositionEvent = (MPChangePlaybackPositionCommandEvent)e;
            AppleAudioPlayer.SharedInstance.SeekTo((int)playbackPositionEvent.PositionTime);
            AppleAudioPlayer.SharedInstance.SetRate();
            return MPRemoteCommandHandlerStatus.Success;
        }

        private MPRemoteCommandHandlerStatus FastForward(MPRemoteCommandEvent e)
        {

            if (BluetoothControls.SeekForward != null || BluetoothControls.StepForward != null)
            {
                BluetoothControls.SeekForward?.Invoke();
                BluetoothControls.StepForward.Invoke();
            }
            else
            {
                audioPlayer.Pause();
                audioPlayer.FastForward();
                audioPlayer.Play();
                audioPlayer.SetRate();
            }
            
            return MPRemoteCommandHandlerStatus.Success;
        }

        private MPRemoteCommandHandlerStatus Rewind(MPRemoteCommandEvent e)
        {
            if (BluetoothControls.SeekBackward != null || BluetoothControls.StepBackward != null)
            {
                BluetoothControls.SeekBackward?.Invoke();
                BluetoothControls.StepBackward.Invoke();
            }
            else
            {
                audioPlayer.Pause();
                audioPlayer.Rewind();
                audioPlayer.Play();
                audioPlayer.SetRate();
            }

            return MPRemoteCommandHandlerStatus.Success;
        }
    }
}
