using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Enums;
using ScriptableObjects.GameEvents;

namespace AudioManger
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;
        
        [Header("Events")]
        [SerializeField] private NullEvent playButtonSoundEvent;
        [SerializeField] private NullEvent playUiMoveSound;

        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("Clips")]
        [SerializeField] private List<SoundClip> soundClips;

        private Dictionary<SoundType, AudioClip> clipMap;

        private void Awake()
        {

            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);


            clipMap = new Dictionary<SoundType, AudioClip>();
            foreach (var s in soundClips)
                clipMap[s.type] = s.clip;
        }

        private void OnEnable()
        {

            playButtonSoundEvent.OnEventRaised += PlayButtonSound;
            playUiMoveSound.OnEventRaised += PlayMoveUISound;


            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {

            playButtonSoundEvent.OnEventRaised -= PlayButtonSound;
            playUiMoveSound.OnEventRaised -= PlayMoveUISound;


            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void PlayButtonSound()
        {
            PlaySFX(SoundType.ButtonClick);
        }

        private void PlayMoveUISound()
        {
            PlaySFX(SoundType.UIMove);
        }

        public void PlaySFX(SoundType type)
        {
            if (!clipMap.ContainsKey(type)) return;
            sfxSource.PlayOneShot(clipMap[type]);
        }


        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            switch (scene.name)
            {
                case "Scene_Game":
                    FadeMusicTo(SoundType.GameSceneMusic, 1f);
                    break;

                default:
                    FadeMusicTo(SoundType.MenuSceneMusic, 1f);
                    break;
            }
        }


        


        public void FadeMusicTo(SoundType type, float duration = 1f)
        {
            if (!clipMap.ContainsKey(type)) return;


            if (musicSource.isPlaying && musicSource.clip == clipMap[type])
                return;

            StartCoroutine(FadeMusicCoroutine(type, duration));
        }


        private System.Collections.IEnumerator FadeMusicCoroutine(SoundType type, float duration)
        {
            float startVolume = musicSource.volume;
            
            float t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
                yield return null;
            }
            
            musicSource.clip = clipMap[type];
            musicSource.Play();
            
            t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(0f, startVolume, t / duration);
                yield return null;
            }
        }
    }

    [Serializable]
    public class SoundClip
    {
        public SoundType type;
        public AudioClip clip;
    }
}
