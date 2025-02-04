/*

Most of the code Originated here
https://stackoverflow.com/questions/604410/global-keyboard-capture-in-c-sharp-application

It just prints received key strokes and their timestamp relative to Windows bootup time.
If the timestamps are equal they were received at the same time by the pc.

Timestamps are given in milliseconds as this seems to be the lowest resolution Windows offers.

*/

using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace KeyChordTester
{
    public static class KeyChordTester
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        [STAThread]
        public static void Main()
        {
            AllocConsole();
            _hookID = SetHook(_proc);
            Application.Run();
            UnhookWindowsHookEx(_hookID);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(
            int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                int time = Marshal.ReadInt32(lParam + 12);
                Console.WriteLine($"Key: {(Keys)vkCode} - Stamp: {time} KEYDOWN");
            }
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYUP)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                int time = Marshal.ReadInt32(lParam + 12);
                Console.WriteLine($"Key: {(Keys)vkCode} - Stamp: {time} KEYUP");
            }
            if (nCode >= 0 && wParam == (IntPtr)WM_SYSKEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                int time = Marshal.ReadInt32(lParam + 12);
                Console.WriteLine($"Key: {(Keys)vkCode} - Stamp: {time} SYSKEYDOWN");
            }
            if (nCode >= 0 && wParam == (IntPtr)WM_SYSKEYUP)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                int time = Marshal.ReadInt32(lParam + 12);
                Console.WriteLine($"Key: {(Keys)vkCode} - Stamp: {time} SYSKEYUP");
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
    }
}
