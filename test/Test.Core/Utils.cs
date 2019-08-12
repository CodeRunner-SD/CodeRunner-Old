using System;

namespace Test.Core
{
    internal class Utils
    {
        public static string GetShell()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                return "powershell.exe";
            }
            else
            {
                return "bash";
            }
        }

        public static string GetPythonFile()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                return "C:/Python37/python.exe";
            }
            else
            {
                return "/usr/bin/python3";
            }
        }
    }
}
