﻿using System.Net;
using UnityEngine;

namespace TetrisWorld
{
    public class LetterBlockController : MonoBehaviour
    {
        public LetterGrid Grid;
        public Letter     ActiveLetterBlock = null;
        public Vector2Int CurrentIndex;
        public Vector2    CurrentPosition, NextPosition;
        public bool       FirstMove   = true;
        public float      elapsedTime = 0f;
        public float      speed       = 0.5f;


        public void SetActiveBlock(Letter letter)
        {
            ActiveLetterBlock = letter;
            FirstMove         = true;
            CurrentPosition   = letter.transform.position;
            NextPosition      = CurrentPosition;
        }

        private void Update()
        {
            if (!ActiveLetterBlock) return;
            
            elapsedTime += Time.deltaTime;
            if (elapsedTime > speed)
            {
                (NextPosition, CurrentIndex) = Grid.GetDown(FirstMove,CurrentIndex);
                CurrentPosition              = ActiveLetterBlock.transform.position;
                elapsedTime                  = 0f;
                FirstMove                    = false;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                (NextPosition, CurrentIndex) = Grid.GetLeft(CurrentIndex);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                (NextPosition, CurrentIndex) = Grid.GetRight(CurrentIndex);
            }
            
            ActiveLetterBlock.transform.position = NextPosition;
        }
    }
}