using System;
using System.Runtime.InteropServices;

namespace LibbluraySharp.Native
{
    /// <summary>
    /// P/Invoke declarations for libbluray native functions
    /// </summary>
    public static class NativeMethods
    {
        private const string LibblurayLibraryName = "bluray";

        #region Version Information

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void bd_get_version(out int major, out int minor, out int micro);

        #endregion

        #region Initialization and Cleanup

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr bd_init();

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void bd_close(IntPtr bd);

        #endregion

        #region Disc Opening Functions

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr bd_open([MarshalAs(UnmanagedType.LPStr)] string devicePath, [MarshalAs(UnmanagedType.LPStr)] string keyfilePath);

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int bd_open_disc(IntPtr bd, [MarshalAs(UnmanagedType.LPStr)] string devicePath, [MarshalAs(UnmanagedType.LPStr)] string keyfilePath);

        #endregion

        #region Disc Information

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr bd_get_disc_info(IntPtr bd);

        #endregion

        #region Title Management

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint bd_get_titles(IntPtr bd, byte flags, uint minTitleLength);

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int bd_get_main_title(IntPtr bd);

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr bd_get_title_info(IntPtr bd, uint titleIdx, uint angle);

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void bd_free_title_info(IntPtr titleInfo);

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int bd_select_title(IntPtr bd, uint title);

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int bd_select_playlist(IntPtr bd, uint playlist);

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint bd_get_current_title(IntPtr bd);

        #endregion

        #region Reading Data

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int bd_read(IntPtr bd, IntPtr buf, int len);

        #endregion

        #region Playback Control

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern long bd_seek(IntPtr bd, ulong pos);

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern long bd_seek_time(IntPtr bd, ulong tick);

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern long bd_seek_chapter(IntPtr bd, uint chapter);

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int bd_select_angle(IntPtr bd, uint angle);

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void bd_seamless_angle_change(IntPtr bd, uint angle);

        #endregion

        #region Playback Status

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern long bd_chapter_pos(IntPtr bd, uint chapter);

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint bd_get_current_chapter(IntPtr bd);

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong bd_get_title_size(IntPtr bd);

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint bd_get_current_angle(IntPtr bd);

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong bd_tell(IntPtr bd);

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong bd_tell_time(IntPtr bd);

        #endregion

        #region Player Settings

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int bd_set_player_setting(IntPtr bd, uint idx, uint value);

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int bd_set_player_setting_str(IntPtr bd, uint idx, [MarshalAs(UnmanagedType.LPStr)] string value);

        #endregion

        #region Events

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int bd_get_event(IntPtr bd, out BdEvent @event);

        #endregion

        #region Playback with On-Disc Menus

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int bd_play(IntPtr bd);

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int bd_play_title(IntPtr bd, uint title);

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int bd_menu_call(IntPtr bd, long pts);

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int bd_read_ext(IntPtr bd, IntPtr buf, int len, out BdEvent @event);

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int bd_read_skip_still(IntPtr bd);

        #endregion

        #region User Interaction

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void bd_set_scr(IntPtr bd, long pts);

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int bd_set_rate(IntPtr bd, uint rate);

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int bd_user_input(IntPtr bd, long pts, uint key);

