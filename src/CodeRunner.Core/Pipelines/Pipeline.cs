using CodeRunner.Diagnostics;
using CodeRunner.Loggings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeRunner.Pipelines
{
    public delegate Task<TResult> PipelineOperation<TOrigin, TResult>(PipelineContext<TOrigin, TResult> context) where TResult : class;

    public class Pipeline<TOrigin, TResult> where TResult : class
    {
        public Pipeline(TOrigin origin, ILogger logger, ServiceProvider services, IReadOnlyList<(string, PipelineOperation<TOrigin, TResult>)> ops)
        {
            Assert.IsNotNull(origin);
            Assert.IsNotNull(logger);
            Assert.IsNotNull(services);
            Assert.IsNotNull(ops);

            Origin = origin;
            Logger = logger;
            Services = services;
            Ops = ops;
            Logs = Logger.CreateScope("pipeline", LogLevel.Debug);
        }

        private TOrigin Origin { get; }

        private ILogger Logger { get; }

        private ServiceProvider Services { get; }

        private IReadOnlyList<(string, PipelineOperation<TOrigin, TResult>)> Ops { get; }

        private PipelineStepException? Exception { get; set; }

        private TResult? Result { get; set; } = default;

        public int Position { get; private set; } = 0;

        private LoggerScope Logs { get; set; }

        private bool HasEnd { get; set; } = false;

        public async Task<bool> Step()
        {
            if (Exception != null || Position >= Ops.Count || HasEnd)
            {
                return false;
            }

            (string, PipelineOperation<TOrigin, TResult>) op = Ops[Position];

            Logs.Debug($"Executing {op.Item1} at {Position}.");

            LoggerScope subLogScope = Logger.CreateScope(op.Item1, LogLevel.Debug);

            try
            {
                PipelineContext<TOrigin, TResult> context = new PipelineContext<TOrigin, TResult>(await Services.CreateScope(op.Item1).ConfigureAwait(false), Origin, Result, subLogScope);
                try
                {
                    TResult result = await op.Item2.Invoke(context).ConfigureAwait(false);
                    Result = result;
                }
                catch (PipelineResultIgnoreException)
                {
                }
                catch (Exception ex)
                {
                    throw new PipelineStepException($"Pipeline failed at step {Position}: {op.Item1}", ex);
                }

                if (context.IsStopped)
                {
                    HasEnd = true;
                }
            }
            catch (PipelineStepException ex)
            {
                Exception = ex;
                subLogScope.Error(ex);
                return false;
            }

            Logs.Debug($"Executed {op.Item1} at {Position}.");

            Position++;
            return true;
        }

        public async Task<PipelineResult<TResult>> Consume()
        {
            while (await Step().ConfigureAwait(false))
            {
                ;
            }

            return new PipelineResult<TResult>(Result, Exception, Logger.View().ToArray());
        }
    }
}
