using CodeRunner.Diagnostics;
using CodeRunner.Loggings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeRunner.Pipelines
{
    public class PipelineBuilder<TOrigin, TResult> where TResult : class
    {
        private List<(string, object)> Configures { get; } = new List<(string, object)>();

        private List<(string, PipelineOperation<TOrigin, TResult>)> Ops { get; } = new List<(string, PipelineOperation<TOrigin, TResult>)>();

        public PipelineBuilder<TOrigin, TResult> Configure(string name, Func<ServiceScope, Task> func)
        {
            Assert.IsNotNull(name);
            Assert.IsNotNull(func);

            Configures.Add((name, func));
            return this;
        }

        public PipelineBuilder<TOrigin, TResult> Configure(string name, Action<ServiceScope> func)
        {
            Assert.IsNotNull(name);
            Assert.IsNotNull(func);

            Configures.Add((name, func));
            return this;
        }

        public PipelineBuilder<TOrigin, TResult> Use(string name, PipelineOperation<TOrigin, TResult> op)
        {
            Assert.IsNotNull(name);
            Assert.IsNotNull(op);

            Ops.Add((name, op));
            return this;
        }

        public async Task<Pipeline<TOrigin, TResult>> Build(TOrigin origin, ILogger logger)
        {
            Assert.IsNotNull(origin);
            Assert.IsNotNull(logger);

            ServiceProvider services = new ServiceProvider();
            foreach ((string name, object func) in Configures)
            {
                switch (func)
                {
                    case Func<ServiceScope, Task> f:
                        await f(await services.CreateScope(name).ConfigureAwait(false)).ConfigureAwait(false);
                        break;
                    case Action<ServiceScope> a:
                        a(await services.CreateScope(name).ConfigureAwait(false));
                        break;
                }
            }
            return new Pipeline<TOrigin, TResult>(origin, logger, services, Ops.ToArray());
        }
    }
}
