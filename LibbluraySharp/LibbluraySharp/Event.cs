using LibbluraySharp.Native;

namespace LibbluraySharp
{
    /// <summary>
    /// Represents a libbluray event
    /// </summary>
    public class Event
    {
        public uint EventType { get; }
        public uint Param { get; }

        internal Event(Native.BdEvent nativeEvent)
        {
            EventType = nativeEvent.Event;
            Param = nativeEvent.Param;
        }

        internal Event()
        {
            EventType = EventTypes.None;
            Param = 0;
        }

        public override string ToString()
        {
            return $"EventType: {GetEventTypeName(EventType)}, Param: {Param}";
        }

        private static string GetEventTypeName(uint eventType)
        {
            return eventType switch
            {
                EventTypes.None => "None",
                EventTypes.Error => "Error",
                EventTypes.ReadError => "ReadError",
                EventTypes.Encrypted => "Encrypted",
                EventTypes.Angle => "Angle",
                EventTypes.Title => "Title",
                EventTypes.Playlist => "Playlist",
                EventTypes.PlayItem => "PlayItem",
                EventTypes.Chapter => "Chapter",
                EventTypes.PlayMark => "PlayMark",
                EventTypes.EndOfTitle => "EndOfTitle",
                EventTypes.AudioStream => "AudioStream",
                EventTypes.IgStream => "IgStream",
                EventTypes.PgTextstStream => "PgTextstStream",
                EventTypes.PipPgTextstStream => "PipPgTextstStream",
                EventTypes.SecondaryAudioStream => "SecondaryAudioStream",
                EventTypes.SecondaryVideoStream => "SecondaryVideoStream",
                EventTypes.PgTextst => "PgTextst",
                EventTypes.PipPgTextst => "PipPgTextst",
                EventTypes.SecondaryAudio => "SecondaryAudio",
                EventTypes.SecondaryVideo => "SecondaryVideo",
                EventTypes.SecondaryVideoSize => "SecondaryVideoSize",
                EventTypes.PlaylistStop => "PlaylistStop",
                EventTypes.Discontinuity => "Discontinuity",
                EventTypes.Seek => "Seek",
                EventTypes.Still => "Still",
                EventTypes.StillTime => "StillTime",
                EventTypes.SoundEffect => "SoundEffect",
                EventTypes.Idle => "Idle",
                EventTypes.Popup => "Popup",
                EventTypes.Menu => "Menu",
                EventTypes.StereoscopicStatus => "StereoscopicStatus",
                EventTypes.KeyInterestTable => "KeyInterestTable",
                EventTypes.UoMaskChanged => "UoMaskChanged",
                _ => $"Unknown({eventType})"
            };
        }
    }
}