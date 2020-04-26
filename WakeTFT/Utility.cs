using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using static WakeTFT.D3DXMath;

namespace WakeTFT
{
    public class Utility
    {
        public static D3DXVECTOR2 WorldToScreen(D3DXVECTOR3 objLoc, D3DXVECTOR2 screenRes, D3DXMATRIX vpMatrix)
        {
            D3DXVECTOR2 returnVec = new D3DXVECTOR2();

            D3DXVECTOR4 clipCoords = new D3DXVECTOR4();
            clipCoords.x = objLoc.x * vpMatrix._11 + objLoc.y * vpMatrix._21 + objLoc.z * vpMatrix._31 + vpMatrix._41;
            clipCoords.y = objLoc.x * vpMatrix._12 + objLoc.y * vpMatrix._22 + objLoc.z * vpMatrix._32 + vpMatrix._42;
            clipCoords.z = objLoc.x * vpMatrix._13 + objLoc.y * vpMatrix._23 + objLoc.z * vpMatrix._33 + vpMatrix._43;
            clipCoords.w = objLoc.x * vpMatrix._14 + objLoc.y * vpMatrix._24 + objLoc.z * vpMatrix._34 + vpMatrix._44;

            if (clipCoords.w < 0.1f)
            {
                return returnVec;
            }

            D3DXVECTOR3 M = new D3DXVECTOR3();
            M.x = clipCoords.x / clipCoords.w;
            M.y = clipCoords.y / clipCoords.w;
            M.z = clipCoords.z / clipCoords.w;

            returnVec.x = (screenRes.x / 2 * M.x) + (M.x + screenRes.x / 2);
            returnVec.y = -(screenRes.y / 2 * M.y) + (M.y + screenRes.y / 2);

            return returnVec;
        }

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
