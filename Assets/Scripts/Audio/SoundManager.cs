using System.Collections;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : SingletonBehaviour<SoundManager>
{
    [SerializeField] private AudioSource musicSource, uiSource, inGameSource;
    public AudioMixer uiMixer, musicMixer, inGameMixer;
    public static float CurrentVolume => AudioListener.volume;
    public float globalVolume = .5f;
    private Coroutine _fadeCoroutine;
    private void Awake()
    {
        InitializeSingleton();

        AudioListener.volume = globalVolume;
        inGameSource.outputAudioMixerGroup = inGameMixer.FindMatchingGroups("Master")[0];
        
        EventHub.gamePaused += OnGamePaused;
        EventHub.gameOvered += OnGameOvered;
    }

    private void OnGameOvered()
    {
        FadeMixerGroup(inGameMixer, 0f);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        EventHub.gamePaused -= OnGamePaused;
        EventHub.gameOvered -= OnGameOvered;
    }

    private void OnGamePaused(bool paused)
    {
        if(_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);

        FadeMixerGroup(inGameMixer, paused ? 0 : globalVolume, duration: .5f);
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
