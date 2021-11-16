using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Crypto.Library.Helpers
{
    public static class Constants
    {
        public static readonly Regex NonlettersPattern = new Regex("[^A-Z]", RegexOptions.Compiled);
        public const string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string ETAOIN = "ETAOINSHRDLCUMWFGYPBVKJXQZ";
                

        public static byte[] GetEnglishLettersBytes()
        {
            return Enumerable.Range(65, 90).Concat(Enumerable.Range(97, 122)).Select(x => (byte)x).ToArray();
        }

        public static byte[] GetUpperEnglishLettersBytes()
        {
            return Enumerable.Range(65, 90).Select(x => (byte)x).ToArray();
        }
        
        public static byte[] GetLowerEnglishLettersBytes()
        {
            return Enumerable.Range(97, 122).Select(x => (byte)x).ToArray();
        }
        
        public static byte[] GetAllBytes()
        {
            return Enumerable.Range(0, 255).Select(x => (byte)x).ToArray();
        }
        
        public static Dictionary<char, double> EnglishLetterFreq = new()
        {
            {'E',12.70},
            {'T', 9.06},
            {'A', 8.17},
            {'O', 7.51},
            {'I', 6.97},
            {'N', 6.75},
            {'S', 6.33},
            {'H', 6.09},
            {'R', 5.99},
            {'D', 4.25},
            {'L', 4.03},
            {'C', 2.78},
            {'U', 2.76},
            {'M', 2.41},
            {'W', 2.36},
            {'F', 2.23},
            {'G', 2.02},
            {'Y', 1.97},
            {'P', 1.93},
            {'B', 1.29},
            {'V', 0.98},
            {'K', 0.77},
            {'J', 0.15},
            {'X', 0.15},
            {'Q', 0.10},
            {'Z', 0.07}
        };
        
        public static Dictionary<char, double> EnglishLetterFreq2 = new()
        {
            {'A',0.0651738 * 100},
            {'B', 0.0124248  * 100},
            {'C', 0.0217339  * 100},
            {'D', 0.0349835  * 100},
            {'E', 0.1041442  * 100},
            {'F', 0.0197881  * 100},
            {'G', 0.0158610  * 100},
            {'H', 0.0492888  * 100},
            {'I', 0.0558094  * 100},
            {'J', 0.0009033  * 100},
            {'K', 0.0050529  * 100},
            {'L', 0.0331490  * 100},
            {'M', 0.0202124  * 100},
            {'N',0.0564513   * 100},
            {'O', 0.0596302  * 100},
            {'P',  0.0137645 * 100},
            {'Q',  0.0008606 * 100},
            {'R',  0.0497563* 100},
            {'S',  0.0515760 * 100},
            {'T',  0.0729357 * 100},
            {'U',  0.0225134 * 100},
            {'V', 0.0082903  * 100},
            {'W', 0.0171272 * 100},
            {'X', 0.0013692  * 100},
            {'Y', 0.0145984  * 100},
            {'Z', 0.0007836  * 100}
        };

        public static Dictionary<char, double> GetEmptyLettersDictionary() => new()
        {
            {'E', 0},
            {'T', 0},
            {'A', 0},
            {'O', 0},
            {'I', 0},
            {'N', 0},
            {'S', 0},
            {'H', 0},
            {'R', 0},
            {'D', 0},
            {'L', 0},
            {'C', 0},
            {'U', 0},
            {'M', 0},
            {'W', 0},
            {'F', 0},
            {'G', 0},
            {'Y', 0},
            {'P', 0},
            {'B', 0},
            {'V', 0},
            {'K', 0},
            {'J', 0},
            {'X', 0},
            {'Q', 0},
            {'Z', 0}
        };

    }
}