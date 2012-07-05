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

        const int MAX_IDLE_SECONDS = 15;
        static bool _userIsIdle = false;

        static void Main(string[] args) {
            IntPtr currentWindowHandle = IntPtr.Zero;
            IntPtr lastWindowHandle = IntPtr.Zero;
            while (true) {
                currentWindowHandle = GetForegroundWindow();
                if (currentWindowHandle.ToInt32() != lastWindowHandle.ToInt32()) {
                    lastWindowHandle = currentWindowHandle;

                    uint outPid;
                    GetWindowThreadProcessId(currentWindowHandle, out outPid);
                    int pID =(int)outPid;

                    try {
                        Process p = Process.GetProcessById(pID);
                        Console.WriteLine("focus on: [" + p.ProcessName + "] pID: " + pID);
                        switch(p.ProcessName.ToLower()){
                            case "chrome":
                                Console.WriteLine("URL:" + BrowserUrlFinder.GetChromeUrl(p));
                            break;
                            case "firefox":
                                Console.WriteLine("URL:" + BrowserUrlFinder.GetFirefoxUrl(p));
                            break;
                            default:
                            break;
                        }
                    }
                    catch {
                        Console.WriteLine("no process with id" + pID);
                    }
                }
                TimeSpan timeSinceLastInput = (DateTime.Now - InputInformation.GetLastInputTime());
                if (!_userIsIdle && timeSinceLastInput.Seconds > MAX_IDLE_SECONDS) {
                    Console.WriteLine("user has left the workstation");
                    _userIsIdle = true;
                }
                else if (_userIsIdle && timeSinceLastInput.Seconds < MAX_IDLE_SECONDS) {
                    Console.WriteLine("user is back");
                    _userIsIdle = false;
                }
                Thread.Sleep(250);
            }
        }
        
    }
}
