using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        public Score                 Score;
        public Timer                 Timer;
        public int                   TotalScore = 0;
        public GameplayCanvasManager CanvasManager;

        private Letter _cachedLetter;

        private void StartGame()
        {
            GenerateGrid();
            TotalScore = 0;
            Score.SetScore(TotalScore);
            Timer.StartTimer();
            CanvasManager.StartGame();
        }

        public void PauseButton()
        {
            Timer.PauseTimer();
            _cachedLetter                           = LetterBlockController.ActiveLetterBlock;
            LetterBlockController.ActiveLetterBlock = null;
            CanvasManager.PauseGame();
        }

        public void ResumeButton()
        {
            Timer.ResumeTimer();
            LetterBlockController.ActiveLetterBlock = _cachedLetter;
            CanvasManager.ResumeGame();
        }

        private void Start()
        {
            StartGame();
            Timer.TimerEnd.AddListener(OnTimerEnd);
        }

        private void OnTimerEnd()
        {
            GameEnd();
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
                letters.Add(Grid[i, columnIndex]
                    .Letter);
            }

            return letters;
        }

        private int RemoveMatchedPatternRow(List<Pattern> matchedPatterns, int rowIndex)
        {
            int score = 0;
            if (matchedPatterns.Count > 0)
            {
                foreach (Pattern pattern in matchedPatterns)
                {
                    foreach (Tuple<Letter, int> tuple in pattern.Combination)
                    {
                        if (tuple.Item1)
                        {
                            if (Grid[rowIndex, tuple.Item2]
                                .Letter)
                            {
                                Grid[rowIndex, tuple.Item2]
                                    .Letter = null;
                                score += 1;
                            }

                            LetterPoolGenerator.AddLetterBackToPool(tuple.Item1);
                        }
                    }
                }
            }

            return score;
        }

        private int RemoveMatchedPatternColumn(List<Pattern> matchedPatterns, int columnIndex)
        {
            int score = 0;
            if (matchedPatterns.Count > 0)
            {
                foreach (Pattern pattern in matchedPatterns)
                {
                    foreach (Tuple<Letter, int> tuple in pattern.Combination)
                    {
                        if (tuple.Item1)
                        {
                            if (Grid[tuple.Item2, columnIndex]
                                .Letter)
                            {
                                Grid[tuple.Item2, columnIndex]
                                    .Letter = null;
                                score += 1;
                            }

                            LetterPoolGenerator.AddLetterBackToPool(tuple.Item1);
                        }
                    }
                }
            }

            return score;
        }

        private void RearrangeGrid()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (!Grid[i, j]
                            .Letter)
                    {
                        for (int k = i + 1; k < Rows; k++)
                        {
                            if (Grid[k, j]
                                .Letter)
                            {
                                Grid[k - 1, j]
                                    .Letter = Grid[k, j]
                                    .Letter;
                                Grid[k - 1, j]
                                    .Letter.transform.position = Grid[k - 1, j]
                                    .Position;
                                Grid[k, j]
                                    .Letter = null;
                            }
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
                TotalScore += RemoveMatchedPatternRow(matchedPatternRow, index.x);
                TotalScore += RemoveMatchedPatternColumn(matchedPatternColumn, index.y);
                Score.SetScore(TotalScore);
                RearrangeGrid();

                for (int i = 0; i < Columns; i++)
                {
                    if (Grid[Rows - 1, i]
                        .Letter)
                    {
                        GameEnd();
                        break;
                    }
                }

                LetterBlockController.SetActiveBlock(LetterPoolGenerator.GetAvailableLetter());
            }
        }

        private void GameEnd()
        {
            CanvasManager.ShowGameOverCanvas(TotalScore);
        }
        
        public void Home()
        {
            SceneManager.LoadScene("Mainmenu", LoadSceneMode.Single);
        }
        public void Restart()
        {
            GenerateGrid();
            LetterPoolGenerator.ResetPool();
            TotalScore = 0;
            Score.SetScore(TotalScore);
            Timer.StartTimer();
            CanvasManager.StartGame();
            LetterBlockController.SetActiveBlock(LetterPoolGenerator.GetAvailableLetter());
        }
    }

    [Serializable]
    public class LetterGridCell
    {
        public Vector2 Position;
        public Letter  Letter;
    }
}