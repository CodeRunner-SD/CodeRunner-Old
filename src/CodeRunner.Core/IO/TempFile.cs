using CodeRunner.Diagnostics;
using System;
using System.IO;

namespace CodeRunner.IO
{
    public class TempFile : IDisposable
    {
        public TempFile(string ext = "")
        {
            Assert.IsNotNull(ext);

            string path = Path.Join(Path.GetTempPath(), Path.GetRandomFileName());
            if (!string.IsNullOrEmpty(ext))
            {
                path = $"{path}.{ext}";
            }

            File = new FileInfo(path);
            File.Create().Close();

            File.Refresh();
        }

        public FileInfo File { get; }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // 释放托管状态(托管对象)。
                }

                // 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // 将大型字段设置为 null。
                if (File.Exists)
                {
                    File.Delete();
                }

                disposedValue = true;
            }
        }

        // 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        ~TempFile()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(false);
        }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // 如果在以上内容中替代了终结器，则取消注释以下行。
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
