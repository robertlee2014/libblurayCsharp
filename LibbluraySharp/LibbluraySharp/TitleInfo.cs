using System;
using System.Runtime.InteropServices;
using LibbluraySharp.Native;

namespace LibbluraySharp
{
    /// <summary>
    /// Represents information about a Blu-ray title
    /// </summary>
    public class TitleInfo
    {
        private readonly BlurayTitleInfo _nativeInfo;

        internal TitleInfo(BlurayTitleInfo nativeInfo)
        {
            _nativeInfo = nativeInfo;
        }

        public uint Idx => _nativeInfo.Idx;
        public uint Playlist => _nativeInfo.Playlist;
        public ulong Duration => _nativeInfo.Duration;
        public uint ClipCount => _nativeInfo.ClipCount;
        public byte AngleCount => _nativeInfo.AngleCount;
        public uint ChapterCount => _nativeInfo.ChapterCount;
        public uint MarkCount => _nativeInfo.MarkCount;
        public byte MvcBaseViewRFlag => _nativeInfo.MvcBaseViewRFlag;

        // Note: These would require complex marshaling to properly implement
        // public BlurayClipInfo[] Clips => MarshalClips();
        // public BlurayTitleChapter[] Chapters => MarshalChapters();
        // public BlurayTitleMark[] Marks => MarshalMarks();
    }
}