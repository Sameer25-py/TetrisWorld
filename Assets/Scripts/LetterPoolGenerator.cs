using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TetrisWorld
{
    public class LetterPoolGenerator : MonoBehaviour
    {
        public int          PoolLength = 60;
        public Letters      Letters;
        public List<Letter> LetterPool;

        private void GeneratePool()
        {
            LetterPool = new();
            for (int i = 0; i < PoolLength; i++)
            {
                GameObject letterObj = Instantiate(Letters.LetterPrefab);
                letterObj.SetActive(false);
                Letter letter             = letterObj.GetComponent<Letter>();
                Sprite randomLetterSprite = Letters.GetRandomLetterSprite();
                letter.PrefabLetter = randomLetterSprite.name;

                letterObj.GetComponent<SpriteRenderer>()
                    .sprite = randomLetterSprite;

                LetterPool.Add(letter);
            }
        }

        private void Start()
        {
            GeneratePool();
        }

        private void OnDestroy()
        {
            foreach (Letter letter in LetterPool.Where(letter => letter))
            {
                Destroy(letter.gameObject);
            }
        }
    }
}