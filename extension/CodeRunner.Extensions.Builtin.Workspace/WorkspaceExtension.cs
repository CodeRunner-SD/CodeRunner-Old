using CodeRunner.Extensions;
using System;

[assembly: EntryExtension(typeof(CodeRunner.Extensions.Builtin.Workspace.WorkspaceExtension))]

namespace CodeRunner.Extensions.Builtin.Workspace
{
    public class WorkspaceExtension : IExtension
    {
        public string Name => "Workspace";

        public string Publisher => "CodeRunner";

        public string Description => "Add core generic workspace commands.";

        public Version Version => new Version(1, 0);
    }
}
