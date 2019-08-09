using CodeRunner.Loggings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeRunner.Pipelines
{
    public class PipelineBuilder<TOrigin, TResult>
    {
        private List<(string, object)> Configures { get; } = new List<(string, object)>();

        private List<(string, PipelineOperator<TOrigin, TResult>)> Ops { get; } = new List<(string, PipelineOperator<TOrigin, TResult>)>();

        public PipelineBuilder<TOrigin, TResult> Configure(string name, Func<ServiceScope, Task> func)
        {
            Configures.Add((name, func));
            return this;
        }

        public PipelineBuilder<TOrigin, TResult> Configure(string name, Action<ServiceScope> func)
        {
            Configures.Add((name, func));
            return this;
        }

        public PipelineBuilder<TOrigin, TResult> Use(string name, PipelineOperator<TOrigin, TResult> op)
        {
            Ops.Add((name, op));
            return this;
        }

        public async Task<Pipeline<TOrigin, TResult>> Build(TOrigin origin, Logger logger)
        {
            ServiceProvider services = new ServiceProvider();
            foreach ((string name, object func) in Configures)
            {
                switch (func)
                {
                    case Func<ServiceScope, Task> f:
                        await f(await services.CreateScope(name));
                        break;
                    case Action<ServiceScope> a:
                        a(await services.CreateScope(name));
                        break;
                }
            }
            return new Pipeline<TOrigin, TResult>(origin, logger, services, Ops.ToArray());
        }
    }
}
