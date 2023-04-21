using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TetrisWorld
{
    public class LetterPatternChecker : MonoBehaviour
    {
        public string ValidPattern = "AAA";

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
                        pattern.Combination.Add(new Tuple<Letter, Index>
                            (Letters[k], k));
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
            return null;
        }
    }

    [Serializable]
    public class Pattern
    {
        public bool                       Matched     = false;
        public List<Tuple<Letter, Index>> Combination = new();


        public string getWordFromCombination()
        {
            return Combination.Aggregate("", (current, tuple) =>
                current + tuple.Item1.PrefabLetter);
        }
    }
}