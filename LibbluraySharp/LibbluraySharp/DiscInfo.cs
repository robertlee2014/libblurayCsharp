using System;
using System.Runtime.InteropServices;
using LibbluraySharp.Native;

namespace LibbluraySharp
{
    /// <summary>
    /// Represents Blu-ray disc information
    /// </summary>
    public class DiscInfo
    {
        private readonly BlurayDiscInfo _nativeInfo;

        internal DiscInfo(BlurayDiscInfo nativeInfo)
        {
            _nativeInfo = nativeInfo;
        }

        public bool BlurayDetected => _nativeInfo.BlurayDetected != 0;

        public string DiscName => Marshal.PtrToStringAnsi(_nativeInfo.DiscName);
        public string UdfVolumeId => Marshal.PtrToStringAnsi(_nativeInfo.UdfVolumeId);
        public byte[] DiscId => _nativeInfo.DiscId;

        public bool NoMenuSupport => _nativeInfo.NoMenuSupport != 0;
        public bool FirstPlaySupported => _nativeInfo.FirstPlaySupported != 0;
        public bool TopMenuSupported => _nativeInfo.TopMenuSupported != 0;

        public uint NumTitles => _nativeInfo.NumTitles;
        // Note: We don't expose Titles array directly due to complexity of marshaling arrays of structs with pointers
        public uint NumHdmvTitles => _nativeInfo.NumHdmvTitles;
        public uint NumBdjTitles => _nativeInfo.NumBdjTitles;
        public uint NumUnsupportedTitles => _nativeInfo.NumUnsupportedTitles;

        public bool BdjDetected => _nativeInfo.BdjDetected != 0;
        public bool BdjSupported => _nativeInfo.BdjSupported != 0;
        public bool LibjvmDetected => _nativeInfo.LibjvmDetected != 0;
        public bool BdjHandled => _nativeInfo.BdjHandled != 0;

        public string BdjOrgId => ExtractNullTerminatedString(_nativeInfo.BdjOrgId);
        public string BdjDiscId => ExtractNullTerminatedString(_nativeInfo.BdjDiscId);

        public byte VideoFormat => _nativeInfo.VideoFormat;
        public byte FrameRate => _nativeInfo.FrameRate;
        public bool ContentExist3D => _nativeInfo.ContentExist3D != 0;
        public bool InitialOutputModePreference => _nativeInfo.InitialOutputModePreference != 0;
        public byte[] ProviderData => _nativeInfo.ProviderData;

        public bool AacsDetected => _nativeInfo.AacsDetected != 0;
        public bool LibaacsDetected => _nativeInfo.LibaacsDetected != 0;
        public bool AacsHandled => _nativeInfo.AacsHandled != 0;
        public int AacsErrorCode => _nativeInfo.AacsErrorCode;
        public int AacsMkbv => _nativeInfo.AacsMkbv;

        public bool BdplusDetected => _nativeInfo.BdplusDetected != 0;
        public bool LibbdplusDetected => _nativeInfo.LibbdplusDetected != 0;
        public bool BdplusHandled => _nativeInfo.BdplusHandled != 0;
        public byte BdplusGen => _nativeInfo.BdplusGen;
        public uint BdplusDate => _nativeInfo.BdplusDate;

        public byte InitialDynamicRangeType => _nativeInfo.InitialDynamicRangeType;

        private static string ExtractNullTerminatedString(byte[] bytes)
        {
            if (bytes == null) return null;
            
            int nullIndex = Array.IndexOf(bytes, (byte)0);
            if (nullIndex >= 0)
            {
                return System.Text.Encoding.UTF8.GetString(bytes, 0, nullIndex);
            }
            else
            {
                return System.Text.Encoding.UTF8.GetString(bytes);
            }
        }
    }
}