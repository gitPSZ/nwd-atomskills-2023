using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.authorization
{
    public static class AsymmetricCypherHelper
    {
        public static string Hash(string text)
        {
            var bytes = System.Text.Encoding.Unicode.GetBytes(text);
            using (var sha = new System.Security.Cryptography.SHA512Managed())
            {
                var hash = sha.ComputeHash(bytes);
                var hashedInputStringBuilder = new StringBuilder(1024);

                foreach (var bt in hash)
                {
                    hashedInputStringBuilder.Append(bt.ToString("X2"));
                }
                return hashedInputStringBuilder.ToString();
            }
        }

        public static string GenerateSalt()
        {
            var salt = StringCipher.Generate256BitsOfRandomEntropy();

            var hashedInputStringBuilder = new StringBuilder(1024);
            foreach (var bt in salt)
            {
                hashedInputStringBuilder.Append(bt.ToString("X2"));
            }

            var saltString = hashedInputStringBuilder.ToString();
            return saltString;
        }

    }
}
