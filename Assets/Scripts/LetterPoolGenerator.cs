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
        public LetterPatternChecker  PatternChecker;

        private void GeneratePool()
        {
            LetterPool = new();
            for (int i = 0; i < 26; i++)
            {
                GameObject letterObj = Instantiate(Letters.LetterPrefab, InitialPosition, Quaternion.identity);
                letterObj.SetActive(false);
                Letter letter             = letterObj.GetComponent<Letter>();
                Sprite randomLetterSprite = Letters.LetterSprites[i];
                letter.PrefabLetter = randomLetterSprite.name;
                letterObj.GetComponent<SpriteRenderer>()
                    .sprite = randomLetterSprite;
                LetterPool.Add(letter);
            }

            List<Letter> letters = new();
            for (int i = 0; i < 7; i++)
            {
                letters.Add(GetAvailableLetter());
            }

            var x = PatternChecker.GenerateCombinations(letters);
            foreach (Pattern pattern in x)
            {
                string word = "";
                foreach (var combination in pattern.Combination)
                {
                    word += (combination.Item2 + " " + combination.Item1.PrefabLetter + " ");
                }
                
                Debug.Log(word);
                word = "";
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
            foreach (Letter letter in LetterPool)
            {
                if (!letter.gameObject.activeSelf)
                {
                    letter.gameObject.SetActive(true);
                    return letter;
                }
            }

            return null;
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