using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

namespace TetrisWorld
{
    public class LetterGrid : MonoBehaviour
    {
        public int               Rows, Columns;
        public LetterGridCell[,] Grid;

        public float   SizeMultiplier = 5;
        public Vector2 PositionOffset = new Vector2(0.04f, 0.0f);
        public Letters Letters;
        
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

        public Vector2            InitialPosition;
        public Vector2            InitialPositionRight, InitialPositionDown;
        public LetterBlockManager BlockManager;


        private void StartGame()
        {
            Background.Instance.ApplyBackground();
            GenerateGrid();
            LetterPoolGenerator.GeneratePool();
            TotalScore = 0;
            Score.SetScore(TotalScore);
            Timer.StartTimer();
            CanvasManager.StartGame();
            InitializeBlockLetters();
        }

        public void PauseButton()
        {
            Timer.PauseTimer();
            BlockManager.StartTicking = false;
            CanvasManager.PauseGame();
        }

        public void ResumeButton()
        {
            Timer.ResumeTimer();
            BlockManager.StartTicking = true;
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
            float   halfHeight = _mainCamera.orthographicSize;
            float   halfWidth  = halfHeight * _mainCamera.aspect;
            Vector2 newOffset;
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

            InitialPosition = Grid[Rows - 1, 3]
                .Position;
            InitialPositionRight = InitialPositionDown = InitialPosition;
            InitialPositionRight.x = Grid[0, 4]
                .Position.x;
            InitialPositionDown.y = Grid[Rows - 2, 0]
                .Position.y;
        }

        public void InitializeBlockLetters(int letterCount = 2)
        {
            int blockType = UnityEngine.Random.Range(0, 2);
            if (blockType == 0)
            {
                BlockManager.InitializeBlockControllers(new Vector2Int(Rows - 1, 3), new Vector2Int(Rows - 1, 4), InitialPosition,
                    InitialPositionRight, 0);
            }

            else
            {
                BlockManager.InitializeBlockControllers(new Vector2Int(Rows - 2, 3), new Vector2Int(Rows - 1, 3),
                    InitialPositionDown,
                    InitialPosition, 1);
            }
        }

        public (Vector2, Vector2Int) GetLeft(Letter letter)
        {
            Vector2Int index = letter.Index;
            Vector2Int newIndex = new Vector2Int(index.x, index.y - 1 >= 0 && !Grid[index.x, index.y - 1]
                .Letter
                ? index.y - 1
                : index.y);

            return (Grid[newIndex.x, newIndex.y]
                .Position, newIndex);
        }

        public (Vector2, Vector2Int) GetRight(Letter letter)
        {
            Vector2Int index = letter.Index;
            Vector2Int newIndex = new Vector2Int(index.x, index.y + 1 < Columns && !Grid[index.x, index.y + 1]
                .Letter
                ? index.y + 1
                : index.y);

            return (Grid[newIndex.x, newIndex.y]
                .Position, newIndex);
        }

        public (Vector2, Vector2Int) GetDown(Letter letter)
        {
            Vector2Int index = letter.Index;
            Vector2Int newIndex = new Vector2Int(index.x - 1 >= 0 && !Grid[index.x - 1, index.y]
                .Letter
                ? index.x - 1
                : index.x, index.y);

            return (Grid[newIndex.x, newIndex.y]
                .Position, newIndex);
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

        public void AddLetterToGrid(Letter letter)
        {
            Vector2Int index = letter.Index;
            if ((index.x < Rows && index.x >= 0) && (index.y < Columns && index.y >= 0) && !Grid[index.x, index.y]
                    .Letter)
            {
                Grid[index.x, index.y]
                    .Letter = letter;


                for (int i = 0; i < Columns; i++)
                {
                    if (Grid[Rows - 1, i]
                        .Letter)
                    {   
                        GameEnd();
                        break;
                    }
                }
            }
        }

        public void CheckPattern(Vector2Int index)
        {
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

            InitializeBlockLetters();
        }

        private void GameEnd()
        {   
            AudioManager.Instance.PlayGameOverSound();
            CanvasManager.ShowGameOverCanvas(TotalScore);
            BlockManager.StartTicking = false;
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
            InitializeBlockLetters();
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