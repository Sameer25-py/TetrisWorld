using TMPro;
using UnityEngine;

namespace TetrisWorld
{
    public class GameplayCanvasManager : MonoBehaviour
    {
        public GameObject GameplayCanvas, PauseCanvas, GameOverCanvas;
        public TMP_Text   ScoreText;


        public void PauseGame()
        {
            GameplayCanvas.SetActive(false);
            PauseCanvas.SetActive(true);
        }

        public void ResumeGame()
        {
            GameplayCanvas.SetActive(true);
            PauseCanvas.SetActive(false);
        }

        public void ShowGameOverCanvas(int score)
        {
            GameplayCanvas.SetActive(false);
            GameOverCanvas.SetActive(true);
            ScoreText.text = "Score : " + score;
        }

        public void StartGame()
        {
            GameplayCanvas.SetActive(true);
            GameOverCanvas.SetActive(false);
            PauseCanvas.SetActive(false);
        }
    }
}