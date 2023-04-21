using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WordsDictionary", menuName = "WordsDictionary", order = 0)]
public class WordsDictionary : ScriptableObject
{
    public List<Words> Dictionary;


    public List<string> GetWordsListByLetterCount(int letterCount)
    {
        return Dictionary[letterCount - 3].WordsList;
    }
}

[Serializable]
public class Words
{
    public List<string> WordsList;
}
