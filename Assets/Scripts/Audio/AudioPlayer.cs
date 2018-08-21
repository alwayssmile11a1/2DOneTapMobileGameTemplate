using System.Collections;
using UnityEngine;

namespace Gamekit2D
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour
    {

        #region Singleton
        public static AudioPlayer Instance
        {
            get
            {
                if (s_Instance != null)
                    return s_Instance;

                s_Instance = FindObjectOfType<AudioPlayer>();

                if (s_Instance != null)
                    return s_Instance;

                //Create new 
                GameObject playerDataObject = new GameObject("AudioPlayer");
                s_Instance = playerDataObject.AddComponent<AudioPlayer>();

                return s_Instance;
            }
        }

        private static AudioPlayer s_Instance;

        #endregion

        private AudioSource m_AudioSource;

        private void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            m_AudioSource = GetComponent<AudioSource>();
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

        public void SetAudioVolume(float volume)
        {
            m_AudioSource.volume = volume;
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