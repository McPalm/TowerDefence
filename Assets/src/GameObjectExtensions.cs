using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Extensions
{
    static public void PlaySound(this GameObject o, AudioClip clip, float volume = 1f, float pitch = 0f) => ObjectPooling.AudioPool.PlaySound(o.transform.position, clip, volume, pitch);
    static public void PlaySound(this Component c, AudioClip clip, float volume = 1f, float pitch = 0f) => ObjectPooling.AudioPool.PlaySound(c.transform.position, clip, volume, pitch);

    static public void PlaySoundUnscaled(this GameObject o, AudioClip clip, float volume = 1f) => ObjectPooling.AudioPool.PlaySound(o.transform.position, clip, 1f, 1f, false);
    static public void PlaySoundUnscaled(this Component o, AudioClip clip, float volume = 1f) => ObjectPooling.AudioPool.PlaySound(o.transform.position, clip, 1f, 1f, false);
}
