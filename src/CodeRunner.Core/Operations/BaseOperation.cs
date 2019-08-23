using CodeRunner.Pipelines;
using CodeRunner.Templates;

namespace CodeRunner.Operations
{
    public abstract class BaseOperation : BaseTemplate<PipelineBuilder<OperationWatcher, bool>>
    {

    }

    public class OperationWatcher
    {

    }
}
