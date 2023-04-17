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

        public LetterBlockController LetterBlockController;
        public LetterPoolGenerator   LetterPoolGenerator;

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
            Vector2Int newIndex = new Vector2Int(index.x, index.y - 1 >= 0 ? index.y - 1 : 0);
            if (Grid[newIndex.x, newIndex.y]
                .Letter)
            {
                newIndex = index;
            }

            return (Grid[newIndex.x, newIndex.y]
                .Position, newIndex);
        }

        public (Vector2, Vector2Int) GetRight(Vector2Int index)
        {
            Vector2Int newIndex = new Vector2Int(index.x, index.y + 1 < Columns ? index.y + 1 : Columns - 1);
            if (Grid[newIndex.x, newIndex.y]
                .Letter)
            {
                newIndex = index;
            }

            return (Grid[newIndex.x, newIndex.y]
                .Position, newIndex);
        }

        public (Vector2, Vector2Int, bool) GetDown(bool isFirstMove, Vector2Int index)
        {
            if (isFirstMove)
            {
                index = new Vector2Int(Rows - 1, 3);
                return (Grid[Rows - 1, 3]
                    .Position, index, true);
            }

            Vector2Int newIndex = new Vector2Int(index.x - 1 > 0 ? index.x - 1 : 0, index.y);

            bool isValidMove = !(index.x == 0 || Grid[newIndex.x, newIndex.y]
                .Letter);
            return (Grid[newIndex.x, newIndex.y]
                .Position, newIndex, isValidMove);
        }

        public void AddLetterToGrid(Vector2Int index, Letter letter)
        {
            if ((index.x < Rows && index.x >= 0) && (index.y < Columns && index.y >= 0) && !Grid[index.x, index.y]
                    .Letter)
            {
                Grid[index.x, index.y]
                    .Letter = letter;

                LetterBlockController.SetActiveBlock(LetterPoolGenerator.GetAvailableLetter());
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