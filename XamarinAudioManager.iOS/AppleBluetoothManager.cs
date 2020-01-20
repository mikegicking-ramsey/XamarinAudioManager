using System;
using AVFoundation;
using MediaPlayer;
using XamarinAudioManager.Enums;
using XamarinAudioManager.Interfaces;
using XamarinAudioManager.Models;

namespace XamarinAudioManager
{
    public class AppleBluetoothManager : IBluetoothManager
    {
        public IBluetoothControls BluetoothControls { get; set; }
        private IAudioPlayer audioPlayer;
        private bool isInitialized = false;

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
            commandCenter.ChangePlaybackPositionCommand.AddTarget(ChangePlaybackPositionCommandHandler);

            commandCenter.SeekForwardCommand.Enabled = true;
            commandCenter.SeekForwardCommand.AddTarget(FastForwardCommandHandler);

            commandCenter.SkipForwardCommand.Enabled = true;
            commandCenter.SkipForwardCommand.AddTarget(FastForwardCommandHandler);

            commandCenter.SkipBackwardCommand.Enabled = true;
            commandCenter.SkipBackwardCommand.AddTarget(RewindCommandHandler);

            commandCenter.SeekBackwardCommand.Enabled = true;
            commandCenter.SeekBackwardCommand.AddTarget(RewindCommandHandler);

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
            Play();
            return MPRemoteCommandHandlerStatus.Success;
        }

        private MPRemoteCommandHandlerStatus PauseCommandHandler(MPRemoteCommandEvent e)
        {
            Pause();
            return MPRemoteCommandHandlerStatus.Success;
        }

        private MPRemoteCommandHandlerStatus ChangePlaybackPositionCommandHandler(MPRemoteCommandEvent e)
        {
            var playbackPositionEvent = (MPChangePlaybackPositionCommandEvent)e; 
            ChangePosition((int)playbackPositionEvent.PositionTime);
            return MPRemoteCommandHandlerStatus.Success;
        }

        private MPRemoteCommandHandlerStatus FastForwardCommandHandler(MPRemoteCommandEvent e)
        {
            FastFoward();
            return MPRemoteCommandHandlerStatus.Success;
        }

        private MPRemoteCommandHandlerStatus RewindCommandHandler(MPRemoteCommandEvent e)
        {
            Rewind();
            return MPRemoteCommandHandlerStatus.Success;
        }

        public void Play()
        {
            if (BluetoothControls.Play != null)
            {
                BluetoothControls.Play.Invoke();
            }
            else
            {
                audioPlayer.Play();
                audioPlayer.SetRate();
            }
        }

        public void Pause()
        {
            if (BluetoothControls.Pause != null)
            {
                BluetoothControls.Pause.Invoke();
            }
            else
            {
                audioPlayer.Pause();
            }
        }

        public void Rewind()
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
        }

        public void FastFoward()
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
        }

        public void ChangePosition(int newTime)
        {
            audioPlayer.SeekTo(newTime);
            audioPlayer.SetRate();
        }
    }
}
