using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using Crypto.Library.Helpers;

namespace Crypto.Library.Analysis
{
    public class NGrams
    {
        public Dictionary<string, double> NGramsFrequency { get; }
        public int N { get; set; }
        public NGrams(int n)
        {
            N = n;
            NGramsFrequency =
                GetNGramsDictionary(n, File.ReadAllText(@"../../../../Crypto.Library/Files/WarAndPeace.txt"));
        }
        private static Dictionary<string,double> GetNGramsDictionary(int n, string text)
        {
            var filteredText = new string(text.Where(English.IsEnglishLetter).Select(char.ToUpper).ToArray());
            var dict = Combinator.GetCombinations(Constants.Letters.ToCharArray(), n).ToDictionary<char[], string, double>(nGram => new string(nGram), _ => 0);
            for (int i = 0; i < filteredText.Length - n; i++)
            {
                var nGram = filteredText.Substring(i, n);
                dict[nGram]++;
            }
            return dict;
        }

        public double GetScore(string text)
        {
            var inputNGrams = GetNGramsDictionary(N, text);
            return inputNGrams.Sum(x => 
                x.Value * NGramsFrequency[x.Key] > 0 
                        ? Math.Log2(NGramsFrequency[x.Key]) 
                        : 0);
        }
    }
}