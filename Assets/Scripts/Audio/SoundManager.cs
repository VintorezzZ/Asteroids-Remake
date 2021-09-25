using System.Collections;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Audio;
using EventHandler = DefaultNamespace.EventHandler;

public class SoundManager : SingletonBehaviour<SoundManager>
{
    [SerializeField] private AudioSource musicSource, uiSource, inGameSource;
    [SerializeField] private AudioMixer masterMixer;
    public static float CurrentVolume => AudioListener.volume;
    public float globalVolume = .5f;
    private Coroutine _fadeCoroutine;
    private void Awake()
    {
        InitializeSingleton();

        AudioListener.volume = globalVolume;
        
        EventHandler.gamePaused += OnGamePaused;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        EventHandler.gamePaused -= OnGamePaused;
    }

    private void OnGamePaused(bool paused)
    {
        if(_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
            
        if (paused)
        {
            FadeMixerGroup(masterMixer, 0, duration: .5f);
        }       
        else
        {
            FadeMixerGroup(masterMixer, 1, duration: .5f);
        }
    }

    private void FadeMixerGroup(AudioMixer audioMixer, float targetVolume, string exposedParam = "volume", float duration = 1f)
    {
        _fadeCoroutine = StartCoroutine(MixerGroupFader.StartFade(audioMixer, exposedParam, duration, targetVolume));
    }

    public void PlayBoomSFX(AudioClip audioClip)
    {
        inGameSource.PlayOneShot(audioClip);
    }
}
