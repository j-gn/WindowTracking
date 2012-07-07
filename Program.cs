using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace WindowTracking
{
    class Program
    {
        public static readonly string DATA_FOLDER = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\WindowTracking\";
        public static readonly string DATA_FILE = "log.db";
        /// <summary>Gives you the curret foreground window handle</summary>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();
        /// <param name="hWnd">window handle</param>
        /// <param name="lpdwProcessId">gives you the process id </param>
        /// <returns>might return the thread id</returns>
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
//        [DllImport("user32.dll")]
//        static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        const int MAX_IDLE_SECONDS = 30;
        static bool _userIsIdle = false;

        static void Main(string[] args) {
            EventRecorder eventRecorder = new EventRecorder(DATA_FOLDER, DATA_FILE);
            IntPtr currentWindowHandle = IntPtr.Zero;
            IntPtr lastWindowHandle = IntPtr.Zero;
            while (true) {
                currentWindowHandle = GetForegroundWindow();
                if (currentWindowHandle.ToInt32() != lastWindowHandle.ToInt32()) {
                    lastWindowHandle = currentWindowHandle;
                    string focusedProcessName = GetProcessName(currentWindowHandle);

                    eventRecorder.SwapEvent(focusedProcessName);
                    switch (focusedProcessName.ToLower()) {
                        case "chrome":
                        //Console.WriteLine("URL:" + BrowserUrlFinder.GetChromeUrl(p));
                        break;
                        case "firefox":
                        //Console.WriteLine("URL:" + BrowserUrlFinder.GetFirefoxUrl(p));
                        break;
                        default:
                        break;
                    }
                }
                TimeSpan timeSinceLastInput = (DateTime.Now - InputInformation.GetLastInputTime());
                if (!_userIsIdle && timeSinceLastInput.Seconds > MAX_IDLE_SECONDS) {
                    Console.WriteLine("user has left the workstation");
                    _userIsIdle = true;
                    eventRecorder.SwapEvent("user_idle");
                }
                else if (_userIsIdle && timeSinceLastInput.Seconds < MAX_IDLE_SECONDS) {
                    Console.WriteLine("user is back");
                    _userIsIdle = false;
                    eventRecorder.SwapEvent(GetProcessName(currentWindowHandle));
                }
                Thread.Sleep(250);
            }
        }
        static string GetProcessName(IntPtr pWindowHandle) {
            uint outPid;
            GetWindowThreadProcessId(pWindowHandle, out outPid);
            int pID = (int)outPid;
            Process p = null;
            try {
                p = Process.GetProcessById(pID);
                Console.WriteLine("focus on: [" + p.ProcessName + "]");
                return p.ProcessName;
            }
            catch (Exception e) {
                Console.WriteLine("no process with id " + pID);
                return "";
            }
        }
        
    }
}
