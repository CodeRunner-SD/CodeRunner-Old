using CodeRunner.Commands;
using CodeRunner.Helpers;
using CodeRunner.IO;
using CodeRunner.Managers;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner
{
    public enum EnvironmentType
    {
        Production,
        Develop,
        Test
    }

    public class Program
    {
        public static Workspace Workspace { get; set; }

        public static EnvironmentType Environment { get; set; } = EnvironmentType.Develop;

        public static TextReader Input { get; set; }

        public static IConsole Console { get; set; }

        private static RootCommand RootCommand { get; set; }

        private static TempDirectory? TestDir { get; set; }

        public static void Initialize()
        {
            RootCommand = new RootCommand("Code-runner");
            RootCommand.TreatUnmatchedTokensAsErrors = false;
            RootCommand.AddCommand(new InitCommand().Build());
            RootCommand.AddCommand(new NewCommand().Build());
            RootCommand.AddCommand(new RunCommand().Build());
        }

        public static bool Prompt()
        {
            Console.Out.Write("> ");
            return true;
        }

        public static async Task<int> Main(string[] args)
        {
            switch (Environment)
            {
                case EnvironmentType.Production:
                    Workspace = new Workspace(new DirectoryInfo(Directory.GetCurrentDirectory()));
                    Input = System.Console.In;
                    Console = new SystemConsole();
                    break;
                case EnvironmentType.Develop:
                    Workspace = new Workspace(new DirectoryInfo(Path.Join(Directory.GetCurrentDirectory(), "temp")));
                    Input = System.Console.In;
                    Console = new SystemConsole();
                    Console.Out.WriteLine(Workspace.PathRoot.FullName);
                    break;
                case EnvironmentType.Test:
                    if (Workspace == null)
                    {
                        TestDir = new TempDirectory();
                        Workspace = new Workspace(TestDir.Directory);
                    }
                    if (Input == null)
                    {
                        Input = System.Console.In;
                    }

                    if (Console == null)
                    {
                        Console = new TestConsole();
                    }

                    break;
            }

            Initialize();

            if (args.Length != 0)
            {
                return await RootCommand.InvokeAsync(args, Console);
            }

            while (Prompt() && !Console.IsEndOfInput())
            {
                string? line = Console.InputLine();
                if (line != null)
                {
                    if (line == "quit")
                        break;
                    int exitCode = await RootCommand.InvokeAsync(line, Console);
                    if (exitCode != 0)
                        Console.Error.WriteLine($"Executed with code {exitCode}.");
                }
            }

            if (TestDir != null)
            {
                TestDir.Dispose();
            }

            return 0;
        }
    }
}