using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TetrisWorld
{
    public class LetterPoolGenerator : MonoBehaviour
    {
        public int                   PoolLength = 60;
        public Letters               Letters;
        public List<Letter>          LetterPool;
        public Vector2               InitialPosition = new Vector2(-0.00999998301f, 3.6900003f);
        public LetterBlockController LetterBlockController;
        public WordsDictionary       WordsDictionary;

        public int PoolIndex = 0;

        private void GeneratePool()
        {
            List<string> randomWords = WordsDictionary.GetRandomWords(PoolLength);
            LetterPool = new();

            for (int i = 0; i < randomWords.Count; i++)
            {
                for (int j = 0;
                     j < randomWords[i]
                         .Length;
                     j++)
                {
                    GameObject letterObj = Instantiate(Letters.LetterPrefab, InitialPosition, Quaternion.identity);
                    letterObj.SetActive(false);
                    Letter letter = letterObj.GetComponent<Letter>();
                    Sprite sprite = Letters.GetSpriteByName(randomWords[i][j].ToString());
                    letter.PrefabLetter = sprite.name;
                    letterObj.GetComponent<SpriteRenderer>()
                        .sprite = sprite;
                    LetterPool.Add(letter);
                }
            }
        }

        private void Start()
        {
            GeneratePool();
            LetterBlockController.SetActiveBlock(GetAvailableLetter());
        }

        private void OnDestroy()
        {
            foreach (Letter letter in LetterPool.Where(letter => letter))
            {
                Destroy(letter.gameObject);
            }
        }

        public Letter GetAvailableLetter()
        {
            if (PoolIndex >= LetterPool.Count)
            {
                PoolIndex = 0;
            }
            Letter letter = LetterPool[PoolIndex ++];
            letter.gameObject.SetActive(true);
            return letter;
        }

        public void AddLetterBackToPool(Letter letter)
        {   
            letter.transform.SetAsLastSibling();
            letter.gameObject.SetActive(false);
            letter.transform.position = InitialPosition;
        }

        public void ResetPool()
        {
            foreach (GameObject letterObj in from letter in LetterPool
                     let letterObj = letter.gameObject
                     where letter.gameObject.activeSelf
                     select letterObj)
            {
                letterObj.SetActive(false);
                letterObj.transform.position = InitialPosition;
            }
        }
    }
}