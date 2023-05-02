using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace TetrisWorld
{
    public class Background : MonoBehaviour
    {
        public static Background Instance;
        public Sprite     ActiveBackground;

        public Backgrounds Backgrounds;
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            
        }

        private void Start()
        {
            ActiveBackground = Backgrounds.Sprites[Random.Range(0, Backgrounds.Sprites.Count)];
            ApplyBackground();
        }

        public void ApplyBackground()
        {
            foreach (Image img in FindObjectsOfType<Image>(true))
            {
                if (img.gameObject.CompareTag("BG"))
                {
                    img.sprite = ActiveBackground;
                }
            }
        }
    }
}