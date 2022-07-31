using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace BuildNotification.BetterExtension.Runtime.Extension
{
    public static class SerializeExtensions
    {
        /// <summary>
        /// Deserialize bytes to type with using <see cref="BinaryFormatter"/>
        /// </summary>
        /// <param name="bytes"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Deserialize<T>(this byte[] bytes) where T : class
        {
            using (var stream = new MemoryStream())
            {
                var binFormatter = new BinaryFormatter();

                stream.Write(bytes, 0, bytes.Length);
                stream.Position = 0;

                return binFormatter.Deserialize(stream) as T;
            }
        }

        /// <summary>
        /// Serialize type to bytes with using <see cref="BinaryFormatter"/>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] Serialize(this object obj)
        {
            var binFormatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                binFormatter.Serialize(stream, obj);

                return stream.ToArray();
            }
        }

        /// <summary>
        /// Async version of <see cref="Deserialize{T}"/>
        /// </summary>
        /// <param name="bytes"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<T> DeserializeAsync<T>(this byte[] bytes) where T : class
        {
            await using (var stream = new MemoryStream())
            {
                var binFormatter = new BinaryFormatter();
                await stream.WriteAsync(bytes, 0, bytes.Length);
                stream.Position = 0;

                return binFormatter.Deserialize(stream) as T;
            }
        }

        /// <summary>
        /// Async version of <see cref="Serialize"/>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static async Task<byte[]> SerializeAsync(this object obj)
        {
            return await Task.Factory.StartNew(() => Serialize(obj));
        }

        public static byte[] CompressGZip(byte[] bytes)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
                {
                    gzipStream.Write(bytes, 0, bytes.Length);
                }

                return memoryStream.ToArray();
            }
        }

        public static byte[] DecompressGZip(byte[] bytes)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
                {
                    gzipStream.Write(bytes, 0, bytes.Length);
                }

                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Compressing bytes
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<byte[]> CompressGZipAsync(byte[] data)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
                {
                    await gzipStream.WriteAsync(data, 0, data.Length);
                }

                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Decompressing bytes
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<byte[]> DecompressGZipAsync(byte[] data)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
                {
                    await gzipStream.WriteAsync(data, 0, data.Length);
                }

                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Compressing bytes
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] CompressDeflate(this byte[] data)
        {
            using (var output = new MemoryStream())
            {
                using (var stream = new DeflateStream(output, CompressionLevel.Optimal))
                {
                    stream.Write(data, 0, data.Length);
                }

                return output.ToArray();
            }
        }

        /// <summary>
        /// Decompressing bytes
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] DecompressDeflate(this byte[] data)
        {
            using (var input = new MemoryStream(data))
            {
                using (var output = new MemoryStream())
                {
                    using (var stream = new DeflateStream(input, CompressionMode.Decompress))
                    {
                        stream.CopyTo(output);
                    }

                    return output.ToArray();
                }
            }
        }

        /// <summary>
        /// Compresses a string and returns a deflate compressed, Base64 encoded string.
        /// </summary>
        /// <param name="uncompressedString">String to compress</param>
        public static string Compress(this string uncompressedString)
        {
            var uncompressed = Encoding.UTF8.GetBytes(uncompressedString);
            return Convert.ToBase64String(uncompressed.CompressDeflate());
        }

        /// <summary>
        /// Decompresses a deflate compressed, Base64 encoded string and returns an uncompressed string.
        /// </summary>
        /// <param name="compressedString">String to decompress.</param>
        public static string Decompress(this string compressedString)
        {
            var compressed = Convert.FromBase64String(compressedString);
            return Encoding.UTF8.GetString(compressed.DecompressDeflate());
        }

        /// <summary>
        /// Compresses a string and returns a deflate compressed, Base64 encoded string.
        /// </summary>
        /// <param name="uncompressedString">String to compress</param>
        public static async Task<string> CompressAsync(this string uncompressedString)
        {
            var uncompressed = Encoding.UTF8.GetBytes(uncompressedString);
            return Convert.ToBase64String(await uncompressed.CompressDeflateAsync());
        }

        /// <summary>
        /// Decompresses a deflate compressed, Base64 encoded string and returns an uncompressed string.
        /// </summary>
        /// <param name="compressedString">String to decompress.</param>
        public static async Task<string> DecompressAsync(this string compressedString)
        {
            var compressed = Convert.FromBase64String(compressedString);
            return Encoding.UTF8.GetString(await compressed.DecompressDeflateAsync());
        }


        /// <summary>
        /// Async version of <see cref="CompressDeflate"/>
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<byte[]> CompressDeflateAsync(this byte[] data)
        {
            await using (var output = new MemoryStream())
            {
                await using (var stream = new DeflateStream(output, CompressionLevel.Optimal))
                {
                    await stream.WriteAsync(data, 0, data.Length);
                }

                return output.ToArray();
            }
        }

        /// <summary>
        /// Async version of <see cref="DecompressDeflate"/>
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<byte[]> DecompressDeflateAsync(this byte[] data)
        {
            var input = new MemoryStream(data);
            await using (var output = new MemoryStream())
            {
                await using (var stream = new DeflateStream(input, CompressionMode.Decompress))
                {
                    await stream.CopyToAsync(output);
                }

                return output.ToArray();
            }
        }
    }
}