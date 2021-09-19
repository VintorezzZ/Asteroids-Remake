using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    AudioSource audioSource;
    private void Awake()
    {
        instance = this;

        audioSource = GetComponent<AudioSource>();
    }
    
    public void PlayBoomSFX(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

}
