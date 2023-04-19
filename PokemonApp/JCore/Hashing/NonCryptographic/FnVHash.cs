using System.Text;

namespace JCore.Hashing.NonCryptographic
{
    public static class FNV1a32Hash
    {
        public static uint ToFnVHash(this string s) => ToFnVHash(Encoding.UTF8.GetBytes(s));

        public static uint ToFnVHash(byte[] bytes) => Compute(bytes);

        const uint FNVPrime32 = 16777619;
        const uint FNVOffsetBasis32 = 2166136261;

        public static uint Compute(byte[] data)
        {
            uint hash = FNVOffsetBasis32;
            for (int i = 0; i < data.Length; i++)
            {
                hash ^= data[i];
                hash *= FNVPrime32;
            }
            return hash;
        }
    }
}