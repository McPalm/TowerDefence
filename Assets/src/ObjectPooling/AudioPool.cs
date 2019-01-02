using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooling
{
    public class AudioPool : ObjectPool
    {
        static AudioPool _instance;
        void Start() => _instance = this;

        public bool mute = false;

        static public void PlaySound(Vector3 position, AudioClip clip, float volume = 1f, float pitch = 0f, bool timescale = true) => _instance.IPlaySound(position, clip, volume, pitch, timescale);
        void IPlaySound(Vector3 position, AudioClip clip, float volume, float pitch, bool timescale) => StartCoroutine(PlaySoundRoutine(position, clip, volume, pitch, timescale));

        IEnumerator PlaySoundRoutine(Vector3 position, AudioClip clip, float volume, float pitch, bool timescale)
        {
            if (!mute && ActiveObjects < 32)
            {
                var masterVolume = Menu.VolumeControl.volume;
                if (masterVolume > 0f)
                {
                    var audio = Create().GetComponent<AudioSource>();
                    audio.clip = clip;
                    audio.Play();
                    audio.volume = volume * masterVolume;
                    if (pitch == 0f)
                        pitch = .9f + Random.value * .1f;
                    while (audio.isPlaying)
                    {
                        if (timescale)
                            audio.pitch = pitch * Mathf.Min((.5f + Time.timeScale * .5f), 1.25f);
                        yield return null;
                    }
                    Dispose(audio.gameObject);
                }
            }
        }

    }
}