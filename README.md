# Audio Manager
The Audio Manager is an overridable, extensible, shared audio solution for Android and iOS to play local audio or streamed audio files(e.g. MP3s) and HLS streams.

## Properties Overview
### Audio Player
- Contains methods for Audio Playback
- Generates Notification / Lockscreen Controls
- Auto-seeks to a position in an audio stream
- Allows pre-load of media items into a buffer before playing
- Each audio player method is overridable with MediaControls class
- Has life cycle event hooks for loading items and audio playback

### Bluetooth Manager
- Houses commands to receive remote commands from lock screens and bluetooth events
- Each bluetooth method is overridable with the BluetoothControls class

### Media Item
```
string Id { get; set; }
string MediaUrl { get; set; }
string Title { get; set; }
string Artist { get; set; }
string Album { get; set; }
string AlbumArtUrl { get; set; }
double ProgressInSeconds { get; set; }
```

## Usage
Initialize the Components

```
AudioManger.AudioPlayer.Initialize();
AudioManager.BluetoothManager.Initialize();
```

Play a Media Item

```
var mediaItem = new MediaItem() {
  MediaUrl = "https://yoursite.com/path/to/your.mp3"
}

AudioManager.AudioPlayer.Play(mediaItem);
```

Available LifeCycle Events

```
BeforeNewPlay
AfterNewPlay
PositionChanged
BufferChanged
MediaItemChanged
MediaItemFinished
```

