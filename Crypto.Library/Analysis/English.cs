using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Crypto.Library.Helpers;

namespace Crypto.Library.Analysis
{
    public static class English
    {
        private const double DeviationThreshold = 80;
        private const double LettersPercentThreshold = 70;
        private const double UpperPercentThreshold = 10;
        private static readonly Regex EnglishLetterPattern = new Regex("[a-zA-Z]", RegexOptions.Compiled);

        public static bool IsEnglishLetter(char letter)
        {
            return EnglishLetterPattern.IsMatch(letter.ToString());
        }

        public static bool IsEnglish(string message, double deviationThreshold = DeviationThreshold,
            double lettersPercentThreshold = LettersPercentThreshold, double upperPercentThreshold = UpperPercentThreshold)
        {
            var letters = Constants.GetEmptyLettersDictionary();
            foreach (var letter in message.Where(IsEnglishLetter).Select(char.ToUpper))
            {
                letters[letter]++;
            }

            var lettersCount = letters.Sum(x => x.Value);
            var upperCount = message.Where(Char.IsUpper).Count();

            foreach (var key in letters.Keys)
            {
                letters[key] = (letters[key] / message.Length) * 100;
            }

            var deviation = GetDeviation(message) * 100;
            var lettersPercent = (lettersCount / message.Length) * 100;
            var upperPercent = ((double)upperCount / message.Length) * 100;
            return deviation > deviationThreshold 
                   && lettersPercent > lettersPercentThreshold 
                   && upperPercent < upperPercentThreshold;
        }

        public static double GetDeviation(string text)
        {
            var letters = Constants.GetEmptyLettersDictionary();
            foreach (var letter in text.Where(IsEnglishLetter).Select(char.ToUpper)) letters[letter]++;
            var sum = letters.Sum(x => Math.Abs((Constants.EnglishLetterFreq2[x.Key] / 100) * text.Length - x.Value));
            var norm = 2 * (text.Length - (Constants.EnglishLetterFreq2['Z'] / 100) * text.Length);
            return (norm - sum) / norm;
        }
    }
}