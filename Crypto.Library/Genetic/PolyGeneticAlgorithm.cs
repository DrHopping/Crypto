using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Crypto.Library.Analysis;
using Crypto.Library.Ciphers;
using Crypto.Library.Helpers;

namespace Crypto.Library.Genetic
{
    public class PolyGeneticAlgorithm
    {
        private static Random _rand = new();
        private WeightedRandom<List<char[]>> _weightedRandom;
        public double MutationProbability { get; set; } = 0.5;
        public int Mutations { get; set; } = 10;
        public int MutationSwaps { get; set; } = 1;
        private int GenerationSize { get; }
        private SortedList<double, List<char[]>> CurrentGeneration { get; }
        private string Text { get; }
        private List<(NGrams nGram, double coef)> NGrams { get; }
        public int KeyLength { get; }

        public PolyGeneticAlgorithm(int generationSize, string text, int keyLength)
        {
            GenerationSize = generationSize;
            Text = text;
            KeyLength = keyLength;
            NGrams = new List<(NGrams, double)>{ (new(3), 0.6), (new(2), 0.4) };
            CurrentGeneration = CreateInitialGeneration();
            _weightedRandom = new WeightedRandom<List<char[]>>(CurrentGeneration);
        }

        private SortedList<double, List<char[]>> CreateInitialGeneration()
        {
            var keys = new List<char[]>();
            var generation = new SortedList<double, List<char[]>>(new DuplicateKeyComparer<double>());
            for (var i = 0; i < GenerationSize * KeyLength; i++)
            {
                char[] key;
                do
                {
                    key = Constants.Letters.OrderBy(_ => _rand.Next()).ToArray();
                } while (keys.Any(x => x.SequenceEqual(key)));

                keys.Add(key);
            }

            for (var i = 0; i < GenerationSize; i++)
            {
                var key = new List<char[]>();
                for (var j = 0; j < KeyLength; j++)
                {
                    key.Add(keys[i + j]);
                }
                generation.Add(CalculateFitness(key), key);
            }
            
            Seed(generation);
            
            return generation;
        }

        private void Seed(SortedList<double, List<char[]>> generation)
        {
            var seed1 = new List<char[]>
            {
                "TEUIJYSPBGAOHFKNDLMZRVQWXC".ToCharArray(), 
                "YQJBEUNIDPFLATMRXHOKZCWSVG".ToCharArray(),
                "OQFPASKZHCLTJUIXVDYRGEWBMN".ToCharArray(), 
                "LJGONRPKCBIHDTUEFSQYXAZVMW".ToCharArray()
            };
            generation.Add(CalculateFitness(seed1), seed1);
        }

        private double CalculateFitness(List<char[]> key)
        {
            var decrypted = PolySubstitutionCipher.Decrypt(Text, key);
            double score = NGrams.Sum(nGram => nGram.nGram.GetScore(decrypted) * nGram.coef);
            return score;
        }

        public KeyValuePair<double, List<char[]>> RunEpoch()
        {
            var isCrossover = _rand.NextDouble() > MutationProbability;

            if (isCrossover)
            {
                List<char[]> child1;
                List<char[]> child2;
                var parents = _weightedRandom.NextN(2);
                child1 = Crossover(parents[0], parents[1]);
                child2 = Crossover(parents[1], parents[0]);
                CurrentGeneration.Remove(0);
                CurrentGeneration.Remove(1);
                CurrentGeneration.Add(CalculateFitness(child1), child1);
                CurrentGeneration.Add(CalculateFitness(child2), child2);
            }
            else
            {
                for (int i = 0; i < Mutations; i++)
                {
                    var toMutate = _weightedRandom.Next();
                    var mutated = Mutate(toMutate);
                    CurrentGeneration.Remove(0);
                    CurrentGeneration.Add(CalculateFitness(mutated), mutated);
                }
            }

            return CurrentGeneration.Last();
        }

        public KeyValuePair<double, List<char[]>> Run(int generations)
        {
            for (var i = 0; i < generations - 1; i++) RunEpoch();
            return RunEpoch();
        }
        private List<char[]> Mutate(List<char[]> key)
        {
            var keys = new List<char[]>();
            for (int j = 0; j < KeyLength; j++)
            {
                var mutatedKey = new char[key[j].Length];
                Array.Copy(key[j], mutatedKey, key[j].Length);
                for (var i = 0; i < MutationSwaps; i++)
                {
                    var index1 = _rand.Next(0, key[j].Length);
                    var index2 = _rand.Next(0, key[j].Length);
                    while (index1 == index2) index2 = _rand.Next(0, key[j].Length);
                    (mutatedKey[index1], mutatedKey[index2]) = (mutatedKey[index2], mutatedKey[index1]);
                }
                keys.Add(mutatedKey);                
            }
            return keys;
        }

        private List<char[]> Crossover(List<char[]> key1, List<char[]> key2)
        {
            var keys = new List<char[]>();
            for (int j = 0; j < KeyLength; j++)
            {
                var key = new char[key2[j].Length];
                Array.Copy(key2[j], key, key2[j].Length);
                for (var i = 0; i < key.Length / 2; i++)
                {
                    var swapIndex = Array.IndexOf(key, key1[j][i]);
                    (key[i], key[swapIndex]) = (key[swapIndex], key[i]);
                }
                keys.Add(key);
            }

            return keys;
        }
    }
    
    
    
}