using System;

namespace CodeRunner.Managements.Configurations
{
    public class AppSettings
    {
        public AppSettings()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                DefaultShell = "powershell.exe";
            }
            else
            {
                DefaultShell = "bash";
            }
        }

        public Version Version { get; set; } = new Version();

        public string DefaultShell { get; set; }
    }
}
