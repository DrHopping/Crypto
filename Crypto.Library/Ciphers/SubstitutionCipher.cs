using System;
using Crypto.Library.Helpers;

namespace Crypto.Library.Ciphers
{
    public class SubstitutionCipher
    {
        private static string Crypt(string text, char[] oldAlphabet, char[] newAlphabet)
        {
            var result = String.Empty;
            foreach (var t in text)
            {
                var oldCharIndex = Array.IndexOf(oldAlphabet,char.ToUpper(t));
                if (oldCharIndex >= 0)
                    result += char.IsUpper(t) ? char.ToUpper(newAlphabet[oldCharIndex]) : newAlphabet[oldCharIndex];
                else
                    result += t;
            }

            return result;
        }

        public static string Encrypt(string text, char[] cipherAlphabet)
        {
            return Crypt(text, Constants.Letters.ToCharArray(), cipherAlphabet);
        }
        
        public static string Decrypt(string text, char[] cipherAlphabet)
        {
            return Crypt(text, cipherAlphabet, Constants.Letters.ToCharArray());
        }
    }
}