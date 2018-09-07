using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamekit2D
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicPlayer : MonoBehaviour
    {

        #region Singleton
        public static MusicPlayer Instance
        {
            get
            {
                if (s_Instance != null)
                    return s_Instance;

                s_Instance = FindObjectOfType<MusicPlayer>();

                if (s_Instance != null)
                    return s_Instance;

                //Create new 
                GameObject playerDataObject = new GameObject("MusicPlayer");
                s_Instance = playerDataObject.AddComponent<MusicPlayer>();

                return s_Instance;
            }
        }

        private static MusicPlayer s_Instance;

        #endregion

        private AudioSource m_AudioSource;

        private void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            m_AudioSource = GetComponent<AudioSource>();
        }

        public void Play()
        {
            m_AudioSource.Stop();
            m_AudioSource.Play();
        }

        public void Continue()
        {
            m_AudioSource.Play();
        }

        public void SetAudio(AudioClip clip)
        {
            m_AudioSource.clip = clip;
        }

        public void PlayAudio(AudioClip audioClip)
        {
            m_AudioSource.clip = audioClip;
            m_AudioSource.Play();
        }

        public void PlayAudio(AudioClip audioClip, float delayTime)
        {
            StartCoroutine(InternalPlayAudioWithDelay(audioClip, delayTime));
        }

        public void Mute()
        {
            m_AudioSource.mute = true;
        }

        public void UnMute()
        {
            m_AudioSource.mute = false;
        }

        public void ToggleMute()
        {
            m_AudioSource.mute = !m_AudioSource.mute;
        }

        public void Mute(bool mute)
        {
            m_AudioSource.mute = mute;
        }

        public void SetAudioVolume(float volume)
        {
            m_AudioSource.volume = volume;
        }

        public void Pause()
        {
            m_AudioSource.Pause();
        }

        public void Stop()
        {
            m_AudioSource.Stop();
        }

        public bool IsPlaying()
        {
           return m_AudioSource.isPlaying;
        }

        public void SetTime(float time)
        {
            m_AudioSource.time = time;
        }

        public float GetClipLength()
        {
            return m_AudioSource.clip.length;
        }

        private IEnumerator InternalPlayAudioWithDelay(AudioClip audioClip, float delayTime)
        {
            float m_Timer = delayTime;
            while (m_Timer > 0)
            {
                m_Timer -= Time.deltaTime;
                yield return null;
            }
            m_AudioSource.clip = audioClip;
            m_AudioSource.Play();
        }

    }
}
