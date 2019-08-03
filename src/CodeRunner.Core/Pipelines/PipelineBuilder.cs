using CodeRunner.Loggers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeRunner.Pipelines
{
    public class PipelineBuilder<TOrigin, TResult>
    {
        List<(string, object)> Configures { get; } = new List<(string, object)>();

        List<PipelineOperator<TOrigin, TResult>> Ops { get; } = new List<PipelineOperator<TOrigin, TResult>>();

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

        public PipelineBuilder<TOrigin, TResult> Use(PipelineOperator<TOrigin, TResult> op)
        {
            Ops.Add(op);
            return this;
        }

        public async Task<Pipeline<TOrigin, TResult>> Build(TOrigin origin, Logger logger)
        {
            var services = new ServiceProvider();
            foreach(var (name,func) in Configures)
            {
                switch (func)
                {
                    case Func<ServiceScope, Task> f:
                        await f(await services.CreateScope(name));
                        break;
                    case Action<ServiceScope> a:
                        a(await services.CreateScope(name));
                        break;
                    default:
                        throw new Exception("Not support configure method.");
                }
            }
            return new Pipeline<TOrigin, TResult>(origin, logger, services, Ops.ToArray());
        }
    }
}
