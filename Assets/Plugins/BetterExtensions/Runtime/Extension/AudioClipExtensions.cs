using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Better.Extensions.Runtime
{
    public static class AudioClipExtensions
    {
        /// <summary>
        /// Creates clip form uncompressed bytes
        /// </summary>
        /// <param name="array"></param>
        /// <param name="name"></param>
        /// <param name="channels"></param>
        /// <param name="frequency"></param>
        /// <returns></returns>
        public static AudioClip FromByteArray(this byte[] array, string name, int channels, int frequency)
        {
            var audioClipData = new float[array.Length / sizeof(float)];
            Buffer.BlockCopy(array, 0, audioClipData, 0, array.Length);
            var audioClip = AudioClip.Create(name,
                audioClipData.Length, channels, frequency,
                false);

            audioClip.SetData(audioClipData, 0);
            return audioClip;
        }

        /// <summary>
        /// Converts audio clip to uncompressed bytes
        /// </summary>
        /// <param name="audioClip"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this AudioClip audioClip)
        {
            var audioClipData = new float[audioClip.samples * audioClip.channels];
            audioClip.GetData(audioClipData, 0);

            var uncompressedData = new byte[audioClipData.Length * sizeof(float)];
            Buffer.BlockCopy(audioClipData, 0, uncompressedData, 0, uncompressedData.Length);
            return uncompressedData;
        }

        /// <summary>
        /// Removes part of audio clip
        /// </summary>
        /// <param name="audioClip"></param>
        /// <param name="trimLengthSeconds"></param>
        /// <returns></returns>
        public static AudioClip Trim(this AudioClip audioClip, float trimLengthSeconds)
        {
            if (audioClip.length <= trimLengthSeconds)
            {
                return audioClip;
            }

            var samplePosition = Mathf.Min((int)(trimLengthSeconds * audioClip.samples / audioClip.length),
                audioClip.samples);

            var data = new float[samplePosition * audioClip.channels];
            audioClip.GetData(data, 0);

            var newAudioClip = AudioClip.Create(audioClip.name,
                samplePosition,
                audioClip.channels,
                audioClip.frequency,
                false);

            newAudioClip.SetData(data, 0);

            return newAudioClip;
        }

        /// <summary>
        /// Amplifying audio clip volume
        /// </summary>
        /// <param name="audioClip"></param>
        /// <param name="amplificationRatio"></param>
        public static void Amplify(this AudioClip audioClip, float amplificationRatio)
        {
            if (amplificationRatio <= 0.0f)
            {
                return;
            }

            var data = new float[audioClip.samples * audioClip.channels];
            audioClip.GetData(data, 0);

            for (var i = 0; i < data.Length; i++)
            {
                data[i] *= amplificationRatio;
            }

            audioClip.SetData(data, 0);
        }

        /// <summary>
        /// Async version of <see cref="FromByteArray"/>
        /// </summary>
        /// <param name="array"></param>
        /// <param name="name"></param>
        /// <param name="channels"></param>
        /// <param name="frequency"></param>
        /// <returns></returns>
        public static async Task<AudioClip> FromByteArrayAsync(this byte[] array, string name, int channels,
            int frequency)
        {
            var audioClipData = await Task.Factory.StartNew(() => BlockCopy(array));
            var audioClip = AudioClip.Create(name,
                audioClipData.Length, channels, frequency,
                false);

            audioClip.SetData(audioClipData, 0);
            return audioClip;
        }

        /// <summary>
        /// Async version of <see cref="ToByteArray"/>
        /// </summary>
        /// <param name="audioClip"></param>
        /// <returns></returns>
        public static async Task<byte[]> ToByteArrayAsync(this AudioClip audioClip)
        {
            var audioClipData = new float[audioClip.samples * audioClip.channels];
            audioClip.GetData(audioClipData, 0);

            var uncompressedData = await Task.Factory.StartNew(() => BlockCopy(audioClipData));

            return uncompressedData;
        }

        private static byte[] BlockCopy(float[] audioClipData)
        {
            var uncompressedData = new byte[audioClipData.Length * sizeof(float)];
            Buffer.BlockCopy(audioClipData, 0, uncompressedData, 0, uncompressedData.Length);
            return uncompressedData;
        }
        
        private static float[] BlockCopy(byte[] array)
        {
            var audioClipData = new float[array.Length / sizeof(float)];
            Buffer.BlockCopy(array, 0, audioClipData, 0, array.Length);
            return audioClipData;
        }

        /// <summary>
        /// Async version of <see cref="Amplify"/>
        /// </summary>
        /// <param name="audioClip"></param>
        /// <param name="amplificationRatio"></param>
        public static async Task AmplifyAsync(this AudioClip audioClip, float amplificationRatio)
        {
            if (amplificationRatio <= 0.0f)
            {
                return;
            }

            var data = new float[audioClip.samples * audioClip.channels];
            audioClip.GetData(data, 0);

            var bytes = await Task.Factory.StartNew(() =>
            {
                for (var i = 0; i < data.Length; i++)
                {
                    data[i] *= amplificationRatio;
                }

                return data;
            });

            audioClip.SetData(bytes, 0);
        }
    }
}