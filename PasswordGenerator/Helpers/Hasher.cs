using System;
using System.Security.Cryptography;
using System.Text;
using NSec.Cryptography;
using HashAlgorithm = NSec.Cryptography.HashAlgorithm;

namespace PasswordGenerator.Helpers
{
    public class Hasher
    {
        private MD5 _md5;
        private Argon2id _argon;
        
        public Hasher()
        {
            _md5 = MD5.Create();
            _argon = new Argon2id(new Argon2Parameters()
            {
                DegreeOfParallelism = 1,
                MemorySize = 1024,
                NumberOfPasses = 1
            });
        }

        public string ComputeMd5Hash(string input)
        {
            var hash = _md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(hash);
        }

        public (string Hash, string Salt) ComputeArgon2IdHash(string input)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(input);
            var salt = CreateSalt();
            var hash = _argon.DeriveBytes(passwordBytes, salt, 16);
            return (Convert.ToHexString(hash), Convert.ToHexString(salt));
        }

        private byte[] CreateSalt()
        {
            var buffer = new byte[16];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buffer);
            return buffer;
        }
    }
}