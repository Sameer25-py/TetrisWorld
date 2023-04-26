using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace TetrisWorld
{
    public class Timer : MonoBehaviour
    {
        public float    countDownTime = 45;
        public TMP_Text CountDownText;

        private bool  _isCountDownInProgress;
        private float _elapsedTime;
        private float _remainingTime;

        public static UnityEvent TimerEnd = new();

        private void DecrementTimer()
        {
            _remainingTime -= 1f;
            int seconds = Mathf.FloorToInt(_remainingTime % 60f);
            int minutes = Mathf.FloorToInt(_remainingTime / 60f);
            CountDownText.text = $"{0:00}:{minutes:00}:{seconds:00}";

            if (_remainingTime == 0f)
            {
                _isCountDownInProgress = false;
                TimerEnd?.Invoke();
            }
        }
        
        private void Update()
        {
            if (!_isCountDownInProgress) return;
            _elapsedTime += Time.deltaTime;
            if (!(_elapsedTime >= 1f)) return;
            _elapsedTime = 0f;
            DecrementTimer();
        }

        public void StartTimer()
        {
            _remainingTime = countDownTime;
            int seconds = Mathf.FloorToInt(_remainingTime % 60f);
            int minutes = Mathf.FloorToInt(_remainingTime / 60f);
            CountDownText.text     = $"{0:00}:{minutes:00}:{seconds:00}";
            _isCountDownInProgress = true;
        }

        public void PauseTimer()
        {
            _isCountDownInProgress = false;
        }

        public void ResumeTimer()
        {
            _isCountDownInProgress = true;
        }
    }
}