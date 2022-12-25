using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Better.Extensions.Runtime
{
    /// <summary>
    /// This class allows to load file directly to file without loading file into memory.
    /// <remarks>
    /// Useful for preloading AssetBundles without them automatically being uncompressed and loaded into memory.
    /// </remarks>
    /// </summary>
    public class DownloadHandlerFile : DownloadHandlerScript
    {
        public ulong ContentLength => _received > _contentLength ? _received : _contentLength;

        private ulong _contentLength;
        private ulong _received;
        private FileStream _stream;

        public DownloadHandlerFile(string localFilePath, int bufferSize = 4096,
            FileShare fileShare = FileShare.ReadWrite) : base(new byte[bufferSize])
        {
            var directory = Path.GetDirectoryName(localFilePath);
            if (directory != null && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            _contentLength = 0;
            _received = 0;
            _stream = new FileStream(localFilePath, FileMode.OpenOrCreate, FileAccess.Write, fileShare, bufferSize);
        }

        protected override float GetProgress()
        {
            return ContentLength <= 0 ? 0 : Mathf.Clamp01(_received / (float)ContentLength);
        }

        [Obsolete("Use ReceiveContentLengthHeader")]
        protected override void ReceiveContentLength(int contentLength)
        {
            _contentLength = (ulong)contentLength;
        }

        protected override void ReceiveContentLengthHeader(ulong contentLength)
        {
            _contentLength = contentLength;
            base.ReceiveContentLengthHeader(contentLength);
        }

        protected override bool ReceiveData(byte[] dataBytes, int dataLength)
        {
            if (dataBytes == null || dataBytes.Length == 0) return false;

            _received += (ulong)dataLength;
            _stream.Write(dataBytes, 0, dataLength);

            return true;
        }

        protected override void CompleteContent()
        {
            CloseStream();
        }
#if UNITY_2021_3_OR_NEWER
        public override void Dispose()
        {
            CloseStream();
            base.Dispose();
        }
#else
        ~DownloadHandlerFile()
        {
            CloseStream();
        }
#endif
        private void CloseStream()
        {
            if (_stream == null) return;
            _stream.Dispose();
            _stream = null;
        }
    }
}