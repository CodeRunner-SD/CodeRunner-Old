using CodeRunner.Templates;
using System.Collections.Generic;

namespace CodeRunner.Operations
{
    public class CommandLineOperationSettings
    {
        public string Shell { get; set; } = string.Empty;

        public string WorkingDirectory { get; set; } = string.Empty;

        public IList<CommandLineTemplate> Scripts { get; } = new List<CommandLineTemplate>();
    }
}
