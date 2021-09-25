using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using EventHandler = DefaultNamespace.EventHandler;

public class AudioManager : SingletonBehaviour<AudioManager>
{
    AudioSource audioSource;
    [SerializeField] private AudioMixer masterMixer;
    public float currentVolume = 50;
    private void Awake()
    {
        InitializeSingleton();
        
        //EventHandler.gamePaused += OnGamePaused;
        audioSource = GetComponent<AudioSource>();
        masterMixer.SetFloat("Volume", currentVolume);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnGamePaused(bool paused)
    {
        if (paused)
            SetAudioVolume(-80, 1);
        else
            SetAudioVolume(0, 1);
    }

    public void PlayBoomSFX(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    public void SetAudioVolume(float targetVol, float speed)
    {
        var currVol = currentVolume;
        StartCoroutine(ChangeVolume(currVol, targetVol, speed));
    }

    IEnumerator ChangeVolume(float currVol, float targetVol, float speed)
    {
        var sp = speed;
        while (!Mathf.Approximately(currentVolume, targetVol))
        {
            var newVol = Mathf.SmoothDamp(currentVolume, targetVol, ref sp, 2);
            masterMixer.SetFloat("Volume", currentVolume);
            currentVolume = newVol;
            Debug.LogError(currentVolume);
            yield return null;
        }
    }
}
