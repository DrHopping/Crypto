using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Intro
{
    class Program
    {
        static void Main(string[] args)
        {
            var cipherBinary = File.ReadAllText(@"..\..\..\Files\Ciphertext.txt");
            var cipherBytes = GetBytesFromBinaryString(cipherBinary);
            var cipherText = Encoding.ASCII.GetString(cipherBytes);
            var decodedText = Encoding.UTF8.GetString(Convert.FromBase64String(cipherText));
            File.WriteAllText(@"..\..\..\Files\decoded.txt", decodedText);
        }

        private static byte[] GetBytesFromBinaryString(string binary)
        {
            var list = new List<byte>();

            for (var i = 0; i < binary.Length; i += 8)
            {
                var t = binary.Substring(i, 8);
                list.Add(Convert.ToByte(t, 2));
            }

            return list.ToArray();
        }
    }
}