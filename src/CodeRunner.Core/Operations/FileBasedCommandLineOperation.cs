using CodeRunner.Diagnostics;
using CodeRunner.IO;
using CodeRunner.Templates;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Operations
{
    public class FileBasedCommandLineOperationSettings
    {
        public IList<string> Scripts { get; } = new List<string>();

        public string WorkingDirectory { get; set; } = string.Empty;

        public string Shell { get; set; } = string.Empty;
    }

    public class FileBasedCommandLineOperation : CommandLineOperation
    {
        public const string DefaultFileName = "settings.json";

        public string FileName { get; set; } = DefaultFileName;

        protected override async Task<CommandLineOperationSettings> GetSettings(ResolveContext context)
        {
            Assert.IsNotNull(context);

            string inputPath = context.GetInputPath();
            string workingDir = context.GetWorkingDirectory();
            string path = Path.Join(workingDir, inputPath, FileName);
            using FileStream st = File.Open(path, FileMode.Open, FileAccess.Read);
            FileBasedCommandLineOperationSettings settings = await JsonFormatter.Deserialize<FileBasedCommandLineOperationSettings>(st).ConfigureAwait(false);
            CommandLineOperationSettings res = new CommandLineOperationSettings
            {
                WorkingDirectory = settings.WorkingDirectory,
                Shell = settings.Shell
            };
            foreach (string v in settings.Scripts)
            {
                CommandLineTemplate item = new CommandLineTemplate
                {
                    Raw = v
                };
                res.Scripts.Add(item);
            }
            return res;
        }

        public static DirectoryTemplate GetDirectoryTemplate(string fileName = DefaultFileName)
        {
            Assert.IsNotNull(fileName);

            FileBasedCommandLineOperationSettings settings = new FileBasedCommandLineOperationSettings();
            PackageDirectoryTemplate res = new PackageDirectoryTemplate(new StringTemplate(StringTemplate.GetVariableTemplate("name"),
                new Variable[] { new Variable("name").Required() }));
            return res.AddFile(fileName)
                .UseTemplate(new TextFileTemplate(new StringTemplate(JsonFormatter.Serialize(settings, new Newtonsoft.Json.JsonSerializerSettings())))); ;
        }
    }
}
