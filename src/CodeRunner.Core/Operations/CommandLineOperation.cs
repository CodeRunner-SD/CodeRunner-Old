using CodeRunner.Diagnostics;
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

        public override async Task<PipelineBuilder<OperationWatcher, Wrapper<bool>>> Resolve(ResolveContext context)
        {
            Assert.IsNotNull(context);

            CommandLineOperationSettings settings = await GetSettings(context).ConfigureAwait(false);
            string shell = string.IsNullOrEmpty(settings.Shell) ? context.GetShell() : settings.Shell;
            string workingDirectory = string.IsNullOrEmpty(settings.WorkingDirectory) ? context.GetWorkingDirectory() : settings.WorkingDirectory;
            PipelineBuilder<OperationWatcher, Wrapper<bool>> builder = new PipelineBuilder<OperationWatcher, Wrapper<bool>>();
            _ = builder.Configure("service", scope => scope.Add<CommandLineOperationSettings>(settings))
                .Use("init", context => Task.FromResult<Wrapper<bool>>(true));
            foreach (CommandLineTemplate item in settings.Scripts)
            {
                CLIExecutorSettings res = new CLIExecutorSettings(shell, new string[]
                {
                    "-c",
                    string.Join(' ', await item.Resolve(context).ConfigureAwait(false))
                })
                {
                    TimeLimit = TimeSpan.FromSeconds(10),
                    WorkingDirectory = workingDirectory,
                    CollectError = true,
                    CollectOutput = true,
                };

                _ = builder.Use("script", async context =>
                {
                    context.Logs.Debug($"Execute {res.Arguments[1]}");
                    using CLIExecutor exe = new CLIExecutor(res);
                    ExecutorResult result = await exe.Run().ConfigureAwait(false);
                    if (result.ExitCode != 0)
                    {
                        context.IsStopped = true;
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
