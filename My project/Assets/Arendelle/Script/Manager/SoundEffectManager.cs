using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoSingleton<SoundEffectManager>
{
    public AudioSource audioSource;

    public AudioClip plAt;
    public AudioClip accAt;

    public void PlayerAttack()
    {
        audioSource.PlayOneShot(plAt);
    }

    public void AccAttack()
    {
        audioSource.PlayOneShot(accAt);
    }
}
