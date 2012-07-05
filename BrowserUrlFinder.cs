using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using System.Runtime.InteropServices;
using System.Diagnostics;
namespace WindowTracking
{
    /// <summary>
    /// code retrieved from http://stackoverflow.com/questions/5317642/retrieve-current-url-from-c-sharp-windows-form
    /// </summary>
    class BrowserUrlFinder
    {
        public static string GetChromeUrl(Process process) {
            if (process == null)
                throw new ArgumentNullException("process");

            if (process.MainWindowHandle == IntPtr.Zero)
                return null;

            AutomationElement element = AutomationElement.FromHandle(process.MainWindowHandle);
            if (element == null)
                return null;

            AutomationElement edit = element.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));
            return ((ValuePattern)edit.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
        }

        public static string GetInternetExplorerUrl(Process process) {
            if (process == null)
                throw new ArgumentNullException("process");

            if (process.MainWindowHandle == IntPtr.Zero)
                return null;

            AutomationElement element = AutomationElement.FromHandle(process.MainWindowHandle);
            if (element == null)
                return null;

            AutomationElement rebar = element.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, "ReBarWindow32"));
            if (rebar == null)
                return null;

            AutomationElement edit = rebar.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));

            return ((ValuePattern)edit.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
        }

        public static string GetFirefoxUrl(Process process) {
            if (process == null)
                throw new ArgumentNullException("process");

            if (process.MainWindowHandle == IntPtr.Zero)
                return null;

            AutomationElement element = AutomationElement.FromHandle(process.MainWindowHandle);
            if (element == null)
                return null;

            AutomationElement doc = element.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Document));
            if (doc == null)
                return null;

            return ((ValuePattern)doc.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
        }
    }
}
