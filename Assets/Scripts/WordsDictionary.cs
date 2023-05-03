using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "WordsDictionary", menuName = "WordsDictionary", order = 0)]
public class WordsDictionary : ScriptableObject
{
    public List<Words>  Dictionary;
    public List<string> ValidLetters;


    public List<string> GetWordsListByLetterCount(int letterCount)
    {
        return Dictionary[letterCount - 3].WordsList;
    }

    public List<string> GetRandomWords(int randomWordCount)
    {
        List<string> randomWords = new();
        for (int i = 0; i < randomWordCount; i++)
        {
            int randomDictionaryList = Random.Range(0, Dictionary.Count);
            int randomWordList = Random.Range(0, Dictionary[randomDictionaryList]
                .WordsList.Count);
            
            randomWords.Add(Dictionary[randomDictionaryList].WordsList[randomWordList]);
        }

        return randomWords;
    }

    public List<string> GetRandomValidLetters(int count)
    {
        List<string> randomWords = new();
        for (int i = 0; i < count; i++)
        {
            randomWords.Add(ValidLetters[Random.Range(0,ValidLetters.Count)]);
        }

        return randomWords;
    }
}

[Serializable]
public class Words
{
    public List<string> WordsList;
}
