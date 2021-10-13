namespace Orc.FileAssociation.Win32
{
    using System;
    using System.Runtime.InteropServices;

    //Credits:
    //https://pinvoke.net/default.aspx/shell32/ShellExecuteEx.html

    public static class Shell32
    {
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        internal static extern bool ShellExecuteEx(ref SHELLEXECUTEINFO lpExecInfo);

        public static void ShowFileProperties(string fileName)
        {
            SHELLEXECUTEINFO info = new SHELLEXECUTEINFO();
            info.cbSize = Marshal.SizeOf(info);
            info.lpVerb = "properties";
            info.lpFile = fileName;
            info.nShow = 5;
            info.fMask = 12;
            ShellExecuteEx(ref info);
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct SHELLEXECUTEINFO
        {
            public int cbSize;
            public uint fMask;
            public IntPtr hwnd;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpVerb;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpFile;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpParameters;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpDirectory;
            public int nShow;
            public IntPtr hInstApp;
            public IntPtr lpIDList;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpClass;
            public IntPtr hkeyClass;
            public uint dwHotKey;
            public IntPtr hIcon;
            public IntPtr hProcess;
        }
    }
}
