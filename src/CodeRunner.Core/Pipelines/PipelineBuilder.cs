using CodeRunner.Loggers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeRunner.Pipelines
{
    public class PipelineBuilder<TOrigin, TResult>
    {
        ServiceProvider Services { get; } = new ServiceProvider();

        List<PipelineOperator<TOrigin, TResult>> Ops { get; } = new List<PipelineOperator<TOrigin, TResult>>();

        public async Task Configure(string name, Func<ServiceScope, Task> func)
        {
            await func.Invoke(await Services.CreateScope(name));
        }

        public async Task Configure(string name, Action<ServiceScope> func)
        {
            func.Invoke(await Services.CreateScope(name));
        }

        public void Use(PipelineOperator<TOrigin, TResult> op)
        {
            Ops.Add(op);
        }

        public Task<Pipeline<TOrigin, TResult>> Build(TOrigin origin, Logger logger)
        {
            return Task.FromResult(new Pipeline<TOrigin, TResult>(origin, logger, Services, Ops));
        }
    }
}
