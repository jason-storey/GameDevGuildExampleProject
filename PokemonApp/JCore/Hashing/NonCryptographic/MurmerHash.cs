using System;
using System.Text;

namespace JCore.Hashing.NonCryptographic
{
    public static class MurmurHash3
    {
        const uint Seed = 0x9747b28c;
        
        public static uint ToMurmerHash3(this string s) => ComputeHash(s);
        
        public static uint ComputeHash(string s) => ComputeHash(Encoding.UTF8.GetBytes(s));
        public static uint ComputeHash(byte[] data)
        {
            const uint c1 = 0xcc9e2d51;
            const uint c2 = 0x1b873593;
            const int r1 = 15;
            const int r2 = 13;
            const uint m = 5;
            const uint n = 0xe6546b64;

            int length = data.Length;
            int remainder = length & 3;
            int alignedLength = length - remainder;

            uint hash = Seed;

            for (int i = 0; i < alignedLength; i += 4)
            {
                uint k = BitConverter.ToUInt32(data, i);
                k *= c1;
                k = RotateLeft(k, r1);
                k *= c2;

                hash ^= k;
                hash = RotateLeft(hash, r2);
                hash = hash * m + n;
            }

            if (remainder > 0)
            {
                uint remainingBytes = 0;
                for (int i = length - 1; i >= alignedLength; i--)
                {
                    remainingBytes <<= 8;
                    remainingBytes |= data[i];
                }

                remainingBytes *= c1;
                remainingBytes = RotateLeft(remainingBytes, r1);
                remainingBytes *= c2;

                hash ^= remainingBytes;
            }

            hash ^= (uint)length;
            hash ^= hash >> 16;
            hash *= 0x85ebca6b;
            hash ^= hash >> 13;
            hash *= 0xc2b2ae35;
            hash ^= hash >> 16;

            return hash;
        }

        private static uint RotateLeft(uint x, int count)
        {
            return (x << count) | (x >> (32 - count));
        }
    }

}