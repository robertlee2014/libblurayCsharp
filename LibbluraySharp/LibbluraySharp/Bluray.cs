using System;
using System.Runtime.InteropServices;
using System.Text;
using LibbluraySharp.Native;

namespace LibbluraySharp
{
    /// <summary>
    /// Main class for interacting with libbluray
    /// </summary>
    public class Bluray : IDisposable
    {
        private IntPtr _bdPtr;
        private bool _disposed = false;

        #region Properties

        public IntPtr Handle => _bdPtr;
        public bool IsOpen => _bdPtr != IntPtr.Zero;

        #endregion

        #region Constructors

        public Bluray()
        {
            _bdPtr = NativeMethods.bd_init();
            if (_bdPtr == IntPtr.Zero)
            {
                throw new InvalidOperationException("Failed to initialize libbluray");
            }
        }

        #endregion

        #region Finalizer

        ~Bluray()
        {
            Dispose(false);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Opens a Blu-ray disc
        /// </summary>
        /// <param name="devicePath">Path to mounted Blu-ray disc, device or image file</param>
        /// <param name="keyfilePath">Path to KEYDB.cfg (optional)</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool Open(string devicePath, string keyfilePath = null)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Bluray));

            if (string.IsNullOrEmpty(devicePath))
                throw new ArgumentException("Device path cannot be null or empty", nameof(devicePath));

            var result = NativeMethods.bd_open_disc(_bdPtr, devicePath, keyfilePath ?? string.Empty);
            return result != 0;
        }

        /// <summary>
        /// Opens a Blu-ray disc (static method that creates and returns a Bluray instance)
        /// </summary>
        /// <param name="devicePath">Path to mounted Blu-ray disc, device or image file</param>
        /// <param name="keyfilePath">Path to KEYDB.cfg (optional)</param>
        /// <returns>Bluray instance if successful, null otherwise</returns>
        public static Bluray OpenDisc(string devicePath, string keyfilePath = null)
        {
            if (string.IsNullOrEmpty(devicePath))
                throw new ArgumentException("Device path cannot be null or empty", nameof(devicePath));

            var bdPtr = NativeMethods.bd_open(devicePath, keyfilePath ?? string.Empty);
            if (bdPtr == IntPtr.Zero)
                return null;

            var bluray = new Bluray { _bdPtr = bdPtr };
            return bluray;
        }

