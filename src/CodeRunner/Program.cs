using CodeRunner.Commands;
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
            RootCommand.AddCommand(new InitCommand().Build());
            RootCommand.AddCommand(new NewCommand().Build());
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
                    TestDir = new TempDirectory();
                    Workspace = new Workspace(TestDir.Directory);
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

            while (Prompt())
            {
                if (Environment == EnvironmentType.Test && Program.Input.Peek() == -1)
                {
                    break;
                }

                string? line = Program.Input.ReadLine();
                if (line != null)
                {
                    if (line == "quit")
                    {
                        break;
                    }
                    _ = await RootCommand.InvokeAsync(line, Console);
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