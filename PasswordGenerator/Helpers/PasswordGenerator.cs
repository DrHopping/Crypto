using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace PasswordGenerator.Helpers
{
    public class PasswordGenerator
    {
        private readonly double _top25Percentage;
        private readonly double _randomPercentage;
        private readonly double _customPercentage;
        private readonly double _top100KPercentage;
        private readonly List<string> _top100KPasswords;

        public PasswordGenerator(double top25Percentage, double randomPercentage, double customPercentage, double top100KPercentage)
        {
            _top25Percentage = top25Percentage;
            _randomPercentage = randomPercentage;
            _customPercentage = customPercentage;
            _top100KPercentage = top100KPercentage;
            _top100KPasswords = File.ReadAllLines("../../../../PasswordGenerator/Files/top-100k-passwords.txt").ToList();
        }
        
        public List<string> GeneratePasswords(int amount)
        {
            return GenerateCustomPasswords((int) (amount * _customPercentage))
                    .Concat(GenerateRandomPasswords((int) (amount * _randomPercentage), 24, 12))
                    .Concat(GenerateTop25Passwords((int) (amount * _top25Percentage)))
                    .Concat(GenerateTop100KPasswords((int) (amount * _top100KPercentage)))
                    .ToList();
        }

        private List<string> GenerateTop25Passwords(int amount)
        {
            return _top100KPasswords.Take(amount).OrderBy(_ => Guid.NewGuid()).Take(amount).ToList();
        }
        
        private List<string> GenerateTop100KPasswords(int amount)
        {
            return _top100KPasswords.OrderBy(_ => Guid.NewGuid()).Take(amount).ToList();
        }
        
        private List<string> GenerateCustomPasswords(int amount)
        {
            var replacementTemplates = new Dictionary<char, string>()
            {
                {'a', "@"},
                {'o', "0"},
                {'O', "0"},
                {'i', "1"},
                {'l', "1"},
                {'e', "3"},
                {'E', "3"},
                {'I', "1"},
                {'s', "$"},
                {'L', "|_"},
                {'K', "|<"},
                {'M', @"|\/|"},
                {'m', @"|\/|"},
                {'W', @"|/\|"},
                {'w', @"|/\|"},
                {'N', @"|\|"},
                {'n', @"|\|"},
            };
            
            var suffixes = new List<string>()
            {
                "1995","1996","1997","1998","1999","2000","2001","2002","2003","2004","2005","2006","2007","2008","2009","2010",
                "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "0", "!", ")", ".", "^", "*", "?", 
                "111", "222", "333", "444", "555", "666", "777", "888", "999", "000"
            };

            var passwords = GenerateTop100KPasswords(amount);
            var customPasswords = 
                passwords.Select(password => 
                    string.Join("", password.Select(x => replacementTemplates.ContainsKey(x) ? replacementTemplates[x] : x.ToString()))
                                             + suffixes[RandomNumberGenerator.GetInt32(0, suffixes.Count)]).ToList();
            return customPasswords;
        }
        
        private List<string> GenerateRandomPasswords(int amount, int maxLength, int minLength)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890!@#$%^&*()_-+={}[]".ToList();

            string GetPassword()
            {
                return Enumerable.Range(0, RandomNumberGenerator.GetInt32(minLength, maxLength))
                    .Aggregate("", (s, _) => s + chars[RandomNumberGenerator.GetInt32(chars.Count)]);
            }

            return Enumerable.Range(0, amount).Select(_ => GetPassword()).ToList();
        }

    }
}