using System.Collections.Generic;
using System.Linq;

namespace Crypto.Library.Ciphers
{
    public class PolySubstitutionCipher
    {
        public static string Decrypt(string text, List<char[]> alphabets)
        {
            return string.Concat(text.Select((x, i) => SubstitutionCipher.Decrypt(x.ToString(), alphabets[i % alphabets.Count])));
        }
    }
}