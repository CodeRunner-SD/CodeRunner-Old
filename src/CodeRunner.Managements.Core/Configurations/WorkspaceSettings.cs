using System;

namespace CodeRunner.Managements.Configurations
{
    public class WorkspaceSettings
    {
        public WorkspaceSettings() => DefaultShell = Environment.OSVersion.Platform == PlatformID.Win32NT ? "powershell.exe" : "bash";

        public Version Version { get; set; } = new Version();

        public string DefaultShell { get; set; }
    }
}
