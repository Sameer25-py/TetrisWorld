using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
        public LetterPatternChecker  LetterPatternChecker;

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

        private List<Letter> GetRowAroundIndex(int rowIndex)
        {
            List<Letter> letters = new();
            for (int i = 0; i < Columns; i++)
            {
                letters.Add(Grid[rowIndex, i]
                    .Letter);
            }

            return letters;
        }

        private List<Letter> GetColumnAroundIndex(int columnIndex)
        {
            List<Letter> letters = new();
            for (int i = 0; i < Rows - 1; i++)
            {
                letters.Add(Grid[i,columnIndex].Letter);
            }

            return letters;
        }

        private void RemoveMatchedPatternRow(List<Pattern> matchedPatterns,int rowIndex)
        {
            if (matchedPatterns.Count > 0)
            {
                foreach (Pattern pattern in matchedPatterns)
                {
                    foreach (Tuple<Letter, int> tuple in pattern.Combination)
                    {
                        if (tuple.Item1)
                        {
                            Grid[rowIndex, tuple.Item2]
                                .Letter = null;
                            LetterPoolGenerator.AddLetterBackToPool(tuple.Item1);
                        }
                    }
                }
            }
        }
        
        private void RemoveMatchedPatternColumn(List<Pattern> matchedPatterns,int columnIndex)
        {
            if (matchedPatterns.Count > 0)
            {
                foreach (Pattern pattern in matchedPatterns)
                {
                    foreach (Tuple<Letter, int> tuple in pattern.Combination)
                    {
                        if (tuple.Item1)
                        {
                            Grid[tuple.Item2,columnIndex]
                                .Letter = null;
                            LetterPoolGenerator.AddLetterBackToPool(tuple.Item1);
                        }
                    }
                }
            }
        }
        
        public void AddLetterToGrid(Vector2Int index, Letter letter)
        {
            if ((index.x < Rows && index.x >= 0) && (index.y < Columns && index.y >= 0) && !Grid[index.x, index.y]
                    .Letter)
            {
                Grid[index.x, index.y]
                    .Letter = letter;
                
                var matchedPatternRow    = LetterPatternChecker.MatchPattern(GetRowAroundIndex(index.x));
                var matchedPatternColumn = LetterPatternChecker.MatchPattern(GetColumnAroundIndex(index.y));
                RemoveMatchedPatternRow(matchedPatternRow,index.x);
                RemoveMatchedPatternColumn(matchedPatternColumn,index.y);
                
                for (int i = 0; i < Columns; i++)
                {
                    if (Grid[Rows - 1, i]
                        .Letter)
                    {
                        GenerateGrid();
                        LetterPoolGenerator.ResetPool();
                        break;
                    }
                }

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