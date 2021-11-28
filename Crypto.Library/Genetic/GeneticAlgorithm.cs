using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Crypto.Library.Analysis;
using Crypto.Library.Ciphers;
using Crypto.Library.Helpers;

namespace Crypto.Library.Genetic
{
    public class GeneticAlgorithm
    {
        private static Random _rand = new();
        private WeightedRandom<char[]> _weightedRandom;
        public double MutationProbability { get; set; } = 0.5;
        public int Mutations { get; set; } = 10;
        public int MutationSwaps { get; set; } = 1;
        private int GenerationSize { get; }
        private SortedList<double, char[]> CurrentGeneration { get; }
        private string Text { get; }
        private List<(NGrams nGram, double coef)> NGrams { get; }

        public GeneticAlgorithm(int generationSize, string text)
        {
            GenerationSize = generationSize;
            Text = text;
            NGrams = new List<(NGrams, double)>{ (new(3), 0.5), (new(2), 0.3), (new(1), 0.2) };
            CurrentGeneration = CreateInitialGeneration();
            _weightedRandom = new WeightedRandom<char[]>(CurrentGeneration);
        }

        private SortedList<double, char[]> CreateInitialGeneration()
        {
            var generation = new SortedList<double, char[]>(new DuplicateKeyComparer<double>());
            for (int i = 0; i < GenerationSize; i++)
            {
                char[] key;
                do
                {
                    key = Constants.Letters.OrderBy(_ => _rand.Next()).ToArray();
                } while (generation.Any(x => x.Value.SequenceEqual(key)));

                generation.Add(CalculateFitness(key), key);
            }
            generation.Add(CalculateFitness("EKMFLGDQVZNTOWYHXUSPAIBRCJ".ToCharArray()), "EKMFLGDQVZNTOWYHXUSPAIBRCJ".ToCharArray());
            return generation;
        }

        private double CalculateFitness(char[] key)
        {
            var decrypted = SubstitutionCipher.Decrypt(Text, key);
            double score = NGrams.Sum(nGram => nGram.nGram.GetScore(decrypted) * nGram.coef);
            return score;
        }

        public KeyValuePair<double, char[]> RunEpoch()
        {
            //var keyFitnesses = CurrentGeneration.Select(k => (Key: k, Fitness: CalculateFitness(k))).ToList();
            var isCrossover = _rand.NextDouble() > MutationProbability;

            if (isCrossover)
            {
                //Crossover
                char[] child1;
                char[] child2;
                //do
                //{
                var parents = _weightedRandom.NextN(2);
                child1 = Crossover(parents[0], parents[1]);
                child2 = Crossover(parents[1], parents[0]);
                //} while (CurrentGeneration.Any(k => k.SequenceEqual(child1) || k.SequenceEqual(child2)));
                CurrentGeneration.Remove(0);
                CurrentGeneration.Remove(1);
                CurrentGeneration.Add(CalculateFitness(child1), child1);
                CurrentGeneration.Add(CalculateFitness(child2), child2);
            }
            else
            {
                //Mutation
                for (int i = 0; i < Mutations; i++)
                {
                    var toMutate = _weightedRandom.Next();
                    var mutated = Mutate(toMutate);
                    //while (CurrentGeneration.Any(k => k.SequenceEqual(mutated))) mutated = Mutate(mutated);
                    CurrentGeneration.Remove(0);
                    CurrentGeneration.Add(CalculateFitness(mutated), mutated);
                }
            }

            return CurrentGeneration.Last();
        }

        public KeyValuePair<double, char[]> Run(int generations)
        {
            for (var i = 0; i < generations - 1; i++) RunEpoch();
            return RunEpoch();
        }
        private char[] Mutate(char[] key)
        {
            var mutatedKey = new char[key.Length];
            Array.Copy(key, mutatedKey, key.Length);
            for (var i = 0; i < MutationSwaps; i++)
            {
                var index1 = _rand.Next(0, key.Length);
                var index2 = _rand.Next(0, key.Length);
                while (index1 == index2) index2 = _rand.Next(0, key.Length);
                (mutatedKey[index1], mutatedKey[index2]) = (mutatedKey[index2], mutatedKey[index1]);
            }

            return mutatedKey;
        }

        private char[] Crossover(char[] key1, char[] key2)
        {
            var key = new char[key2.Length];
            Array.Copy(key2, key, key2.Length);
            for (var i = 0; i < key1.Length / 4; i++)
            {
                var swapIndex = Array.IndexOf(key, key1[i]);
                (key[i], key[swapIndex]) = (key[swapIndex], key[i]);
            }

            return key;
        }
    }
    
    
    
}