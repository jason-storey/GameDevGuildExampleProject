using System;
using JCore.Hashing.NonCryptographic;

namespace JCore.Hashing
{
    public static class HashingExtensions
    {
        public static uint ToHash(string text,NonCryptographicHashes algorithm = NonCryptographicHashes.FnV)
        {
            switch (algorithm)
            {
                case NonCryptographicHashes.Murmer:
                    return text.ToMurmerHash3();
                case NonCryptographicHashes.XXHash:
                    return text.ToXXHash();
                case NonCryptographicHashes.FnV:
                    return text.ToFnVHash();
                case NonCryptographicHashes.FarmHash32:
                    return text.ToFarmHash32();
               // case NonCryptographicHashes.CityHash:
                    //return text.ToCityHash(); - is a ulong not a uint
                default:
                    throw new ArgumentOutOfRangeException(nameof(algorithm), algorithm, null);
            }
        }
    }

    public enum NonCryptographicHashes
    {
        FnV,Murmer,XXHash,FarmHash32
    }
}