using System;
using UnityEngine;
using UnityEngine.Events;

namespace TetrisWorld
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        public AudioSource AudioSource1, AudioSource2;

        public AudioClip ButtonPressClip, TileMatchClip, GameOverClip;

        public static UnityEvent ChangeSoundState = new();

        public bool MusicState = true;

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

            ChangeSoundState.AddListener(OnSoundStateChanged);
        }

        private void OnSoundStateChanged()
        {
            MusicState        = !MusicState;
            AudioSource1.mute = !MusicState;
            AudioSource2.mute = !MusicState;
        }

        public void PlayButtonPressedSound()
        {
            if (AudioSource1.clip == TileMatchClip && AudioSource1.isPlaying)
            {
                return;
            }

            AudioSource1.clip = ButtonPressClip;
            AudioSource1.Play();
        }

        public void PlayTileMatchSound()
        {
            AudioSource1.clip = TileMatchClip;
            AudioSource1.Play();
        }

        public void PlayGameOverSound()
        {
            AudioSource1.clip = GameOverClip;
            AudioSource1.Play();
            AudioSource2.Stop();
        }

        public void PlayBgMusic()
        {
            AudioSource2.Play();
        }
    }
}