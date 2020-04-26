using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace WakeTFT
{
    public class Utility
    {

        public static bool IsOpen
        {
            get { return LeagueProcess.Any(IsProcessOpen); }
        }

        private static bool IsProcessOpen(Process leagueProcess)
        {
            if (leagueProcess != null)
            {
                return true;
            }
            return false;
        }
        public static List<IntPtr> LeagueInstances
        {
            get { return FindWindows("League of Legends (TM) Client"); }
        }

        public static UInt32 LeagueId
        {
            get {
                if (LeagueProcess.Count <= 0)
                {
                    return 0;
                }
                return (UInt32)LeagueProcess[0].Id; 
            }
        }

        private static List<Process> LeagueProcess
        {
            get { return Process.GetProcessesByName("League of Legends").ToList(); }
        }
        private static string GetWindowText(IntPtr hWnd)
        {
            var size = Interop.GetWindowTextLength(hWnd);
            if (size++ > 0)
            {
                var builder = new StringBuilder(size);
                Interop.GetWindowText(hWnd, builder, builder.Capacity);
                return builder.ToString();
            }
            return String.Empty;
        }


        private static List<IntPtr> FindWindows(string title)
        {
            var windows = new List<IntPtr>();
            Interop.EnumWindows(delegate (IntPtr wnd, IntPtr param)
            {
                if (GetWindowText(wnd).Contains(title))
                {
                    windows.Add(wnd);
                }
                return true;
            }, IntPtr.Zero);
            return windows;
        }
    }
}
