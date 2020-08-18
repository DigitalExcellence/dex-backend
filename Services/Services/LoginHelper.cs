using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Services.Services
{
    public static class LoginHelper
    {

        private const int SaltSize = 20;
        private const int HashSize = 20;
        private const int HashIterations = 100000;

        /// <summary>
        /// Get the hash of the password
        /// </summary>
        /// <param name="password">string password</param>
        /// <returns>Hash secured password</returns>
        public static string GetHashPassword(string password)
        {
            // 1.-Create the salt value with a cryptographic PRNG
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            // 2.-Create the RFC2898DeriveBytes and get the hash value
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, HashIterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            // 3.-Combine the salt and password bytes for later use
            byte[] hashBytes = new byte[SaltSize+HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            // 4.-Turn the combined salt+hash into a string for storage
            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Check if the password is valid
        /// </summary>
        /// <param name="password">Entered by user</param>
        /// <param name="hashPass">Stored password</param>
        /// <returns>True if is Valid.</returns>
        public static bool IsValidPassword(string password, string hashPass)
        {
            // Extract the bytes
            byte[] hashBytes = Convert.FromBase64String(hashPass);
            // Get the salt
            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);
            // Compute the hash on the password the user entered
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, HashIterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);
            // compare the results
            for(int i = 0; i < HashSize; i++)
            {
                if(hashBytes[i + SaltSize] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
