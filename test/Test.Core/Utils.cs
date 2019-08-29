using System;

namespace Test.Core
{
    internal class Utils
    {
        public static string GetShell() => Environment.OSVersion.Platform == PlatformID.Win32NT ? "powershell.exe" : "bash";
    }
}
