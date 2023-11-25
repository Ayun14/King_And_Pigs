using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : Singleton<AudioManager>
{
    public enum BGM
    {
        Start, Stage1, Stage2, Stage3, Boss
    }

    public enum Sfx
    {
        GameClear, GameOver,
        PlayerAttack, PlayerHit, PlayerJump, PlayerDash,
        PigAttack, Cannon,
        BossHit, BossAttack, CrabbyAttack, CaptainAttack, SeaShellAttack, ToothAttack,
        UIClick, LifeUpdate,
        DoorOpen, DoorClose
    }

    [Header("º¼·ýÁ¶Àý")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider sfxSlider;

    [Header("BGM")]
    [SerializeField] private AudioClip[] bgmClips;
    private AudioSource bgmAudioSource;

    [Header("SFX")]
    [SerializeField] private AudioClip[] sfxClips;
    private AudioSource[] sfxAudioSources;
    private int channelIndex;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        bgmAudioSource = transform.Find("BGM").GetComponent<AudioSource>();
        sfxAudioSources = transform.Find("SFX").GetComponents<AudioSource>();
        channelIndex = sfxAudioSources.Length;
    }

    public void SetBGMVolume(float volume)
    {
        bgmAudioSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }

    public void PlayBGM(BGM bgm)
    {
        if (bgmAudioSource.clip == bgmClips[(int)bgm])
            return;

        bgmAudioSource.clip = bgmClips[(int)bgm];
        bgmAudioSource.Play();
    }

    public void PlaySFX(Sfx sfx, float pitch = 1)
    {
        for (int index = 0; index < sfxAudioSources.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxAudioSources.Length;

            if (sfxAudioSources[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;
            sfxAudioSources[loopIndex].clip = sfxClips[(int)sfx];
            sfxAudioSources[loopIndex].pitch = pitch;
            sfxAudioSources[loopIndex].Play();
            break;
        }
    }
}
