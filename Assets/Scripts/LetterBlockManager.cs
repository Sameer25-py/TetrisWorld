using System;
using UnityEngine;

namespace TetrisWorld
{
    public class LetterBlockManager : MonoBehaviour
    {
        public Letter              L1, L2;
        public LetterPoolGenerator LetterPoolGenerator;
        public LetterGrid          Grid;

        public  bool  StartTicking = false;
        private float _elpasedTime = 0f;
        public  int   BlockType;

        public float Speed = 0.5f;


        public void InitializeBlockControllers(Vector2Int index1, Vector2Int index2, Vector2 pos1, Vector2 pos2,
            int blockType = 1)
        {
            if (blockType == 1)
            {   
                L2 = LetterPoolGenerator.GetAvailableLetter(pos2, index2);
                L1 = LetterPoolGenerator.GetAvailableLetter(pos1, index1);
            }
            else if(blockType == 0)
            {
                L1 = LetterPoolGenerator.GetAvailableLetter(pos1, index1);
                L2 = LetterPoolGenerator.GetAvailableLetter(pos2, index2);
            }
            else
            {
                L1 = LetterPoolGenerator.GetAvailableLetter(pos1, index1);
                L2 = null;
            }

            BlockType    = blockType;
            StartTicking = true;
            _elpasedTime = 0f;
        }

        public void MoveDown()
        {
            AudioManager.Instance.PlayButtonPressedSound();
            Speed = 0.15f;
        }

        public void MoveLeft()
        {
            AudioManager.Instance.PlayButtonPressedSound();

            var (_, newIndex)  = Grid.GetDown(L1);
            var (_, newIndex1) = Grid.GetDown(L2);

            if (!CheckIndex(newIndex, L1) && !CheckIndex(newIndex1, L2))
            {
                var (newPos3, newIndex3) = Grid.GetLeft(L1);
                var (newPos4, newIndex4) = Grid.GetLeft(L2);

                if (!CheckIndex(newIndex3, L1) && !CheckIndex(newIndex4, L2))
                {
                    L1.Index              = newIndex3;
                    L1.transform.position = newPos3;

                    if (L2)
                    {
                        L2.Index              = newIndex4;
                        L2.transform.position = newPos4;
                    }
   
                }
            }
        }

        public void MoveRight()
        {
            AudioManager.Instance.PlayButtonPressedSound();
            var (_, newIndex)  = Grid.GetDown(L1);
            var (_, newIndex1) = Grid.GetDown(L2);

            if (!CheckIndex(newIndex, L1) && !CheckIndex(newIndex1, L2))
            {
                var (newPos3, newIndex3) = Grid.GetRight(L1);
                var (newPos4, newIndex4) = Grid.GetRight(L2);

                if (!CheckIndex(newIndex4, L2) && !CheckIndex(newIndex3, L1))
                {
                    L1.Index              = newIndex3;
                    L1.transform.position = newPos3;

                    if (L2)
                    {
                        L2.Index              = newIndex4;
                        L2.transform.position = newPos4;
                    }
          
                }
            }
        }

        private bool CheckIndex(Vector2Int index, Letter letter)
        {
            if (!letter) return false;
            return letter.Index == index;
        }

        private void Update()
        {
            if (!StartTicking) return;
            _elpasedTime += Time.deltaTime;
            if (_elpasedTime >= Speed)
            {
                _elpasedTime = 0f;
                Speed        = 0.5f;
                
                var (newPos, newIndex)  = Grid.GetDown(L1);
                var (newPos1, newIndex1) = Grid.GetDown(L2);
                
                if (!CheckIndex(newIndex, L1) && !CheckIndex(newIndex1, L2))
                {
                    L1.Index              = newIndex;
                    L1.transform.position = newPos;

                    if (L2)
                    {
                        L2.Index              = newIndex1;
                        L2.transform.position = newPos1;
                    }
                }
                else
                {   
                    Grid.AddLetterToGrid(L1);
                    Grid.AddLetterToGrid(L2);
                    Grid.CheckPattern(L1.Index);
                }
                
            }
        }
    }
}