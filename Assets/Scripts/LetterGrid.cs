using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TetrisWorld
{
    public class LetterGrid : MonoBehaviour
    {
        public int               Rows, Columns;
        public LetterGridCell[,] Grid;

        public float   SizeMultiplier = 5;
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

        private Camera _mainCamera;
        private float  _scale  = 0.028f;
        private float  _offset = 0.3f;

        private Vector2 _initialPosition;

        private void StartGame()
        {
            GenerateGrid();
            LetterPoolGenerator.GeneratePool();
            TotalScore = 0;
            Score.SetScore(TotalScore);
            Timer.StartTimer();
            CanvasManager.StartGame();
            LetterBlockController.SetActiveBlock(LetterPoolGenerator.GetAvailableLetter(_initialPosition));
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
            _mainCamera = Camera.main;
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
            float   halfHeight    = _mainCamera.orthographicSize;
            float   halfWidth     = halfHeight * _mainCamera.aspect;
            Vector2 newOffset     = InitialPosition;
            Vector2 adjustedScale = new Vector2(_scale * halfWidth * 2f, _scale * halfWidth * 2f);

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    newOffset.x = -halfWidth  + _offset + (j * adjustedScale.x * SizeMultiplier);
                    newOffset.y = -halfHeight + _offset + (i * adjustedScale.y * SizeMultiplier);
                    Grid[i, j] = new LetterGridCell()
                    {
                        Position = newOffset,
                        Letter   = null
                    };
                    // Letters.LetterPrefab.transform.localScale = adjustedScale;
                    // var obj = Instantiate(Letters.LetterPrefab, Grid[i, j]
                    //     .Position, Quaternion.identity);
                }
            }

            _initialPosition = Grid[Rows - 1, 3]
                .Position;
            _initialPosition.y -= -halfHeight + _offset + (adjustedScale.y * SizeMultiplier);
        }

        public (Vector2, Vector2Int) GetLeft(Vector2Int index)
        {
            if (!GetDown(false, index)
                    .Item3)
            {
                return (Grid[index.x, index.y]
                    .Position, index);
            }

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
            if (!GetDown(false, index)
                    .Item3)
            {
                return (Grid[index.x, index.y]
                    .Position, index);
            }

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
            for (int i = Rows - 1; i >= 0; i--)
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
                                score += 10;
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
                            if (Grid[Rows - 1 - tuple.Item2, columnIndex]
                                .Letter)
                            {
                                Grid[Rows - 1 - tuple.Item2, columnIndex]
                                    .Letter = null;
                                score += 10;
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
                int lastScore            = TotalScore;
                TotalScore += RemoveMatchedPatternRow(matchedPatternRow, index.x);
                TotalScore += RemoveMatchedPatternColumn(matchedPatternColumn, index.y);
                if (lastScore != TotalScore)
                {
                    AudioManager.Instance.PlayTileMatchSound();
                }

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

                LetterBlockController.SetActiveBlock(LetterPoolGenerator.GetAvailableLetter(_initialPosition));
            }
        }

        private void GameEnd()
        {
            AudioManager.Instance.PlayGameOverSound();
            CanvasManager.ShowGameOverCanvas(TotalScore);
        }

        public void Home()
        {
            AudioManager.Instance.PlayButtonPressedSound();
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
            LetterBlockController.SetActiveBlock(LetterPoolGenerator.GetAvailableLetter(_initialPosition));
            AudioManager.Instance.PlayBgMusic();
        }
    }

    [Serializable]
    public class LetterGridCell
    {
        public Vector2 Position;
        public Letter  Letter;
    }
}