        /// <summary>
        /// Gets disc information
        /// </summary>
        /// <returns>Disc information or null if not available</returns>
        public DiscInfo GetDiscInfo()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Bluray));

            var infoPtr = NativeMethods.bd_get_disc_info(_bdPtr);
            if (infoPtr == IntPtr.Zero)
                return null;

            var discInfo = Marshal.PtrToStructure<BlurayDiscInfo>(infoPtr);
            return new DiscInfo(discInfo);
        }

        /// <summary>
        /// Gets the number of titles on the disc
        /// </summary>
        /// <param name="flags">Title filtering flags</param>
        /// <param name="minTitleLength">Minimum title length in seconds (for filtering)</param>
        /// <returns>Number of titles</returns>
        public uint GetTitles(byte flags = TitleFlags.All, uint minTitleLength = 0)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Bluray));

            return NativeMethods.bd_get_titles(_bdPtr, flags, minTitleLength);
        }

        /// <summary>
        /// Gets the main title index
        /// </summary>
        /// <returns>Main title index, or -1 if not available</returns>
        public int GetMainTitle()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Bluray));

            return NativeMethods.bd_get_main_title(_bdPtr);
        }

        /// <summary>
        /// Gets information about a specific title
        /// </summary>
        /// <param name="titleIdx">Title index</param>
        /// <param name="angle">Angle number (default is 0)</param>
        /// <returns>Title information or null if not available</returns>
        public TitleInfo GetTitleInfo(uint titleIdx, uint angle = 0)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Bluray));

            var infoPtr = NativeMethods.bd_get_title_info(_bdPtr, titleIdx, angle);
            if (infoPtr == IntPtr.Zero)
                return null;

            try
            {
                var titleInfo = Marshal.PtrToStructure<BlurayTitleInfo>(infoPtr);
                var result = new TitleInfo(titleInfo);
                return result;
            }
            finally
            {
                NativeMethods.bd_free_title_info(infoPtr);
            }
        }

        /// <summary>
        /// Selects a title for playback
        /// </summary>
        /// <param name="title">Title index to select</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool SelectTitle(uint title)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Bluray));

            var result = NativeMethods.bd_select_title(_bdPtr, title);
            return result != 0;
        }

        /// <summary>
        /// Selects a playlist for playback
        /// </summary>
        /// <param name="playlist">Playlist ID to select</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool SelectPlaylist(uint playlist)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Bluray));

            var result = NativeMethods.bd_select_playlist(_bdPtr, playlist);
            return result != 0;
        }

        /// <summary>
        /// Gets the current title index
        /// </summary>
        /// <returns>Current title index</returns>
        public uint GetCurrentTitle()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Bluray));

            return NativeMethods.bd_get_current_title(_bdPtr);
        }

        /// <summary>
        /// Reads data from the currently selected title
        /// </summary>
        /// <param name="buffer">Buffer to read data into</param>
        /// <param name="offset">Offset in the buffer to start writing</param>
        /// <param name="count">Number of bytes to read</param>
        /// <returns>Number of bytes actually read, or -1 on error</returns>
        public int Read(byte[] buffer, int offset, int count)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Bluray));
            
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            
            if (offset < 0 || offset >= buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            
            if (count <= 0 || offset + count > buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(count));

            var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                var ptr = new IntPtr(handle.AddrOfPinnedObject().ToInt64() + offset);
                var result = NativeMethods.bd_read(_bdPtr, ptr, count);
                return result;
            }
            finally
            {
                handle.Free();
            }
        }

        /// <summary>
        /// Seeks to a specific position in the current title
        /// </summary>
        /// <param name="position">Position to seek to</param>
        /// <returns>New position after seeking</returns>
        public long Seek(ulong position)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Bluray));

            return NativeMethods.bd_seek(_bdPtr, position);
        }

        /// <summary>
        /// Seeks to a specific time in the current title
        /// </summary>
        /// <param name="tick">Time in 90kHz ticks</param>
        /// <returns>New position after seeking</returns>
        public long SeekTime(ulong tick)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Bluray));

            return NativeMethods.bd_seek_time(_bdPtr, tick);
        }

        /// <summary>
        /// Seeks to a specific chapter in the current title
        /// </summary>
        /// <param name="chapter">Chapter number (0-based)</param>
        /// <returns>New position after seeking</returns>
        public long SeekChapter(uint chapter)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Bluray));

            return NativeMethods.bd_seek_chapter(_bdPtr, chapter);
        }

        /// <summary>
        /// Selects an angle for playback
        /// </summary>
        /// <param name="angle">Angle number</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool SelectAngle(uint angle)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Bluray));

            var result = NativeMethods.bd_select_angle(_bdPtr, angle);
            return result != 0;
        }

        /// <summary>
        /// Performs a seamless angle change
        /// </summary>
        /// <param name="angle">New angle to switch to</param>
        public void SeamlessAngleChange(uint angle)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Bluray));

            NativeMethods.bd_seamless_angle_change(_bdPtr, angle);
        }

        /// <summary>
        /// Gets the position of a specific chapter
        /// </summary>
        /// <param name="chapter">Chapter number (0-based)</param>
        /// <returns>Position of the chapter</returns>
        public long ChapterPos(uint chapter)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Bluray));

            return NativeMethods.bd_chapter_pos(_bdPtr, chapter);
        }

        /// <summary>
        /// Gets the current chapter
        /// </summary>
        /// <returns>Current chapter number</returns>
        public uint GetCurrentChapter()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Bluray));

            return NativeMethods.bd_get_current_chapter(_bdPtr);
        }

        /// <summary>
        /// Gets the size of the current title
        /// </summary>
        /// <returns>Size of the current title in bytes</returns>
        public ulong GetTitleSize()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Bluray));

            return NativeMethods.bd_get_title_size(_bdPtr);
        }

        /// <summary>
        /// Gets the current angle
        /// </summary>
        /// <returns>Current angle number</returns>
        public uint GetCurrentAngle()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Bluray));

            return NativeMethods.bd_get_current_angle(_bdPtr);
        }

        /// <summary>
        /// Gets the current position
        /// </summary>
        /// <returns>Current position</returns>
        public ulong Tell()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Bluray));

            return NativeMethods.bd_tell(_bdPtr);
        }

        /// <summary>
        /// Gets the current time
        /// </summary>
        /// <returns>Current time in 90kHz ticks</returns>
        public ulong TellTime()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Bluray));

            return NativeMethods.bd_tell_time(_bdPtr);
        }

        /// <summary>
        /// Sets a player setting (integer value)
        /// </summary>
        /// <param name="setting">Setting to update</param>
        /// <param name="value">New value</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool SetPlayerSetting(uint setting, uint value)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Bluray));

            var result = NativeMethods.bd_set_player_setting(_bdPtr, setting, value);
            return result != 0;
        }

        /// <summary>
        /// Sets a player setting (string value)
        /// </summary>
        /// <param name="setting">Setting to update</param>
        /// <param name="value">New value</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool SetPlayerSettingString(uint setting, string value)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Bluray));

            var result = NativeMethods.bd_set_player_setting_str(_bdPtr, setting, value ?? string.Empty);
            return result != 0;
        }

        /// <summary>
        /// Gets the next event from the event queue
        /// </summary>
        /// <param name="event">Event structure to fill</param>
        /// <returns>True if an event was retrieved, false if no events</returns>
        public bool GetEvent(out Event @event)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Bluray));

            var nativeEvent = new BdEvent();
            var result = NativeMethods.bd_get_event(_bdPtr, out nativeEvent);
            
            if (result != 0)
            {
                @event = new Event(nativeEvent);
                return true;
            }
            
            @event = new Event();
            return false;
        }

        /// <summary>
        /// Starts playing the disc with on-disc menus
        /// </summary>
        /// <returns>True if successful, false otherwise</returns>
        public bool Play()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Bluray));

            var result = NativeMethods.bd_play(_bdPtr);
            return result != 0;
        }

        /// <summary>
        /// Plays a specific title from the disc index
        /// </summary>
        /// <param name="title">Title number from disc index</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool PlayTitle(uint title)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Bluray));

            var result = NativeMethods.bd_play_title(_bdPtr, title);
            return result != 0;
        }

        /// <summary>
        /// Calls the Blu-ray disc top menu
        /// </summary>
        /// <param name="pts">Current playback position in 1/90000s or -1</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool MenuCall(long pts = -1)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Bluray));

            var result = NativeMethods.bd_menu_call(_bdPtr, pts);
            return result != 0;
        }

        /// <summary>
        /// Reads data from currently playing title with event handling
        /// </summary>
        /// <param name="buffer">Buffer to read data into</param>
        /// <param name="offset">Offset in the buffer to start writing</param>
        /// <param name="count">Number of bytes to read</param>
        /// <param name="event">Event structure to fill if an event occurs</param>
        /// <returns>Number of bytes read, -1 on error, 0 if event needs to be handled first</returns>
        public int ReadExtended(byte[] buffer, int offset, int count, out Event @event)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Bluray));
            
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            
            if (offset < 0 || offset >= buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));
            
            if (count <= 0 || offset + count > buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(count));

            var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                var ptr = new IntPtr(handle.AddrOfPinnedObject().ToInt64() + offset);
                
                var nativeEvent = new BdEvent();
                var result = NativeMethods.bd_read_ext(_bdPtr, ptr, count, out nativeEvent);
                
                @event = new Event(nativeEvent);
                return result;
            }
            finally
            {
                handle.Free();
            }
        }

        /// <summary>
        /// Continues reading after still mode clip
        /// </summary>
        /// <returns>True if successful, false otherwise</returns>
        public bool ReadSkipStill()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Bluray));

            var result = NativeMethods.bd_read_skip_still(_bdPtr);
            return result != 0;
        }

        /// <summary>
        /// Updates current playback time stamp
        /// </summary>
        /// <param name="pts">Current playback position in 1/90000s or -1</param>
        public void SetScr(long pts = -1)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Bluray));

            NativeMethods.bd_set_scr(_bdPtr, pts);
        }

        /// <summary>
        /// Sets the current playback rate
        /// </summary>
        /// <param name="rate">Playback rate * 90000 (0 = paused, 90000 = normal)</param>
        /// <returns>0 on success, negative on error</returns>
        public int SetRate(uint rate)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Bluray));

            return NativeMethods.bd_set_rate(_bdPtr, rate);
        }

        /// <summary>
        /// Sends user input to graphics controller or BD-J
        /// </summary>
        /// <param name="pts">Current playback position in 1/90000s or -1</param>
        /// <param name="key">Input key</param>
        /// <returns>Negative on error, 0 on success, positive if selection/activation changed</returns>
        public int UserInput(long pts = -1, uint key = 0)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Bluray));

            return NativeMethods.bd_user_input(_bdPtr, pts, key);
        }

        /// <summary>
        /// Selects menu button at location (x,y)
        /// </summary>
        /// <param name="pts">Current playback position in 1/90000s or -1</param>
        /// <param name="x">Mouse pointer x-position</param>
        /// <param name="y">Mouse pointer y-position</param>
        /// <returns>Negative on error, 0 when mouse is outside of buttons, 1 when mouse is inside button</returns>
        public int MouseSelect(long pts = -1, ushort x = 0, ushort y = 0)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Bluray));

            return NativeMethods.bd_mouse_select(_bdPtr, pts, x, y);
        }

        #endregion

        #region IDisposable Implementation

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (_bdPtr != IntPtr.Zero)
                {
                    NativeMethods.bd_close(_bdPtr);
                    _bdPtr = IntPtr.Zero;
                }
                _disposed = true;
            }
        }

        #endregion
    }
}