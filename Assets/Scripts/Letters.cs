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

        public Sprite GetSpriteByName(string name)
        {
            foreach (Sprite letterSprite in LetterSprites)
            {
                if (letterSprite.name == name)
                {
                    return letterSprite;
                }
            }

            return GetRandomLetterSprite();
        }
    }
}