using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace BestAppearance
{
    internal class Program
    {
        private const uint SPIF_UPDATEINIFILE = 0x01;
        private const uint SPIF_SENDCHANGE = 0x02;

        private const uint SPI_SETDRAGFULLWINDOWS = 0x0025;
        private const uint SPI_SETFONTSMOOTHING = 0x004B;
        private const uint SPI_SETFONTSMOOTHINGTYPE = 0x200B;
        private const uint FE_FONTSMOOTHINGCLEARTYPE = 0x0002;
        private const uint SPI_SETMENUANIMATION = 0x1003;
        private const uint SPI_SETCOMBOBOXANIMATION = 0x1005;
        private const uint SPI_SETLISTBOXSMOOTHSCROLLING = 0x1007;
        private const uint SPI_SETGRADIENTCAPTIONS = 0x1009;
        private const uint SPI_SETKEYBOARDCUES = 0x100B;
        private const uint SPI_SETHOTTRACKING = 0x100F;
        private const uint SPI_SETMENUFADE = 0x1013;
        private const uint SPI_SETSELECTIONFADE = 0x1015;
        private const uint SPI_SETTOOLTIPANIMATION = 0x1017;
        private const uint SPI_SETTOOLTIPFADE = 0x1019;
        private const uint SPI_SETCURSORSHADOW = 0x101B;
        private const uint SPI_SETUIEFFECTS = 0x103F;
        private const uint SPI_SETDISABLEOVERLAPPEDCONTENT = 0x1041;
        private const uint SPI_SETCLIENTAREAANIMATION = 0x1043;

        private const int HWND_BROADCAST = 0xffff;
        private const uint WM_SETTINGCHANGE = 0x001A;
        private const uint SMTO_ABORTIFHUNG = 0x0002;

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SystemParametersInfo(
            uint uiAction, uint uiParam, IntPtr pvParam, uint fWinIni);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr SendMessageTimeout(
            IntPtr hWnd, uint msg, IntPtr wParam, string lParam,
            uint flags, uint timeout, out IntPtr result);

        [STAThread]
        private static void Main()
        {
            // These are the persisted values used by Performance Options.
            using (RegistryKey visualEffects = Registry.CurrentUser.CreateSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects"))
            {
                visualEffects.SetValue("VisualFXSetting", 1, RegistryValueKind.DWord);
            }

            using (RegistryKey desktop = Registry.CurrentUser.OpenSubKey(
                @"Control Panel\Desktop", true))
            {
                desktop.SetValue("DragFullWindows", "1", RegistryValueKind.String);
                desktop.SetValue("FontSmoothing", "2", RegistryValueKind.String);
                desktop.SetValue("FontSmoothingType", 2, RegistryValueKind.DWord);
            }

            using (RegistryKey metrics = Registry.CurrentUser.OpenSubKey(
                @"Control Panel\Desktop\WindowMetrics", true))
            {
                metrics.SetValue("MinAnimate", "1", RegistryValueKind.String);
            }

            // For BOOL-based SPI_SET actions, the value belongs in uiParam.
            SetPointerBool(SPI_SETUIEFFECTS, true);
            SetPointerBool(SPI_SETCLIENTAREAANIMATION, true);
            Set(SPI_SETDRAGFULLWINDOWS, true);
            Set(SPI_SETFONTSMOOTHING, true);
            SetValue(SPI_SETFONTSMOOTHINGTYPE, FE_FONTSMOOTHINGCLEARTYPE);
            SetPointerBool(SPI_SETMENUANIMATION, true);
            SetPointerBool(SPI_SETCOMBOBOXANIMATION, true);
            SetPointerBool(SPI_SETLISTBOXSMOOTHSCROLLING, true);
            SetPointerBool(SPI_SETGRADIENTCAPTIONS, true);
            SetPointerBool(SPI_SETKEYBOARDCUES, true);
            SetPointerBool(SPI_SETHOTTRACKING, true);
            SetPointerBool(SPI_SETMENUFADE, true);
            SetPointerBool(SPI_SETSELECTIONFADE, true);
            SetPointerBool(SPI_SETTOOLTIPANIMATION, true);
            SetPointerBool(SPI_SETTOOLTIPFADE, true);
            SetPointerBool(SPI_SETCURSORSHADOW, true);
            SetPointerBool(SPI_SETDISABLEOVERLAPPEDCONTENT, false);

            IntPtr ignored;
            SendMessageTimeout(
                new IntPtr(HWND_BROADCAST), WM_SETTINGCHANGE, IntPtr.Zero,
                @"Control Panel\Desktop", SMTO_ABORTIFHUNG, 5000, out ignored);
        }

        private static void Set(uint action, bool enabled)
        {
            if (!SystemParametersInfo(
                action,
                enabled ? 1u : 0u,
                IntPtr.Zero,
                SPIF_UPDATEINIFILE | SPIF_SENDCHANGE))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        private static void SetValue(uint action, uint value)
        {
            if (!SystemParametersInfo(
                action,
                0,
                new IntPtr(value),
                SPIF_UPDATEINIFILE | SPIF_SENDCHANGE))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }
        private static void SetPointerBool(uint action, bool enabled)
        {
            int value = enabled ? 1 : 0;
            IntPtr pointer = Marshal.AllocHGlobal(sizeof(int));

            try
            {
                Marshal.WriteInt32(pointer, value);

                if (!SystemParametersInfo(
                    action,
                    0,
                    pointer,
                    SPIF_UPDATEINIFILE | SPIF_SENDCHANGE))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            finally
            {
                Marshal.FreeHGlobal(pointer);
            }
        }
    }
}



