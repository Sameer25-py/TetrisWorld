using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

namespace TetrisWorld
{
    public class DictionaryCreator : MonoBehaviour
    {
        [SerializeField] private TextAsset Text;

        private void Start()
        {
            CreateWordsDictionary(Text.text);
        }

        private void CreateWordsDictionary(string json)
        {
            WordsDict wordsDictionary = JsonConvert.DeserializeObject<WordsDict>(json);
            var       newAsset        = ScriptableObject.CreateInstance<WordsDictionary>();
            newAsset.Dictionary = new();
            foreach (List<string> dictionary in wordsDictionary.Dictionary)
            {
                List<string> upperCaseDict = dictionary.Select(word => word.ToUpper())
                    .ToList();
                Words wordsList = new()
                {
                    WordsList = upperCaseDict
                };
                newAsset.Dictionary.Add(wordsList);
            }
#if UNITY_EDITOR
            AssetDatabase.CreateAsset(newAsset, "Assets/WordsDictionary.asset");
            AssetDatabase.SaveAssets();
#endif
        }
    }
}

[Serializable]
public class WordsDict
{
    public List<List<string>> Dictionary;
}