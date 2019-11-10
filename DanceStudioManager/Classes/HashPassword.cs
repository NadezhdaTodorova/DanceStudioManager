using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DanceStudioManager
{
    public static class HashPassword
    {

        /// <summary>
        /// Iteration count for the Pbkdf2 algorithm.
        /// </summary>
        const int IterationCount = 50000;


        /// <summary>
        /// Generates cryptographicaly random salt
        /// </summary>
        /// <returns>The generated salt in Base64 format.</returns>
        public static string GenerateSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        /// <summary>
        /// Generates password hash using Pbkdf2 algorithm.
        /// Usage: Generate a salt for every user, by calling <see cref="GenerateSalt"/>.
        /// After that call this method by passing the generated salt and the password.
        /// The hashed password and the salt must be stored in the database
        /// </summary>
        /// <param name="password"></param>
        /// <param name="saltBase64">The pre-generated salt in Base64 format that must be stored with the hashed password. It will be necessary for password hash verification</param>
        /// <returns>Hashed password in Base64 format</returns>
        public static string HashPasswordFunction(string password, string saltBase64)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Convert.FromBase64String(saltBase64),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: IterationCount,
                numBytesRequested: 256 / 8));
            return hashed;
        }

        /// <summary>
        /// Compares a password with stored password hash and salt.
        /// </summary>
        /// <param name="password">The plain text password</param>
        /// <param name="saltBase64">The Base64 format salt</param>
        /// <param name="hashedPassword">The hashed password</param>
        /// <returns>True if the password and the hash match. False otherwise.</returns>
        public static bool ComparePasswordAndHash(string password, string saltBase64, string hashedPassword)
        {
            return HashPasswordFunction(password, saltBase64) == hashedPassword;
        }

        /// <summary>
        /// Generates a random password with the specified characters and length
        /// </summary>
        /// <param name="length">The deisred length for the generated password</param>
        /// <param name="characterSet">The allowed characters to use in the generated password</param>
        /// <returns></returns>
        public static string GenerateRandomPassword(int length, IEnumerable<char> characterSet)
        {
            if (length < 0)
                throw new ArgumentException("length must not be negative", nameof(length));
            if (length > int.MaxValue / 8) // 250 million chars ought to be enough for anybody
                throw new ArgumentException("length is too big", nameof(length));
            if (characterSet == null)
                throw new ArgumentNullException(nameof(characterSet));
            var characterArray = characterSet.Distinct().ToArray();
            if (characterArray.Length == 0)
                throw new ArgumentException("characterSet must not be empty", nameof(characterSet));
            using (var rng = RandomNumberGenerator.Create())
            {
                var bytes = new byte[length * 8];
                rng.GetBytes(bytes);
                var result = new char[length];
                for (int i = 0; i < length; i++)
                {
                    ulong value = BitConverter.ToUInt64(bytes, i * 8);
                    result[i] = characterArray[value % (uint)characterArray.Length];
                }
                return new string(result);
            }
        }
    }
}

