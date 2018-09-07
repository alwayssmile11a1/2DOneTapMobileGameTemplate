using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamekit2D
{
    public class AudioPlayerWrapper : MonoBehaviour {


        public void PlayAudio(AudioClip audioClip)
        {
            AudioPlayer.Instance.PlayAudio(audioClip);
        }

        public void Mute()
        {
            AudioPlayer.Instance.Mute();
        }

        public void UnMute()
        {
            AudioPlayer.Instance.UnMute();
        }

        public void ToggleMute()
        {
            AudioPlayer.Instance.ToggleMute();
        }

        public void SetAudioVolume(float volume)
        {
            AudioPlayer.Instance.SetAudioVolume(volume);
        }

    }
}