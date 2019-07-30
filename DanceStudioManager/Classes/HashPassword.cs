using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DanceStudioManager
{
    public class HashPassword
    {
        public byte[] GenerateRandomCryptographicBytes(int keyLength)
        {
            RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            byte[] randomBytes = new byte[keyLength];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return randomBytes;
        }

        public string HashWithSalt(string password, int saltLength, HashAlgorithm hashAlgo, byte[] saltBytes)
        {
            byte[] passwordAsBytes = Encoding.UTF8.GetBytes(password);
            List<byte> passwordWithSaltBytes = new List<byte>();
            passwordWithSaltBytes.AddRange(passwordAsBytes);
            passwordWithSaltBytes.AddRange(saltBytes);
            byte[] digestBytes = hashAlgo.ComputeHash(passwordWithSaltBytes.ToArray());
            return new string(Convert.ToBase64String(saltBytes) + Convert.ToBase64String(digestBytes));
        }
    }
}
