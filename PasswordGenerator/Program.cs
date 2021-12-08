using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using CsvHelper;
using PasswordGenerator.Helpers;


namespace PasswordGenerator
{
    class Program
    {

        private static int _passwordsCount = 100000;
        static void Main(string[] args)
        {
            var hasher = new Hasher();
            var generator = new Helpers.PasswordGenerator(0.05, 0.05, 0.10, 0.8);
            var passwords = generator.GeneratePasswords(_passwordsCount);

            SavePasswords("md5", passwords, hasher.ComputeMd5Hash);
            SavePasswords("argon", passwords, hasher.ComputeArgon2IdHash);
        }

        static void SavePasswords(string fileName, List<string> passwords, Func<string, string> hashingFunction)
        {
            var hashes = passwords.Select(hashingFunction).Select(x => new {Hash = x});
            using var writer = File.CreateText($"../../../../PasswordGenerator/Output/{fileName}.csv");
            var csvWriter = new CsvWriter(writer, CultureInfo.CurrentCulture);
            csvWriter.WriteRecords(hashes); 
        }

    }
        
}