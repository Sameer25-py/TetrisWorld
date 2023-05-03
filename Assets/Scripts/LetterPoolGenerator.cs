using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TetrisWorld
{
    public class LetterPoolGenerator : MonoBehaviour
    {
        public  int             PoolLength = 60;
        public  Letters         Letters;
        public  List<Letter>    LetterPool;
        public  Vector2         InitialPosition = new Vector2(-0.00999998301f, 3.6900003f);
        public  WordsDictionary WordsDictionary;
        private Camera          _mainCamera;

        public  int   PoolIndex = 0;
        private float _scale    = 0.028f;

        public void GeneratePool()
        {   
            //todo: use scale from letter grid and initialposition from letter grid
            List<string> randomWords = WordsDictionary.GetRandomWords(PoolLength);
            LetterPool = new();
            float   halfHeight    = _mainCamera.orthographicSize;
            float   halfWidth     = halfHeight * _mainCamera.aspect;
            Vector2 adjustedScale = new Vector2(_scale * halfWidth * 2f, _scale * halfWidth * 2f);

            for (int i = 0; i < randomWords.Count; i++)
            {
                for (int j = 0;
                     j < randomWords[i]
                         .Length;
                     j++)
                {
                    GameObject letterObj = Instantiate(Letters.LetterPrefab);
                    letterObj.SetActive(false);
                    letterObj.transform.localScale = adjustedScale;
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
            _mainCamera = Camera.main;
        }

        private void OnDestroy()
        {
            foreach (Letter letter in LetterPool.Where(letter => letter))
            {
                Destroy(letter.gameObject);
            }
        }

        public Letter GetAvailableLetter(Vector2 initialPosition, Vector2Int index = default)
        {
            if (PoolIndex >= LetterPool.Count)
            {
                PoolIndex = 0;
            }
            Letter letter = LetterPool[PoolIndex ++];
            letter.Index              = index;
            letter.transform.position = initialPosition;
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