using System;
using System.Runtime.InteropServices;

class Program
{
    [DllImport("shell32.dll")]
    private static extern int SHAppBarMessage(uint dwMessage, ref APPBARDATA pData);

    [DllImport("user32.dll")]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [StructLayout(LayoutKind.Sequential)]
    private struct APPBARDATA
    {
        public int cbSize;
        public IntPtr hWnd;
        public uint uCallbackMessage;
        public uint uEdge;
        public RECT rc;
        public IntPtr lParam;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int left, top, right, bottom;
    }

    private const int ABM_SETSTATE = 0x0000000A;
    private const int ABM_GETSTATE = 0x00000004;
    private const int ABS_AUTOHIDE = 0x0000001;
    private const int ABS_ALWAYSONTOP = 0x0000002;

    static void Main()
    {
        IntPtr taskbarHandle = FindWindow("Shell_TrayWnd", null);

        if (taskbarHandle != IntPtr.Zero)
        {
            APPBARDATA abd = new APPBARDATA();
            abd.cbSize = Marshal.SizeOf(abd);
            abd.hWnd = taskbarHandle;

            // Obtener el estado actual
            int state = SHAppBarMessage(ABM_GETSTATE, ref abd);

            // Cambiar entre ocultar automáticamente y siempre visible
            if ((state & ABS_AUTOHIDE) != 0)
            {
                // Está en modo autohide, cambiarlo a siempre visible
                abd.lParam = (IntPtr)ABS_ALWAYSONTOP;
            }
            else
            {
                // Está visible, cambiarlo a autohide
                abd.lParam = (IntPtr)ABS_AUTOHIDE;
            }

            // Aplicar el cambio
            SHAppBarMessage(ABM_SETSTATE, ref abd);
        }
    }
}
