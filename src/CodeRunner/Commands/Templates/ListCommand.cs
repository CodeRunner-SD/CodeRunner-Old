using CodeRunner.Managers;
using CodeRunner.Managers.Configurations;
using CodeRunner.Pipelines;
using CodeRunner.Rendering;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.Threading;
using System.Threading.Tasks;
using SettingItem = System.Collections.Generic.KeyValuePair<string, CodeRunner.Managers.Configurations.TemplateItem>;

namespace CodeRunner.Commands.Templates
{
    public class ListCommand : BaseCommand<ListCommand.CArgument>
    {
        public override Command Configure()
        {
            Command res = new Command("list", "List all.");
            return res;
        }

        public override async Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, OperationContext operation, CancellationToken cancellationToken)
        {
            ITerminal terminal = console.GetTerminal();
            Workspace workspace = operation.Services.Get<Workspace>();
            TemplatesSettings res = await workspace.Templates.Settings;
            if (res != null)
            {
                List<(Func<SettingItem, int>, Action<ITerminal, SettingItem, int>)> funcs = new List<(Func<SettingItem, int>, Action<ITerminal, SettingItem, int>)>();
                {
                    static void render(ITerminal ter, SettingItem source, int len)
                    {
                        ter.Output(source.Value.Type.ToString().PadRight(len));
                    }
                    funcs.Add((source => source.Value.Type.ToString()?.Length ?? 0, render));
                }
                {
                    static void render(ITerminal ter, SettingItem source, int len)
                    {
                        ter.OutputBold(source.Key.PadRight(len));
                    }
                    funcs.Add((source => source.Key.Length, render));
                }
                {
                    static void render(ITerminal ter, SettingItem source, int len)
                    {
                        ter.Output(source.Value.FileName.PadRight(len));
                    }
                    funcs.Add((source => source.Value.FileName.Length, render));
                }
                terminal.OutputTable(res.Items, funcs.ToArray());
            }
            return 0;
        }

        public class CArgument
        {
        }
    }
}
