using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TetrisWorld
{
    public class LetterPatternChecker : MonoBehaviour
    {
        public WordsDictionary WordsDictionary;

        public List<Pattern> GenerateCombinations(List<Letter> Letters)
        {
            List<Pattern> combinations = new();
            for (int i = 0; i < Letters.Count; i++)
            {
                for (int j = i; j < Letters.Count; j++)
                {
                    Pattern pattern = new();
                    for (int k = i; k <= j; k++)
                    {
                        if (!Letters[k])
                        {
                            break;
                        }
                        else
                        {
                            pattern.Combination.Add(new Tuple<Letter, int>
                                (Letters[k], k));
                        }
                    }

                    if (pattern.Combination.Count is >= 3 and <= 7)
                    {
                        combinations.Add(pattern);
                    }
                }
            }

            return combinations;
        }

        public List<Pattern> MatchPattern(List<Letter> Letters)
        {
            List<Pattern> matchedCombinations = new();
            List<Pattern> combinations        = GenerateCombinations(Letters);
            // foreach (Pattern pattern in combinations)
            // {
            //     string word = "";
            //     foreach (var combination in pattern.Combination)
            //     {
            //         word += (combination.Item2 + " " + combination.Item1.PrefabLetter + " ");
            //     }
            //     
            //     Debug.Log(word);
            //     word = "";
            // }
            foreach (Pattern combination in combinations)
            {
                string       word      = combination.getWordFromCombination();
                List<string> wordsList = WordsDictionary.GetWordsListByLetterCount(word.Length);
                
                if (wordsList.Contains(word))
                {
                    matchedCombinations.Add(combination);
                }
            }

            return matchedCombinations;
        }
    }

    [Serializable]
    public class Pattern
    {
        public List<Tuple<Letter, int>> Combination = new();


        public string getWordFromCombination()
        {
            return Combination.Aggregate("", (current, tuple) =>
                current + tuple.Item1.PrefabLetter);
        }
    }
}