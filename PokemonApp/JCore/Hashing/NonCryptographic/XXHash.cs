#region

using System;
using System.Text;

#endregion

namespace JCore.Hashing.NonCryptographic
{
    public static class XXHash
    {
        public static uint ToXXHash(this string s) => ToXXHash(Encoding.UTF8.GetBytes(s));

        public static uint ToXXHash(byte[] bytes, uint seed = 0) => Compute(bytes, seed);

        const uint Prime1 = 2654435761U;
        const uint Prime2 = 2246822519U;
        const uint Prime3 = 3266489917U;
        const uint Prime4 = 668265263U;
        const uint Prime5 = 374761393U;

        public static uint Compute(byte[] data, uint seed = 0)
        {
            var len = data.Length;
            var index = 0;
            uint h32;

            if (len >= 16)
            {
                var limit = len - 16;
                var v1 = seed + Prime1 + Prime2;
                var v2 = seed + Prime2;
                var v3 = seed;
                var v4 = seed - Prime1;

                while (index <= limit)
                {
                    v1 += BitConverter.ToUInt32(data, index) * Prime2;
                    v1 = RotateLeft(v1, 13);
                    v1 *= Prime1;
                    index += 4;

                    v2 += BitConverter.ToUInt32(data, index) * Prime2;
                    v2 = RotateLeft(v2, 13);
                    v2 *= Prime1;
                    index += 4;

                    v3 += BitConverter.ToUInt32(data, index) * Prime2;
                    v3 = RotateLeft(v3, 13);
                    v3 *= Prime1;
                    index += 4;

                    v4 += BitConverter.ToUInt32(data, index) * Prime2;
                    v4 = RotateLeft(v4, 13);
                    v4 *= Prime1;
                    index += 4;
                }

                h32 = RotateLeft(v1, 1) + RotateLeft(v2, 7) + RotateLeft(v3, 12) + RotateLeft(v4, 18);
            }
            else
            {
                h32 = seed + Prime5;
            }

            h32 += (uint)len;

            while (index + 4 <= len)
            {
                h32 += BitConverter.ToUInt32(data, index) * Prime3;
                h32 = RotateLeft(h32, 17) * Prime4;
                index += 4;
            }

            while (index < len)
            {
                h32 += data[index] * Prime5;
                h32 = RotateLeft(h32, 11) * Prime1;
                index++;
            }

            h32 ^= h32 >> 15;
            h32 *= Prime2;
            h32 ^= h32 >> 13;
            h32 *= Prime3;
            h32 ^= h32 >> 16;

            return h32;
        }

        private static uint RotateLeft(uint value, int count)
        {
            return (value << count) | (value >> (32 - count));
        }
    }
}