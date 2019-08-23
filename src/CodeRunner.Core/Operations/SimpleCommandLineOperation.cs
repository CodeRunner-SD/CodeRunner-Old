using CodeRunner.Executors;
using CodeRunner.Pipelines;
using CodeRunner.Templates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeRunner.Operations
{
    public abstract class CommandLineOperation : BaseOperation
    {
        protected abstract Task<CommandLineOperationSettings> GetSettings(ResolveContext context);

        public override async Task<PipelineBuilder<OperationWatcher, bool>> Resolve(ResolveContext context)
        {
            CommandLineOperationSettings settings = await GetSettings(context);
            PipelineBuilder<OperationWatcher, bool> builder = new PipelineBuilder<OperationWatcher, bool>();
            builder.Configure("service", scope =>
            {
                scope.Add<CommandLineOperationSettings>(settings);
            });
            foreach (CommandLineTemplate item in settings.Scripts)
            {
                CLIExecutorSettings res = new CLIExecutorSettings(settings.Shell ?? "bash", new string[]
                {
                    "-c",
                    string.Join(' ', await item.Resolve(context))
                })
                {
                    TimeLimit = TimeSpan.FromSeconds(10),
                    WorkingDirectory = settings.WorkingDirectory,
                    CollectError = true,
                    CollectOutput = true,
                };

                builder.Use("script", async context =>
                {
                    context.Logs.Debug($"Execute {res.Arguments[1]}");
                    using CLIExecutor exe = new CLIExecutor(res);
                    ExecutorResult result = await exe.Run();
                    if (result.ExitCode != 0)
                    {
                        context.IsEnd = true;
                    }

                    if (!string.IsNullOrEmpty(result.Output))
                    {
                        context.Logs.Information(result.Output);
                    }
                    if (!string.IsNullOrEmpty(result.Error))
                    {
                        context.Logs.Error(result.Error);
                    }

                    context.Logs.Debug($"Executed {res.Arguments[1]}");
                    return result.ExitCode == 0;
                });
            }
            return builder;
        }
    }

    public class SimpleCommandLineOperation : CommandLineOperation
    {
        public SimpleCommandLineOperation(IList<CommandLineTemplate>? items = null)
        {
            Items = items ?? new List<CommandLineTemplate>();
        }

        public SimpleCommandLineOperation() : this(null)
        {
        }

        public IList<CommandLineTemplate> Items { get; }

        public BaseOperation Use(CommandLineTemplate command)
        {
            Items.Add(command);
            return this;
        }

        public override VariableCollection GetVariables()
        {
            VariableCollection res = base.GetVariables();
            res.Add(OperationVariables.Shell);
            res.Add(OperationVariables.WorkingDirectory);
            foreach (CommandLineTemplate v in Items)
            {
                res.Collect(v);
            }

            return res;
        }

        protected override Task<CommandLineOperationSettings> GetSettings(ResolveContext context)
        {
            CommandLineOperationSettings res = new CommandLineOperationSettings
            {
                Shell = context.GetVariable<string>(OperationVariables.Shell),
                WorkingDirectory = context.GetVariable<string>(OperationVariables.WorkingDirectory)
            };
            foreach (CommandLineTemplate v in Items)
            {
                res.Scripts.Add(v);
            }
            return Task.FromResult(res);
        }
    }
}
