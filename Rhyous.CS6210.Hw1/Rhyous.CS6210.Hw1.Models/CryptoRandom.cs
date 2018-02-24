using System;
using System.Security.Cryptography;

namespace Rhyous.CS6210.Hw1.Models
{
    public class CryptoRandom
    {
        internal RNGCryptoServiceProvider Rng = new RNGCryptoServiceProvider();
        
        public long Next()
        {

            byte[] randomNumber = new byte[8];//8 for Int64
            Rng.GetBytes(randomNumber);
            return BitConverter.ToInt64(randomNumber, 0);
        }

        public long Next(long start, long end)
        {
            long mod = (end - start) + 1;
            return Next(mod) + start;
        }

        public long Next(long mod)
        {
            return Math.Abs(Next()) % mod;
        }
    }
}