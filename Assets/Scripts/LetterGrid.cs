using System;
using UnityEngine;

namespace TetrisWorld
{
    public class LetterGrid : MonoBehaviour
    {
        public int               Rows, Columns;
        public LetterGridCell[,] Grid;

        public int     SizeMultiplier = 5;
        public Vector2 InitialPosition;
        public Vector2 PositionOffset = new Vector2(0.04f, 0.0f);
        public Letters Letters;

        private void Start()
        {
            GenerateGrid();
        }

        public void GenerateGrid()
        {
            Grid = new LetterGridCell[Rows, Columns];
            Vector2 newOffset = InitialPosition;
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Grid[i, j] = new LetterGridCell()
                    {
                        Position = newOffset,
                        Letter   = null
                    };
                    newOffset.x += Letters.LetterPrefab.transform.localScale.x * SizeMultiplier + PositionOffset.x;
                }

                newOffset.x =  InitialPosition.x;
                newOffset.y += Letters.LetterPrefab.transform.localScale.y * SizeMultiplier + PositionOffset.y;
            }
        }
    }

    [Serializable]
    public class LetterGridCell
    {
        public Vector2 Position;
        public Letter  Letter;
    }
}