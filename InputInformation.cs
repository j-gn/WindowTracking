using System;
using System.Runtime.InteropServices;

namespace WindowTracking
{
    /// <summary>
    /// Code taken from
    /// http://joelabrahamsson.com/entry/detecting-mouse-and-keyboard-input-with-net
    /// http://joelabrahamsson.com/blogfiles/InputTrackingExample.zip
    /// </summary>
    public class InputInformation
    {
        [DllImport("User32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
        public static DateTime GetLastInputTime()
        {
            var lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
            GetLastInputInfo(ref lastInputInfo);
            return DateTime.Now.AddMilliseconds(-(Environment.TickCount - lastInputInfo.dwTime));
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }
    }
}
