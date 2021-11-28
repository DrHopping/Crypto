using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Crypto.Library.Analysis;
using Crypto.Library.Ciphers;
using Crypto.Library.Genetic;
using Crypto.Library.Helpers;

namespace Intro
{
    class Program
    {
        private const string task1 =
            "7958401743454e1756174552475256435e59501a5c524e176f786517545e475f5245191772195019175e4317445f58425b531743565c521756174443455e595017d5b7ab5f525b5b58174058455b53d5b7aa175659531b17505e41525917435f52175c524e175e4417d5b7ab5c524ed5b7aa1b174f584517435f5217515e454443175b524343524517d5b7ab5fd5b7aa17405e435f17d5b7ab5cd5b7aa1b17435f5259174f584517d5b7ab52d5b7aa17405e435f17d5b7ab52d5b7aa1b17435f525917d5b7ab5bd5b7aa17405e435f17d5b7ab4ed5b7aa1b1756595317435f5259174f58451759524f4317545f564517d5b7ab5bd5b7aa17405e435f17d5b7ab5cd5b7aa175650565e591b17435f525917d5b7ab58d5b7aa17405e435f17d5b7ab52d5b7aa1756595317445817585919176e5842175a564e17424452175659175e5953524f1758511754585e59545e53525954521b177f565a5a5e595017535e4443565954521b177c56445e445c5e17524f565a5e5956435e58591b17444356435e44435e54565b17435244434417584517405f564352415245175a52435f5853174e5842175152525b174058425b5317445f584017435f52175552444317455244425b4319";
     
        private const string task2 =
            "G0IFOFVMLRAPI1QJbEQDbFEYOFEPJxAfI10JbEMFIUAAKRAfOVIfOFkYOUQFI15ML1kcJFUeYhA4IxAeKVQZL1VMOFgJbFMDIUAAKUgFOElMI1ZMOFgFPxADIlVMO1VMO1kAIBAZP1VMI14ANRAZPEAJPlMNP1VMIFUYOFUePxxMP19MOFgJbFsJNUMcLVMJbFkfbF8CIElMfgZNbGQDbFcJOBAYJFkfbF8CKRAeJVcEOBANOUQDIVEYJVMNIFwVbEkDORAbJVwAbEAeI1INLlwVbF4JKVRMOF9MOUMJbEMDIVVMP18eOBADKhALKV4JOFkPbFEAK18eJUQEIRBEO1gFL1hMO18eJ1UIbEQEKRAOKUMYbFwNP0RMNVUNPhlAbEMFIUUALUQJKBANIl4JLVwFIldMI0JMK0INKFkJIkRMKFUfL1UCOB5MH1UeJV8ZP1wVYBAbPlkYKRAFOBAeJVcEOBACI0dAbEkDORAbJVwAbF4JKVRMJURMOF9MKFUPJUAEKUJMOFgJbF4JNERMI14JbFEfbEcJIFxCbHIJLUJMJV5MIVkCKBxMOFgJPlWOzKkfbF4DbEMcLVMJPx5MRlgYOEAfdh9DKF8PPx4LI18LIFVCL18BY1QDL0UBKV4YY1RDfXg1e3QAYQUFOGkof3MzK1sZKXIaOnIqPGRYD1UPC2AFHgNcDkMtHlw4PGFDKVQFOA8ZP0BRP1gNPlkCKw==";

        private const string task3 =
            "EFFPQLEKVTVPCPYFLMVHQLUEWCNVWFYGHYTCETHQEKLPVMSAKSPVPAPVYWMVHQLUSPQLYWLASLFVWPQLMVHQLUPLRPSQLULQESPBLWPCSVRVWFLHLWFLWPUEWFYOTCMQYSLWOYWYETHQEKLPVMSAKSPVPAPVYWHEPPLUWSGYULEMQTLPPLUGUYOLWDTVSQETHQEKLPVPVSMTLEUPQEPCYAMEWWYTYWDLUULTCYWPQLSEOLSVOHTLUYAPVWLYGDALSSVWDPQLNLCKCLRQEASPVILSLEUMQBQVMQCYAHUYKEKTCASLFPYFLMVHQLUPQLHULIVYASHEUEDUEHQBVTTPQLVWFLRYGMYVWMVFLWMLSPVTTBYUNESESADDLSPVYWCYAMEWPUCPYFVIVFLPQLOLSSEDLVWHEUPSKCPQLWAOKLUYGMQEUEMPLUSVWENLCEWFEHHTCGULXALWMCEWETCSVSPYLEMQYGPQLOMEWCYAGVWFEBECPYASLQVDQLUYUFLUGULXALWMCSPEPVSPVMSBVPQPQVSPCHLYGMVHQLUPQLWLRPOEDVMETBYUFBVTTPENLPYPQLWLRPTEKLWZYCKVPTCSTESQPBYMEHVPETCMEHVPETZMEHVPETKTMEHVPETCMEHVPETT";

