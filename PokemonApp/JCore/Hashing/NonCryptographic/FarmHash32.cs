using System;
using System.Text;

namespace JCore.Hashing.NonCryptographic
{
    public static class FarmHash32
    {
        public static uint ToFarmHash32(this string s) => Compute(Encoding.UTF8.GetBytes(s));
        public static uint ToFarmHash32(this byte[] bytes) => Compute(bytes);
        
        private const uint c1 = 0xcc9e2d51;
        private const uint c2 = 0x1b873593;

        private static uint Rotate(uint value, int count)
        {
            return (value << count) | (value >> (32 - count));
        }

        private static uint FMix(uint h)
        {
            h ^= h >> 16;
            h *= 0x85ebca6b;
            h ^= h >> 13;
            h *= 0xc2b2ae35;
            h ^= h >> 16;
            return h;
        }

        public static uint Compute(byte[] data)
        {
            int len = data.Length;
            uint h = (uint)len;

            int nBlocks = len / 4;
            for (int i = 0; i < nBlocks; i++)
            {
                uint k = BitConverter.ToUInt32(data, i * 4);
                k *= c1;
                k = Rotate(k, 15);
                k *= c2;

                h ^= k;
                h = Rotate(h, 13);
                h = h * 5 + 0xe6546b64;
            }

            int rem = len & 3;
            if (rem > 0)
            {
                uint k = 0;
                for (int i = 0; i < rem; ++i)
                {
                    k |= (uint)data[(nBlocks * 4) + i] << (i * 8);
                }
                k *= c1;
                k = Rotate(k, 15);
                k *= c2;
                h ^= k;
            }

            h ^= (uint)len;
            h = FMix(h);
            return h;
        }
    }
}