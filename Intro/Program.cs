using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Intro
{
    class Program
    {
        private const string task1 =
            "7958401743454e1756174552475256435e59501a5c524e176f786517545e475f5245191772195019175e4317445f58425b531743565c521756174443455e595017d5b7ab5f525b5b58174058455b53d5b7aa175659531b17505e41525917435f52175c524e175e4417d5b7ab5c524ed5b7aa1b174f584517435f5217515e454443175b524343524517d5b7ab5fd5b7aa17405e435f17d5b7ab5cd5b7aa1b17435f5259174f584517d5b7ab52d5b7aa17405e435f17d5b7ab52d5b7aa1b17435f525917d5b7ab5bd5b7aa17405e435f17d5b7ab4ed5b7aa1b1756595317435f5259174f58451759524f4317545f564517d5b7ab5bd5b7aa17405e435f17d5b7ab5cd5b7aa175650565e591b17435f525917d5b7ab58d5b7aa17405e435f17d5b7ab52d5b7aa1756595317445817585919176e5842175a564e17424452175659175e5953524f1758511754585e59545e53525954521b177f565a5a5e595017535e4443565954521b177c56445e445c5e17524f565a5e5956435e58591b17444356435e44435e54565b17435244434417584517405f564352415245175a52435f5853174e5842175152525b174058425b5317445f584017435f52175552444317455244425b4319";

        static void Main(string[] args)
        {
            //DecodeTasks(@"..\..\..\Files\Ciphertext.txt", @"..\..\..\Files\decoded.txt");
            Console.WriteLine(SolveTask1(task1));
        }


        private static string SolveTask1(string task)
        {
            var list = new List<(string Text, double Coef)>();
            for (var i = 0; i <= 255; i++)
            {
                var text = Encoding.UTF8.GetString(SingleByteXor(HexToByteArray(task1), (byte)i));
                var count = text.Count(c => Char.IsLetterOrDigit(c) || Char.IsPunctuation(c) || Char.IsSeparator(c));
                var percent = (double)count / text.Length;
                list.Add((text,percent));
            }

            return list.OrderByDescending(t => t.Coef).First().Text;
        }

        public static byte[] HexToByteArray(string input)
        {
            return Enumerable.Range(0, input.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(input.Substring(x, 2), 16))
                .ToArray();
        }

        private static byte[] SingleByteXor(byte[] input, byte key)
        {
            return input.Select(x => (byte)(Convert.ToByte(x) ^ key)).ToArray();
        }
        
        private static void DecodeTasks(string inputPath, string outputPath)
        {
            var cipherBinary = File.ReadAllText(inputPath);
            var cipherBytes = GetBytesFromBinaryString(cipherBinary);
            var cipherText = Encoding.ASCII.GetString(cipherBytes);
            var decodedText = Encoding.UTF8.GetString(Convert.FromBase64String(cipherText));
            File.WriteAllText(outputPath, decodedText);
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