        private const string task4 =
            "UMUPLYRXOYRCKTYYPDYZTOUYDZHYJYUNTOMYTOLTKAOHOKZCMKAVZDYBRORPTHQLSERUOERMKZGQJOIDJUDNDZATUVOTTLMQBOWNMERQTDTUFKZCMTAZMEOJJJOXMERKJHACMTAZATIZOEPPJKIJJNOCFEPLFBUNQHHPPKYYKQAZKTOTIKZNXPGQZQAZKTOTIZYNIUISZIAELMKSJOYUYYTHNEIEOESULOXLUEYGBEUGJLHAJTGGOEOSMJHNFJALFBOHOKAGPTIHKNMKTOUUUMUQUDATUEIRBKYUQTWKJKZNLDRZBLTJJJIDJYSULJARKHKUKBISBLTOJRATIOITHYULFBITOVHRZIAXFDRNIORLZEYUUJGEBEYLNMYCZDITKUXSJEJCFEUGJJOTQEZNORPNUDPNQIAYPEDYPDYTJAIGJYUZBLTJJYYNTMSEJYFNKHOTJARNLHHRXDUPZIALZEDUYAOSBBITKKYLXKZNQEYKKZTOKHWCOLKURTXSKKAGZEPLSYHTMKRKJIIQZDTNHDYXMEIRMROGJYUMHMDNZIOTQEKURTXSKKAGZEPLSYHTMKRKJIIQZDTNROAUYLOTIMDQJYQXZDPUMYMYPYRQNYFNUYUJJEBEOMDNIYUOHYYYJHAOQDRKKZRRJEPCFNRKJUHSJOIRQYDZBKZURKDNNEOYBTKYPEJCMKOAJORKTKJLFIOQHYPNBTAVZEUOBTKKBOWSBKOSKZUOZIHQSLIJJMSURHYZJJZUKOAYKNIYKKZNHMITBTRKBOPNUYPNTTPOKKZNKKZNLKZCFNYTKKQNUYGQJKZNXYDNJYYMEZRJJJOXMERKJVOSJIOSIQAGTZYNZIOYSMOHQDTHMEDWJKIULNOTBCALFBJNTOGSJKZNEEYYKUIXLEUNLNHNMYUOMWHHOOQNUYGQJKZLZJZLOLATSEHQKTAYPYRZJYDNQDTHBTKYKYFGJRRUFEWNTHAXFAHHODUPZMXUMKXUFEOTIMUNQIHGPAACFKATIKIZBTOTIKZNKKZNLORUKMLLFBUUQKZNLEOHIEOHEDRHXOTLMIRKLEAHUYXCZYTGUYXCZYTIUYXCZYTCVJOEBKOHE";

        static void Main(string[] args)
        {
            Console.WriteLine($"1) {SolveTask1(task1)}");
            Console.WriteLine($"2) {SolveTask2(task2)}");
            Console.WriteLine($"3) {SolveTask3(task3)}");
            Console.WriteLine($"4) {SolveTask4(task4)}");
        }

        private static string SolveTask4(string task)
        {
            var a = GetPossibleKeyLengths(task, 32).FirstOrDefault();
            var population = new PolyGeneticAlgorithm(200, task, a)
            {
                Mutations = 50,
                MutationProbability = 0.9,
                MutationSwaps = 2
            };
            
            // for (int i = 0; i < 5; i++)
            // {
            //     if (i % 10000 != 0) continue;
            //     var result = population.RunEpoch();
            //     Console.WriteLine($"{i}) Score: {result.Key} Text:{PolySubstitutionCipher.Decrypt(task, result.Value)}");
            // }

            return PolySubstitutionCipher.Decrypt(task, population.Run(5).Value);
        }
        

