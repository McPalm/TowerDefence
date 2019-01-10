using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    static float volume = 1f;

    public AudioClip PrimaryClip;
    public AudioClip SecondaryClip;


    static AudioClip lastClip;

    private void Start()
    {
        Volume = volume;
    }

    public float Volume
    {
        set
        {
            volume = value;
            GetComponent<AudioSource>().volume = volume;
        }
        get
        {
            return volume;
        }
    }

    public void Play()
    {
        var audio = GetComponent<AudioSource>();
        if (audio.isPlaying == false)
        {
            audio.clip = (lastClip != PrimaryClip) ? PrimaryClip : SecondaryClip;
            lastClip = audio.clip;
            audio.Play();
        }
    }

    public void Stop()
    {
        //GetComponent<AudioSource>().Stop();
        StartCoroutine(FadePitch(.7f, 0f, 7f));
        StartCoroutine(FadeVolume(1f, 0f, 7f));
    }

    IEnumerator FadePitch(float startpitch, float endpitch, float duration)
    {
        var audio = GetComponent<AudioSource>();

        for (float f = 0; f < duration; f += Time.unscaledDeltaTime)
        {
            audio.pitch = Mathf.Lerp(startpitch, endpitch, f / duration);

            yield return null;
        }
        audio.pitch = endpitch;
    }

    IEnumerator FadeVolume(float startVolume, float endVolume, float duration)
    {
        var audio = GetComponent<AudioSource>();

        for (float f = 0; f < duration; f += Time.unscaledDeltaTime)
        {
            audio.volume = Mathf.Lerp(startVolume, endVolume, f / duration);

            yield return null;
        }
        audio.volume = endVolume;
    }
}
