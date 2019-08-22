using CodeRunner.Loggings;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace CodeRunner.Pipelines
{
    public delegate Task<TResult> PipelineOperation<TOrigin, TResult>(PipelineContext<TOrigin, TResult> context);

    public class Pipeline<TOrigin, TResult>
    {
        public Pipeline(TOrigin origin, ILogger logger, ServiceProvider services, IReadOnlyList<(string, PipelineOperation<TOrigin, TResult>)> ops)
        {
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

        private Exception? Exception { get; set; }

        [MaybeNull]
#pragma warning disable CS8653 // 默认表达式为类型参数引入了 null 值。
        private TResult Result { get; set; } = default;
#pragma warning restore CS8653 // 默认表达式为类型参数引入了 null 值。

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
                PipelineContext<TOrigin, TResult> context = new PipelineContext<TOrigin, TResult>(await Services.CreateScope(op.Item1), Origin, Result, subLogScope);
                TResult result = await op.Item2.Invoke(context);
                if (!context.IgnoreResult)
                {
                    Result = result;
                }

                if (context.IsEnd)
                {
                    HasEnd = true;
                    return false;
                }
            }
            catch (Exception ex)
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
            while (await Step())
            {
                ;
            }

            return new PipelineResult<TResult>(Result, Exception, Logger.View().ToArray());
        }
    }
}
