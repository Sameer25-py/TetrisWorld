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
                    // Instantiate(Letters.LetterPrefab, Grid[i, j]
                    //     .Position, Quaternion.identity);
                    newOffset.x += Letters.LetterPrefab.transform.localScale.x * SizeMultiplier + PositionOffset.x;
                }

                newOffset.x =  InitialPosition.x;
                newOffset.y += Letters.LetterPrefab.transform.localScale.y * SizeMultiplier + PositionOffset.y;
            }
        }

        public (Vector2, Vector2Int) GetLeft(Vector2Int index)
        {
            index = new Vector2Int(index.x, index.y - 1 >= 0 ? index.y - 1 : 0);
            return (Grid[index.x, index.y]
                .Position, index);
        }

        public (Vector2, Vector2Int) GetRight(Vector2Int index)
        {
            index = new Vector2Int(index.x, index.y + 1 < Columns ? index.y + 1 : Columns - 1);
            return (Grid[index.x, index.y]
                .Position, index);
        }

        public (Vector2, Vector2Int) GetDown(Vector2Int index = default)
        {
            if (index == default)
            {
                index = new Vector2Int(Rows - 1, 3);
                return (Grid[Rows - 1, 3]
                    .Position, index);
            }

            index = new Vector2Int(index.x - 1 > 0 ? index.x - 1 : 0, index.y);
            return (Grid[index.x, index.y]
                .Position, index);
        }
    }

    [Serializable]
    public class LetterGridCell
    {
        public Vector2 Position;
        public Letter  Letter;
    }
}