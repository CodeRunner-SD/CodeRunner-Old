using System;

namespace CodeRunner.Managements.Configurations
{
    public class WorkspaceSettings
    {
        public WorkspaceSettings()
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
