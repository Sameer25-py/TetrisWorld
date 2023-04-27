using TMPro;
using UnityEngine;

namespace TetrisWorld
{
    public class Score : MonoBehaviour
    {
        public TMP_Text Text;

        public void SetScore(int score)
        {   
            Text.text = score.ToString();
        }
    }
}