using CodeRunner.Executors;
using CodeRunner.Pipelines;
using CodeRunner.Templates;
using System;
using System.Threading.Tasks;

namespace CodeRunner.Operations
{
    public abstract class CommandLineOperation : BaseOperation
    {
        protected abstract Task<CommandLineOperationSettings> GetSettings(ResolveContext context);

        public override async Task<PipelineBuilder<OperationWatcher, bool>> Resolve(ResolveContext context)
        {
            CommandLineOperationSettings settings = await GetSettings(context);
            var shell = string.IsNullOrEmpty(settings.Shell) ? context.GetVariable<string>(OperationVariables.Shell) : settings.Shell;
            var workingDirectory = string.IsNullOrEmpty(settings.WorkingDirectory) ? context.GetVariable<string>(OperationVariables.WorkingDirectory) : settings.WorkingDirectory;
            PipelineBuilder<OperationWatcher, bool> builder = new PipelineBuilder<OperationWatcher, bool>();
            builder.Configure("service", scope =>
            {
                scope.Add<CommandLineOperationSettings>(settings);
            });
            builder.Use("init", context => Task.FromResult(true));
            foreach (CommandLineTemplate item in settings.Scripts)
            {
                CLIExecutorSettings res = new CLIExecutorSettings(shell, new string[]
                {
                    "-c",
                    string.Join(' ', await item.Resolve(context))
                })
                {
                    TimeLimit = TimeSpan.FromSeconds(10),
                    WorkingDirectory = workingDirectory,
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
}
