using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Executors
{
    public class CLIExecutor : IDisposable
    {
        ProcessStartInfo StartInfo { get; set; }

        public string Input { get; set; } = "";

        public long? MemoryLimit { get; set; }

        public TimeSpan? TimeLimit { get; set; }

        public Process? Process { get; private set; }

        ExecutorResult? Result { get; set; }

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
                throw new Exception("The process is running.");

            Result = new ExecutorResult();
            Process = new Process { StartInfo = StartInfo, EnableRaisingEvents = true };
        }

        public async Task<ExecutorResult> Run()
        {
            if (Result == null)
                Initialize();
            if (Result!.State != ExecutorState.Pending)
                throw new Exception("Can't run before initialized.");

            Result.State = ExecutorState.Running;
            Result.StartTime = DateTimeOffset.Now;

            Process!.Start();
            var getMemory = Task.Run(() =>
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

            var running = Task.Run(() =>
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
            await Task.Run(() =>
            {
                Result!.ExitCode = Process!.ExitCode;
                Result.EndTime = DateTimeOffset.Now;

                var output = new List<string>();
                while (!Process.StandardOutput.EndOfStream)
                    output.Add(Process.StandardOutput.ReadLine()!);
                Result.Output = output.ToArray();

                var error = new List<string>();
                while (!Process.StandardError.EndOfStream)
                    error.Add(Process.StandardError.ReadLine()!);
                Result.Error = error.ToArray();

                if (Result.State == ExecutorState.Running)
                    Result.State = ExecutorState.Ended;
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
                        Process!.Dispose();
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
