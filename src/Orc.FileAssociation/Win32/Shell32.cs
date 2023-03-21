namespace Orc.FileAssociation.Win32;

using System.Runtime.InteropServices;

// Credits:
// https://pinvoke.net/default.aspx/shell32/ShellExecuteEx.html

internal static class Shell32
{
    [DllImport("shell32.dll", CharSet = CharSet.Auto)]
    internal static extern bool ShellExecuteEx(ref SHELLEXECUTEINFO lpExecInfo);

    public static void ShowFileProperties(string fileName)
    {
        var info = new SHELLEXECUTEINFO
        {
            lpVerb = "properties",
            lpFile = fileName,
            nShow = 5,
            fMask = 12
        };

        ShellExecuteEx(ref info);
    }
}
