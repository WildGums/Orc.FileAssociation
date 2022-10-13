namespace Orc.FileAssociation.Win32
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct SHELLEXECUTEINFO
    {
        private static readonly SHELLEXECUTEINFO DefaultInstance = new SHELLEXECUTEINFO();

        public SHELLEXECUTEINFO()
        {
            cbSize = Marshal.SizeOf(DefaultInstance);
            fMask = default;
            hwnd = default;
            lpVerb = default;
            lpFile = default;
            lpParameters = default;
            lpDirectory = default;
            nShow = default;
            hInstApp = default;
            lpIDList = default;
            lpClass = default;
            hkeyClass = default;
            dwHotKey = default;
            hIcon = default;
            hProcess = default;
        }

        public int cbSize;
        public uint fMask;
        public IntPtr hwnd;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string? lpVerb;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string? lpFile;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string? lpParameters;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string? lpDirectory;
        public int nShow;
        public IntPtr hInstApp;
        public IntPtr lpIDList;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string? lpClass;
        public IntPtr hkeyClass;
        public uint dwHotKey;
        public IntPtr hIcon;
        public IntPtr hProcess;
    }
}
