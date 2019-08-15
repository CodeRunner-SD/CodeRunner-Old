using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Executors
{
    public class CLIExecutor : IDisposable
    {
        private Process? Process { get; set; }

        private ExecutorResult? Result { get; set; }

        private CLIExecutorSettings Settings { get; set; }

        public CLIExecutor(CLIExecutorSettings settings)
        {
            Settings = settings;
        }

        public void Initialize()
        {
            if (Result != null && Result.State == ExecutorState.Running)
            {
                throw new Exception("The process is running.");
            }

            Result = new ExecutorResult();
            Process = new Process { StartInfo = Settings.CreateStartInfo(), EnableRaisingEvents = true };
        }

        public void Kill()
        {
            Process?.Kill(true);
        }

        public async Task<ExecutorResult> Run()
        {
            if (Result == null)
            {
                Initialize();
            }

            if (Result!.State != ExecutorState.Pending)
            {
                throw new Exception("Can't run before initialized.");
            }

            Result.State = ExecutorState.Running;
            Result.StartTime = DateTimeOffset.Now;

            Process!.Start();
            Task getMemory = Task.Run(() =>
            {
                Result!.MaximumMemory = 0;
                while (Result.State == ExecutorState.Running && Process?.HasExited == false)
                {
                    try
                    {
                        long mem = Math.Max(Process.PagedMemorySize64, Process.PeakPagedMemorySize64);
                        mem = Math.Max(mem, Process.WorkingSet64);
                        mem = Math.Max(mem, Process.PeakWorkingSet64);
                        mem = Math.Max(mem, Process.PrivateMemorySize64);
                        Result.MaximumMemory = Math.Max(Result.MaximumMemory, mem);
                        if (Settings.MemoryLimit.HasValue && Result.MaximumMemory > Settings.MemoryLimit)
                        {
                            Result.State = ExecutorState.OutOfMemory;
                            Process.Kill(true);
                        }
                        Thread.Sleep(5);
                    }
                    catch { }
                }
            });

            if (!string.IsNullOrEmpty(Settings.Input))
            {
                await Process.StandardInput.WriteAsync(Settings.Input);
                Process.StandardInput.Close();
            }

            Task running = Task.Run(() =>
            {
                if (Settings.TimeLimit.HasValue)
                {
                    if (Process.WaitForExit((int)Math.Ceiling(Settings.TimeLimit.Value.TotalMilliseconds)))
                    {
                    }
                    else
                    {
                        Result.State = ExecutorState.OutOfTime;
                        Process.Kill(true);
                        Process.WaitForExit();
                    }
                }
                else
                {
                    Process.WaitForExit();
                }
            });

            await Task.WhenAll(running, getMemory);
            await Task.Run(async () =>
            {
                Result!.ExitCode = Process!.ExitCode;
                Result.EndTime = DateTimeOffset.Now;

                if (Settings.CollectOutput)
                {
                    Result.Output = await Process.StandardOutput.ReadToEndAsync();
                }

                if (Settings.CollectError)
                {
                    Result.Error = await Process.StandardError.ReadToEndAsync();
                }

                if (Result.State == ExecutorState.Running)
                {
                    Result.State = ExecutorState.Ended;
                }
            });

            Process.Dispose();

            return Result;
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Process != null)
                    {
                        Process.Dispose();
                    }
                }

                disposedValue = true;
            }
        }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
        }
        #endregion
    }
}
