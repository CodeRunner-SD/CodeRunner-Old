using CodeRunner.IO;
using CodeRunner.Templates;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Operations
{
    public class FileBasedCommandLineOperation : CommandLineOperation
    {
        public class Settings
        {
            public IList<string> Scripts { get; set; } = new List<string>();

            public string WorkingDirectory { get; set; } = string.Empty;

            public string Shell { get; set; } = string.Empty;
        }

        public const string DefaultFileName = "settings.json";

        public string FileName { get; set; } = DefaultFileName;

        protected override async Task<CommandLineOperationSettings> GetSettings(ResolveContext context)
        {
            string inputPath = context.GetVariable<string>(OperationVariables.InputPath);
            string workingDir = context.GetVariable<string>(OperationVariables.WorkingDirectory);
            string path = Path.Join(workingDir, inputPath, FileName);
            using FileStream st = File.Open(path, FileMode.Open, FileAccess.Read);
            var settings = await JsonFormatter.Deserialize<Settings>(st);
            var res = new CommandLineOperationSettings
            {
                WorkingDirectory = settings.WorkingDirectory,
                Shell = settings.Shell
            };
            foreach (var  v in settings.Scripts)
            {
                var item = new CommandLineTemplate
                {
                    Raw = v
                };
                res.Scripts.Add(item);
            }
            return res;
        }

        public static DirectoryTemplate GetDirectoryTemplate(string fileName = DefaultFileName)
        {
            var settings = new Settings();
            var res = new PackageDirectoryTemplate(new StringTemplate(StringTemplate.GetVariableTemplate("name"),
                new Variable[] { new Variable("name").Required() }));
            res.AddFile(fileName).UseTemplate(new TextFileTemplate(new StringTemplate(JsonFormatter.Serialize(settings, new Newtonsoft.Json.JsonSerializerSettings()))));
            return res;
        }
    }
}
