using System;
using System.IO;

namespace CodeRunner.IO
{
    public class TempDirectory : IDisposable
    {
        public TempDirectory()
        {
            Directory = new DirectoryInfo(Path.Join(Path.GetTempPath(), Path.GetRandomFileName()));
        }

        public DirectoryInfo Directory { get; }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Directory.Delete(true);
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
