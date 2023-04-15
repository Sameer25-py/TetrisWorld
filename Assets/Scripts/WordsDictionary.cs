using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WordsDictionary", menuName = "WordsDictionary", order = 0)]
public class WordsDictionary : ScriptableObject
{
    public List<Words> Dictionary;
}

[Serializable]
public class Words
{
    public List<string> WordsList;
}
