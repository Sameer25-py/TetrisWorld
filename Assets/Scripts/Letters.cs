using System.Collections.Generic;
using UnityEngine;

namespace TetrisWorld
{
    [CreateAssetMenu(fileName = "Letters", menuName = "Letters", order = 0)]
    public class Letters : ScriptableObject
    {
        public List<Sprite> LetterSprites;
        public GameObject   LetterPrefab;


        public Sprite GetRandomLetterSprite()
        {
            return LetterSprites[Random.Range(0, LetterSprites.Count)];
        }
    }
}