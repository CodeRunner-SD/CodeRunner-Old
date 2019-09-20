using CodeRunner.Loggings;
using CodeRunner.Managements.Configurations;
using CodeRunner.Operations;
using CodeRunner.Pipelines;
using CodeRunner.Templates;
using System;
using System.Threading.Tasks;

namespace CodeRunner.Managements
{
    public interface IWorkspace : IManager<WorkspaceSettings>
    {
        ITemplateManager Templates { get; }

        IOperationManager Operations { get; }

        // register when template is null
        Task<IWorkItem?> Create(string name, BaseTemplate? template, Func<VariableCollection, ResolveContext, Task> resolveCallback);

        Task<PipelineResult<bool>> Execute(IWorkItem? workItem, BaseOperation operation, Func<VariableCollection, ResolveContext, Task> resolveCallback, OperationWatcher watcher, ILogger logger);
    }
}
