using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace BepInCharacterSwapper.Patches
{
    [HarmonyPatch(typeof(ModelPageManager), nameof(ModelPageManager.Start))]
    internal class GameWindow_Patches
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CreateWindowEx(
       uint dwExStyle,
       string lpClassName,
       string lpWindowName,
       uint dwStyle,
       int x,
       int y,
       int nWidth,
       int nHeight,
       IntPtr hWndParent,
       IntPtr hMenu,
       IntPtr hInstance,
       IntPtr lpParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(
        IntPtr hWnd,
        IntPtr hWndInsertAfter,
        int X,
        int Y,
        int cx,
        int cy,
        uint uFlags);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int MessageBox(IntPtr hWnd, string lpText, string lpCaption, uint uType);

        private const uint WS_EX_NOACTIVATE = 0x08000000;
        private const uint WS_EX_TOOLWINDOW = 0x00000080;
        private const uint WS_EX_APPWINDOW = 0x00040000;

        private const int GWL_EXSTYLE = -20;
        private const int GWLP_HWNDPARENT = -8;

        private const uint MB_ICONERROR = 0x00000010;

        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);

        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_SHOWWINDOW = 0x0040;

        public static void ReParent(IntPtr hWnd)
        {
            IntPtr hOwner = CreateWindowEx(
                WS_EX_NOACTIVATE,
                "STATIC",
                "Dummy",
                0,
                0,
                0,
                0,
                0,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero);

            if (hOwner == IntPtr.Zero)
            {
                // If window creation fails, show an error message
                MessageBox(IntPtr.Zero, "Failed to create owner window", "Error", MB_ICONERROR);
                return;
            }

            // Set the dummy window as the parent
            SetWindowLongPtr(hWnd, GWLP_HWNDPARENT, hOwner);

            // Adjust the window style
            int exStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
            exStyle &= ~((int)WS_EX_APPWINDOW);
            exStyle |= (int)WS_EX_TOOLWINDOW;
            SetWindowLong(hWnd, GWL_EXSTYLE, exStyle);

            SetForegroundWindow(hWnd);
            SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_SHOWWINDOW);
        }

        private static void Postfix(ModelPageManager __instance)
        {
            IntPtr mainWindow = Process.GetCurrentProcess().MainWindowHandle;
            ReParent(mainWindow);
            SetWindowPos(mainWindow, HWND_TOPMOST, 0, 0, 0, 0, SWP_SHOWWINDOW);
        }
    }
}
