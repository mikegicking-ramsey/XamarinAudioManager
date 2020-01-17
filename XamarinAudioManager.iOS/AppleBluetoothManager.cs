using System;
using AVFoundation;
using MediaPlayer;
using XamarinAudioManager.Enums;
using XamarinAudioManager.Interfaces;

namespace XamarinAudioManager
{
    public class AppleBluetoothManager
    {
        private IAudioPlayer audioPlayer;

        private bool isInitialized = false;

        public AppleBluetoothManager()
        {
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
            commandCenter.PauseCommand.AddTarget(Pause);

            commandCenter.PlayCommand.Enabled = true;
            commandCenter.PlayCommand.AddTarget(Play);

            isInitialized = true;
        }

        private void AudioInterruption(object sender, AVAudioSessionInterruptionEventArgs e)
        {
            if (e.InterruptionType == AVAudioSessionInterruptionType.Ended && audioPlayer.LastAudioAction == AudioAction.Play)
            {
                audioPlayer.Play();
            }
        }

        private MPRemoteCommandHandlerStatus Play(MPRemoteCommandEvent e)
        {
            AppleAudioPlayer.SharedInstance.Play();
            AppleAudioPlayer.SharedInstance.SetRate();
            return MPRemoteCommandHandlerStatus.Success;
        }

        private MPRemoteCommandHandlerStatus Pause(MPRemoteCommandEvent e)
        {
            AppleAudioPlayer.SharedInstance.Pause();
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
            audioPlayer.Pause();
            audioPlayer.FastForward();
            audioPlayer.Play();
            audioPlayer.SetRate();
            return MPRemoteCommandHandlerStatus.Success;
        }

        private MPRemoteCommandHandlerStatus Rewind(MPRemoteCommandEvent e)
        {
            audioPlayer.Pause();
            audioPlayer.Rewind();
            audioPlayer.Play();
            audioPlayer.SetRate();
            return MPRemoteCommandHandlerStatus.Success;
        }
    }
}