        [DllImport(LibblurayLibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int bd_mouse_select(IntPtr bd, long pts, ushort x, ushort y);

        #endregion
    }

    #region Struct Definitions

    [StructLayout(LayoutKind.Sequential)]
    public struct BdEvent
    {
        public uint Event;
        public uint Param;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BlurayDiscInfo
    {
        public byte BlurayDetected;

        // Disc ID
        public IntPtr DiscName;
        public IntPtr UdfVolumeId;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] DiscId;

        // Menu support
        public byte NoMenuSupport;
        public byte FirstPlaySupported;
        public byte TopMenuSupported;

        // Titles
        public uint NumTitles;
        public IntPtr Titles; // Array of pointers to BLURAY_TITLE
        public IntPtr FirstPlay; // Pointer to BLURAY_TITLE
        public IntPtr TopMenu; // Pointer to BLURAY_TITLE

        public uint NumHdmvTitles;
        public uint NumBdjTitles;
        public uint NumUnsupportedTitles;

        // BD-J info
        public byte BdjDetected;
        public byte BdjSupported;
        public byte LibjvmDetected;
        public byte BdjHandled;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public byte[] BdjOrgId;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
        public byte[] BdjDiscId;

        // Disc application info
        public byte VideoFormat;
        public byte FrameRate;
        public byte ContentExist3D;
        public byte InitialOutputModePreference;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] ProviderData;

        // AACS info
        public byte AacsDetected;
        public byte LibaacsDetected;
        public byte AacsHandled;
        public int AacsErrorCode;
        public int AacsMkbv;

        // BD+ info
        public byte BdplusDetected;
        public byte LibbdplusDetected;
        public byte BdplusHandled;
        public byte BdplusGen;
        public uint BdplusDate;

        // Additional fields for newer versions
        public byte InitialDynamicRangeType;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BlurayStreamInfo
    {
        public byte CodingType;
        public byte Format;
        public byte Rate;
        public byte CharCode;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Lang;
        public ushort Pid;
        public byte Aspect;
        public byte SubpathId;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BlurayClipInfo
    {
        public uint PktCount;
        public byte StillMode;
        public ushort StillTime;
        public byte VideoStreamCount;
        public byte AudioStreamCount;
        public byte PgStreamCount;
        public byte IgStreamCount;
        public byte SecAudioStreamCount;
        public byte SecVideoStreamCount;
        
        public IntPtr VideoStreams; // Pointer to array of BlurayStreamInfo
        public IntPtr AudioStreams; // Pointer to array of BlurayStreamInfo
        public IntPtr PgStreams; // Pointer to array of BlurayStreamInfo
        public IntPtr IgStreams; // Pointer to array of BlurayStreamInfo
        public IntPtr SecAudioStreams; // Pointer to array of BlurayStreamInfo
        public IntPtr SecVideoStreams; // Pointer to array of BlurayStreamInfo

        public ulong StartTime;
        public ulong InTime;
        public ulong OutTime;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] ClipId;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BlurayTitleChapter
    {
        public uint Idx;
        public ulong Start;
        public ulong Duration;
        public ulong Offset;
        public uint ClipRef;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BlurayTitleMark
    {
        public uint Idx;
        public int Type;
        public ulong Start;
        public ulong Duration;
        public ulong Offset;
        public uint ClipRef;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BlurayTitleInfo
    {
        public uint Idx;
        public uint Playlist;
        public ulong Duration;
        public uint ClipCount;
        public byte AngleCount;
        public uint ChapterCount;
        public uint MarkCount;
        
        public IntPtr Clips; // Pointer to array of BlurayClipInfo
        public IntPtr Chapters; // Pointer to array of BlurayTitleChapter
        public IntPtr Marks; // Pointer to array of BlurayTitleMark
        
        public byte MvcBaseViewRFlag;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BluraySoundEffect
    {
        public byte NumChannels;
        public uint NumFrames;
        public IntPtr Samples; // Pointer to array of short
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BlurayTitle
    {
        public IntPtr Name;
        public byte Interactive;
        public byte Accessible;
        public byte Hidden;
        public byte Bdj;
        public uint IdRef;
    }

    #endregion

    #region Constants

    public static class TitleFlags
    {
        public const byte All = 0;
        public const byte FilterDupTitle = 0x01;
        public const byte FilterDupClip = 0x02;
        public const byte Relevant = FilterDupTitle | FilterDupClip;
    }

    public static class EventTypes
    {
        public const uint None = 0;
        public const uint Error = 1;
        public const uint ReadError = 2;
        public const uint Encrypted = 3;
        public const uint Angle = 4;
        public const uint Title = 5;
        public const uint Playlist = 6;
        public const uint PlayItem = 7;
        public const uint Chapter = 8;
        public const uint PlayMark = 9;
        public const uint EndOfTitle = 10;
        public const uint AudioStream = 11;
        public const uint IgStream = 12;
        public const uint PgTextstStream = 13;
        public const uint PipPgTextstStream = 14;
        public const uint SecondaryAudioStream = 15;
        public const uint SecondaryVideoStream = 16;
        public const uint PgTextst = 17;
        public const uint PipPgTextst = 18;
        public const uint SecondaryAudio = 19;
        public const uint SecondaryVideo = 20;
        public const uint SecondaryVideoSize = 21;
        public const uint PlaylistStop = 22;
        public const uint Discontinuity = 23;
        public const uint Seek = 24;
        public const uint Still = 25;
        public const uint StillTime = 26;
        public const uint SoundEffect = 27;
        public const uint Idle = 28;
        public const uint Popup = 29;
        public const uint Menu = 30;
        public const uint StereoscopicStatus = 31;
        public const uint KeyInterestTable = 32;
        public const uint UoMaskChanged = 33;
    }

    public static class PlayerSettings
    {
        public const uint AudioLang = 16;
        public const uint PgLang = 17;
        public const uint MenuLang = 18;
        public const uint CountryCode = 19;
        public const uint RegionCode = 20;
        public const uint OutputPrefer = 21;
        public const uint Parental = 13;
        public const uint AudioCap = 15;
        public const uint VideoCap = 29;
        public const uint DisplayCap = 23;
        public const uint ThreeDCap = 24;
        public const uint UhdCap = 25;
        public const uint UhdDisplayCap = 26;
        public const uint HdrPreference = 27;
        public const uint SdrConvPrefer = 28;
        public const uint TextCap = 30;
        public const uint PlayerProfile = 31;

        public const uint DecodePg = 0x100;
        public const uint PersistentStorage = 0x101;

        public const uint PersistentRoot = 0x200;
        public const uint CacheRoot = 0x201;
        public const uint JavaHome = 0x202;
    }

    public static class StreamTypes
    {
        public const uint AudioStream = 0;
        public const uint PgTextstStream = 1;
    }

    public static class RateValues
    {
        public const uint Paused = 0;
        public const uint Normal = 90000;
    }

    #endregion
}