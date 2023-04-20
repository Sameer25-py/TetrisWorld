using System.Net;
using UnityEngine;

namespace TetrisWorld
{
    public class LetterBlockController : MonoBehaviour
    {
        public LetterGrid Grid;
        public Letter     ActiveLetterBlock = null;
        public Vector2Int CurrentIndex,    LastIndex;
        public Vector2    CurrentPosition, NextPosition;
        public bool       FirstMove   = true;
        public float      elapsedTime = 0f;
        public float      speed       = 0.5f;

        private bool _isMoveLeft, _isMoveDown, _isMoveRight;


        public void SetActiveBlock(Letter letter)
        {
            ActiveLetterBlock = letter;
            FirstMove         = true;
            CurrentPosition   = letter.transform.position;
            NextPosition      = CurrentPosition;
            elapsedTime       = 0f;
        }

        public void MoveDown()
        {
            _isMoveDown = true;
        }

        public void MoveLeft()
        {
            _isMoveLeft = true;
        }

        public void MoveRight()
        {
            _isMoveRight = true;
        }

        private void Update()
        {
            if (!ActiveLetterBlock) return;
            
            elapsedTime += Time.deltaTime;
            if (elapsedTime > speed)
            {
                bool isValidMove = true;
                (NextPosition, CurrentIndex, isValidMove) = Grid.GetDown(FirstMove, CurrentIndex);
                if (!isValidMove)
                {
                    Grid.AddLetterToGrid(LastIndex,
                        ActiveLetterBlock);
                    elapsedTime = 0f;
                    return;
                }

                CurrentPosition = ActiveLetterBlock.transform.position;
                elapsedTime     = 0f;
                FirstMove       = false;
                speed           = 0.5f;
            }

            if (!FirstMove)
            {
                if (_isMoveLeft)
                {
                    (NextPosition, CurrentIndex) = Grid.GetLeft(CurrentIndex);
                    _isMoveLeft                  = false;
                }

                else if (_isMoveRight)
                {
                    (NextPosition, CurrentIndex) = Grid.GetRight(CurrentIndex);
                    _isMoveRight                 = false;
                }
                else if (_isMoveDown)
                {
                    speed       = 0.1f;
                    _isMoveDown = false;
                }
            }

            ActiveLetterBlock.transform.position = NextPosition;
            LastIndex                            = CurrentIndex;
        }
    }
}