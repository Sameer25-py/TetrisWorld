using UnityEngine;
using UnityEngine.SceneManagement;

namespace TetrisWorld
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject Menu, Settings;
        public void PlayButton()
        {
            SceneManager.LoadScene("Gameplay", LoadSceneMode.Single);
        }

        public void SettingsButton()
        {
            Menu.SetActive(false);
            Settings.SetActive(true);
        }

        public void BackToMenuButton()
        {
            Menu.SetActive(true);
            Settings.SetActive(false);
        }
    }
}