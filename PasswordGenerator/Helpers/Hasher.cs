using System;
using System.Security.Cryptography;
using System.Text;
using Sodium;

namespace PasswordGenerator.Helpers
{
    public class Hasher
    {
        private MD5 _md5;
        
        public Hasher()
        {
            _md5 = MD5.Create();
        }

        public string ComputeMd5Hash(string input)
        {
            var hash = _md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(hash);
        }

        public string ComputeArgon2IdHash(string input) => PasswordHash.ArgonHashString(input).TrimEnd('\0');
    }
}