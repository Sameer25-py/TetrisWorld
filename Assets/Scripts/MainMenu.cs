using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TetrisWorld
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject Menu,               Settings;
        public Sprite     DefualtMusicSprite, MuteMusicSprite;
        public Image      Music;

        public bool localMusicState = true;
        public void PlayButton()
        {   
            AudioManager.Instance.PlayButtonPressedSound();
            SceneManager.LoadScene("Gameplay", LoadSceneMode.Single);
        }

        public void SettingsButton()
        {   
            AudioManager.Instance.PlayButtonPressedSound();
            Menu.SetActive(false);
            Settings.SetActive(true);
        }

        public void BackToMenuButton()
        {   
            AudioManager.Instance.PlayButtonPressedSound();
            Menu.SetActive(true);
            Settings.SetActive(false);
        }

        public void MusicButton()
        {
            localMusicState = !AudioManager.Instance.MusicState;
            if (localMusicState)
            {
                Music.sprite = DefualtMusicSprite;
            }
            else
            {
                Music.sprite = MuteMusicSprite;
            }

            AudioManager.ChangeSoundState?.Invoke();

        }

        private void Start()
        {   
            AudioManager.Instance.PlayBgMusic();
            if (AudioManager.Instance.MusicState)
            {
                Music.sprite = DefualtMusicSprite;
            }
            else
            {
                Music.sprite = MuteMusicSprite;
            }
        }
    }
}