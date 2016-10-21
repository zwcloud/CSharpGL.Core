using System.Runtime.InteropServices;

namespace System
{
    //https://blez.wordpress.com/2012/09/17/determine-os-with-netmono/
    // CurrentOS Class by blez
    // Detects the current OS (Windows, Linux, MacOS)
    //
    public static class CurrentOS
    {
        static CurrentOS()
        {
            IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            if (IsWindows) return;
            IsMac = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            if (IsMac) return;
            IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            if (IsLinux) return;

            IsUnknown = true;
        }

        public static bool IsWindows { get; private set; }
        public static bool IsMac { get; private set; }
        public static bool IsLinux { get; private set; }
        public static bool IsUnknown { get; private set; }

        public static bool Is64BitProcess
        {
            get { return (IntPtr.Size == 8); }
        }

        public static bool Is32BitProcess
        {
            get { return (IntPtr.Size == 4); }
        }
    }
}