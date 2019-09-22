using CodeRunner.Pipelines;
using CodeRunner.Templates;

namespace CodeRunner.Operations
{
    public abstract class BaseOperation : BaseTemplate<PipelineBuilder<OperationWatcher, Wrapper<bool>>>
    {

    }

    public class OperationWatcher
    {

    }
}
