using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Executors
{
    public class CLIExecutor : IDisposable
    {
        private ProcessStartInfo StartInfo { get; set; }

        public string Input { get; set; } = "";

        public long? MemoryLimit { get; set; }

        public TimeSpan? TimeLimit { get; set; }

        public Process? Process { get; private set; }

        private ExecutorResult? Result { get; set; }

        public CLIExecutor(ProcessStartInfo startInfo)
        {
            StartInfo = startInfo;
            StartInfo.UseShellExecute = false;
            StartInfo.RedirectStandardError = true;
            StartInfo.RedirectStandardInput = true;
            StartInfo.RedirectStandardOutput = true;
        }

        public void Initialize()
        {
            if (Result != null && Result.State == ExecutorState.Running)
            {
                throw new Exception("The process is running.");
            }

            Result = new ExecutorResult();
            Process = new Process { StartInfo = StartInfo, EnableRaisingEvents = true };
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
                        Result.MaximumMemory = Math.Max(Result.MaximumMemory, Math.Max(Process.PagedMemorySize64, Process.PeakPagedMemorySize64));
                        if (MemoryLimit.HasValue && Result.MaximumMemory > MemoryLimit)
                        {
                            Result.State = ExecutorState.OutOfMemory;
                            Process.Kill();
                        }
                        Thread.Sleep(5);
                    }
                    catch { }
                }
            });

            {
                await Process.StandardInput.WriteLineAsync(Input);
                Process.StandardInput.Close();
            }

            Task running = Task.Run(() =>
            {
                if (TimeLimit.HasValue)
                {
                    if (Process.WaitForExit((int)Math.Ceiling(TimeLimit.Value.TotalMilliseconds)))
                    {
                    }
                    else
                    {
                        Result.State = ExecutorState.OutOfTime;
                        Process.Kill();
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

                Result.Output = await Process.StandardOutput.ReadToEndAsync();
                Result.Error = await Process.StandardError.ReadToEndAsync();

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
