using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace BetterExtension.Runtime
{
    public static class SerializeExtensions
    {
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

        public static byte[] Serialize(this object obj)
        {
            var binFormatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                binFormatter.Serialize(stream, obj);

                return stream.ToArray();
            }
        }

        public static byte[] Compress(this byte[] data)
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

        public static byte[] Decompress(this byte[] data)
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

        public static async Task<byte[]> SerializeAsync(this object obj)
        {
            return await Task.Factory.StartNew(() => Serialize(obj));
        }

        public static async Task<byte[]> CompressAsync(this byte[] data)
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

        public static async Task<byte[]> DecompressAsync(this byte[] data)
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