using System;
using System.Runtime.InteropServices;
using System.Text;
using DragonAge2CameraTools.ProcessMemoryAccess.Interfaces;

namespace DragonAge2CameraTools.ProcessMemoryAccess
{
    public class ActiveWindowService : IActiveWindowService
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
        
        public string GetActiveWindowTitle()
        {
            const int expectedMaxCharacterCount = 256;
            var buffer = new StringBuilder(expectedMaxCharacterCount);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, buffer, expectedMaxCharacterCount) > 0)
            {
                return buffer.ToString();
            }
            
            return null;
        }
        
        public IntPtr GetActiveWindowHandle()
        {
            IntPtr handle = GetForegroundWindow();
            return handle;
        }
    }
}