        private static string SolveTask3(string task)
        {
            var population = new GeneticAlgorithm(50, task)
            {
                Mutations = 5,
                MutationProbability = 0.8,
                MutationSwaps = 1
            };
            var result = population.Run(5);;
            return SubstitutionCipher.Decrypt(task, result.Value);
        }
        
        private static string SolveTask2(string task)
        {
            var taskBytes = Convert.FromBase64String(task);
            var taskString = Encoding.ASCII.GetString(taskBytes);
            var keyLength = GetPossibleKeyLengths(taskString, 16).First();
            var key = new byte[keyLength];
            for (var i = 0; i < keyLength; i++)
            {
                foreach (var keyByte in Constants.GetAllBytes())
                {
                    var everyNthByte = taskBytes.Where((x, j) => j % keyLength == i).ToArray();
                    var decrypted = Encoding.ASCII.GetString(SingleByteXor(everyNthByte, keyByte));
                    if (!English.IsEnglish(decrypted)) continue;
                    key[i] = keyByte;
                    break;
                }
            }

            return Encoding.ASCII.GetString(MultipleByteXor(taskBytes, key));
        }

        // private static int GetPossibleKeyLengthForTask4(string message)
        // {
        //     int max = 64;
        //     var ics = new double[max];
        //     for (var i = 1; i <= max; i++)
        //     {
        //         var offsetText = GetOffsetText(message, i);
        //         var ic = Utils.CalculateIC(offsetText);
        //         ics[i - 1] = ic;
        //         Console.WriteLine($"{i},{ic}");
        //     }
        //     for (var i = 0; i < ics.Length; i++)
        //     {
        //         var ic = ics[i];
        //         var next = i > 0 ? ics[i - 1] : ics[i + 1];
        //         var diff = ic - next;
        //         if (diff > 0.2)
        //         {
        //             return i + 1;
        //         }
        //     }
        //     return -1;
        //     
        // }
        //
        // private static string GetOffsetText(string text, int offset)
        // {
        //     var builder = new StringBuilder();
        //     for (var i = 0; i < text.Length; i += offset)
        //     {
        //         builder.Append(text[i]);
        //     }
        //     return builder.ToString();
        // }
        
        private static List<int> GetPossibleKeyLengths(string message, int maxLength)
        {
            var paddingIndexList = new List<(int Length, double Index)>();
            for (var i = 1; i <= maxLength; i++)
            {
                var shifted = message[i..] + message[..i];
                var equalCount = message.Select((c, j) => shifted[j] == c).Count(x => x);
                paddingIndexList.Add((i, (double)equalCount/message.Length));
            }
            var averageIndex = paddingIndexList.Average(x => x.Index);
            var usefulLengthsList = paddingIndexList.Where(x => x.Index > averageIndex)
                .Where(x => x.Length != 1)
                .OrderBy(x => x.Length).ToList();
            return usefulLengthsList.All(x => x.Length % usefulLengthsList.First().Length == 0) 
                ? new List<int> { usefulLengthsList.First().Length } 
                : usefulLengthsList.Select(x => x.Length).ToList();
        }
        
        private static string SolveTask1(string task)
        {
            var list = new List<string>();
            for (var i = 0; i <= 255; i++)
            {
                var text = Encoding.UTF8.GetString(SingleByteXor(HexToByteArray(task1), (byte)i));
                if (English.IsEnglish(text)) list.Add(text);   
            }
            if (list.Count != 1) throw new Exception();
            return list.First();
        }

        private static byte[] HexToByteArray(string input)
        {
            return Enumerable.Range(0, input.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(input.Substring(x, 2), 16))
                .ToArray();
        }

        private static byte[] MultipleByteXor(byte[] input, byte[] key)
        {
            return input.Select((x,i) => (byte)(x ^ key[i % key.Length])).ToArray();
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