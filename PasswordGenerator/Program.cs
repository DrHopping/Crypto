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
        static void Main(string[] args)
        {
            var hasher = new Hasher();
            var generator = new Helpers.PasswordGenerator(0.05, 0.05, 0.10, 0.8);
            SaveArgonPasswords(hasher, generator, 100000);
            SaveMd5(hasher, generator, 100000);

        }

        static void SaveArgonPasswords(Hasher hasher, Helpers.PasswordGenerator generator, int amount)
        {
            var passwords = generator.GeneratePasswords(amount);
            var hashes = passwords.Select(hasher.ComputeArgon2IdHash).Select(x => new {x.Hash, x.Salt});
            using var writer = File.CreateText(@"../../../../PasswordGenerator/Output/argon.csv");
            var csvWriter = new CsvWriter(writer, CultureInfo.CurrentCulture);
            csvWriter.WriteRecords(hashes);
        }

        static void SaveMd5(Hasher hasher, Helpers.PasswordGenerator generator, int amount)
        {
            var passwords = generator.GeneratePasswords(amount);
            var hashes = passwords.Select(hasher.ComputeMd5Hash).Select(x => new {Hash = x});
            using var writer = File.CreateText(@"../../../../PasswordGenerator/Output/md5.csv");
            var csvWriter = new CsvWriter(writer, CultureInfo.CurrentCulture);
            csvWriter.WriteRecords(hashes);
        }
        
    }
        
}