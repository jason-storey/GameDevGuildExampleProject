#region

using System;
using System.Text;

#endregion

namespace JCore.Hashing.NonCryptographic
{
    public static class CityHash
    {
        public static ulong ToCityHash(this string s) => ComputeHash(s);
        
        public static ulong ComputeHash(string s) => CityHash64(Encoding.UTF8.GetBytes(s));
        
        const ulong kMul = 0x9ddfea08eb382d69UL;

        public static ulong CityHash64(byte[] s)
        {
            var len = s.Length;
            if (len <= 16)
                return HashLen0to16(s);
            if (len <= 32)
                return HashLen17to32(s);
            if (len <= 64) return HashLen33to64(s);

            var x = BitConverter.ToUInt64(s, len - 40);
            var y = BitConverter.ToUInt64(s, len - 16) + BitConverter.ToUInt64(s, len - 56);
            var z = HashLen16(BitConverter.ToUInt64(s, len - 48) + ((ulong)len * kMul),
                BitConverter.ToUInt64(s, len - 24));

            var v = WeakHashLen32WithSeeds(s, len - 64, y, z);
            var w = WeakHashLen32WithSeeds(s, len - 32, y + kMul, x);
            x = x * kMul + BitConverter.ToUInt64(s, len - 8);

            var mul = kMul + ((z & 0x82A2B175UL) << 49);
            var a = (Rotate(v.Item2, 42) + v.Item1) * mul;
            var b = (w.Item2 + Rotate(w.Item1, 35)) * mul;

            return HashLen16(Rotate(x - b, 39) * mul + a, a + Rotate(x, 43) * mul);
        }

        static ulong Rotate(ulong val, int shift)
        {
            return shift == 0 ? val : (val >> shift) | (val << (64 - shift));
        }

        static ulong ShiftMix(ulong val)
        {
            return val ^ (val >> 47);
        }

        static ulong HashLen16(ulong u, ulong v)
        {
            return Hash128to64(new Tuple<ulong, ulong>(u, v));
        }

        static ulong Hash128to64(Tuple<ulong, ulong> x)
        {
            var a = (x.Item1 ^ x.Item2) * kMul;
            a ^= (a >> 47);
            var b = (x.Item2 ^ a) * kMul;
            b ^= (b >> 47);
            b *= kMul;
            return b;
        }

        static Tuple<ulong, ulong> WeakHashLen32WithSeeds(byte[] s, int index, ulong a, ulong b)
        {
            var w = BitConverter.ToUInt64(s, index);
            var x = BitConverter.ToUInt64(s, index + 8);
            var y = BitConverter.ToUInt64(s, index + 16);
            var z = BitConverter.ToUInt64(s, index + 24);
            a += w;
            b = Rotate(b + a + z, 21);
            var c = a;
            a += x;
            a += y;
            b += Rotate(a, 44);
            return new Tuple<ulong, ulong>(a + z, b + c);
        }

        private static ulong HashLen0to16(byte[] s)
        {
            var len = s.Length;
            if (len >= 8)
            {
                var mul = kMul + ((ulong)len * 2);
                var a = BitConverter.ToUInt64(s, 0) + kMul;
                var b = BitConverter.ToUInt64(s, len - 8);
                var c = Rotate(b, 37) * mul + a;
                var d = Rotate(a, 25) + b;
                return HashLen16(c, d) * mul;
            }

            if (len >= 4)
            {
                ulong a = BitConverter.ToUInt32(s, 0);
                ulong b = BitConverter.ToUInt32(s, len - 4);
                return HashLen16((ulong)len + (a << 3), b) * kMul;
            }

            if (len > 0)
            {
                ushort a = s[0];
                ushort b = s[len >> 1];
                ushort c = s[len - 1];
                ulong y = (uint)(a + (b << 8));
                ulong z = (uint)(len + (c << 2));
                return ShiftMix(y * kMul ^ z * kMul) * kMul;
            }

            return kMul;
        }

        private static ulong HashLen17to32(byte[] s)
        {
            var len = s.Length;
            var mul = kMul + ((ulong)len * 2);
            var a = BitConverter.ToUInt64(s, 0) * kMul;
            var b = BitConverter.ToUInt64(s, 8);
            var c = BitConverter.ToUInt64(s, len - 24);
            var d = BitConverter.ToUInt64(s, len - 16) * mul;
            var e = BitConverter.ToUInt64(s, len - 8);
            var h = d + Rotate(a + b, 43) + Rotate(c, 30) + e;
            var y = Rotate(a + e, 20) + h + b;
            return HashLen16(y, h) * mul;
        }

        private static ulong HashLen33to64(byte[] s)
        {
            var len = s.Length;
            var mul = kMul + ((ulong)len * 2);
            var a = BitConverter.ToUInt64(s, 0) * kMul;
            var b = BitConverter.ToUInt64(s, 8);
            var c = BitConverter.ToUInt64(s, len - 24);
            var d = BitConverter.ToUInt64(s, len - 16) * mul;
            var e = BitConverter.ToUInt64(s, len - 8);
            var h = d + Rotate(a + b, 43) + Rotate(c, 30) + e;
            var y = Rotate(a + e, 20) + h + b;
            var x = HashLen16(y, h) * mul;
            var z = Rotate(e + c, 18) + a;

            return HashLen16(x, z) * mul;
        }
    }
}