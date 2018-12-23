using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooling
{
    public class AudioPool : ObjectPool
    {
        static AudioPool _instance;
        void Start() => _instance = this;

        static public void PlaySound(Vector3 position, AudioClip clip, float volume = 1f) => _instance.IPlaySound(position, clip, volume);
        void IPlaySound(Vector3 position, AudioClip clip, float volume) => StartCoroutine(PlaySoundRoutine(position, clip, volume));

        IEnumerator PlaySoundRoutine(Vector3 position, AudioClip clip, float volume)
        {
            var audio = Create().GetComponent<AudioSource>();
            audio.clip = clip;
            audio.Play();
            audio.volume = volume;
            float pitch = .9f + Random.value * .1f;
            while (audio.isPlaying)
            {
                audio.pitch = pitch * Mathf.Min((.5f + Time.timeScale * .5f), 1.2f);
                yield return null;
            }
            Debug.Log("done");
            Dispose(audio.gameObject);
        }

    }
}