using System;
using System.Runtime.InteropServices;

namespace InputTrackingExample
{
    /// <summary>
    /// Code taken from
    /// http://joelabrahamsson.com/entry/detecting-mouse-and-keyboard-input-with-net
    /// http://joelabrahamsson.com/blogfiles/InputTrackingExample.zip
    /// </summary>
    public class WindowsHookHelper
    {
        public delegate IntPtr HookDelegate(
            Int32 Code, IntPtr wParam, IntPtr lParam);

        [DllImport("User32.dll")]
        public static extern IntPtr CallNextHookEx(
            IntPtr hHook, Int32 nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("User32.dll")]
        public static extern IntPtr UnhookWindowsHookEx(IntPtr hHook);


        [DllImport("User32.dll")]
        public static extern IntPtr SetWindowsHookEx(
            Int32 idHook, HookDelegate lpfn, IntPtr hmod, 
            Int32 dwThreadId);
    }
}