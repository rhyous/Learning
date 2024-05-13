using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw1.Simulator
{
    public class Random
    {
        internal RNGCryptoServiceProvider Rng = new RNGCryptoServiceProvider();
        
        public int Next()
        {

            byte[] randomNumber = new byte[4];//4 for int32
            Rng.GetBytes(randomNumber);
            return BitConverter.ToInt32(randomNumber, 0);
        }

        public int Next(int mod)
        {
            return Math.Abs(Next()) % mod;
        }
    }
}