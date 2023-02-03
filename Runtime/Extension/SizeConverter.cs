using UnityEngine;

namespace Better.Extensions.Runtime
{
    public static class SizeConverter
    {
        private const float ConvertRation = 1024f;

        public enum SizeUnits : long
        {
            Byte = 0,
            Kb = 1,
            Mb = 2,
            Gb = 3,
            Tb = 4,
            Pb = 5,
            Eb = 6,
            Zb = 7,
            Yb = 8
        }

        public static double ToSize(this ulong value, SizeUnits unit)
        {
            return (value / Mathf.Pow(1024, (long)unit));
        }

        public static double ToSize(this long value, SizeUnits unit)
        {
            return (value / Mathf.Pow(1024, (long)unit));
        }

        public static double ToSize(this double value, SizeUnits unit)
        {
            return (value / Mathf.Pow(1024, (long)unit));
        }

        public static double ToMegabytes(this ulong bytes)
        {
            return bytes.ToSize(SizeUnits.Mb);
        }

        public static double ToMegabytes(this double bytes)
        {
            return bytes.ToSize(SizeUnits.Mb);
        }

        public static double ToMegabytes(this long bytes)
        {
            return bytes.ToSize(SizeUnits.Mb);
        }
    }
}