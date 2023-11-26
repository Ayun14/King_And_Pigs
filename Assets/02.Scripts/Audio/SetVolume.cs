using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider bgmSlider;

    private void Start()
    {
        AudioManager.Instance.LoadData();
        bgmSlider.value = AudioManager.Instance.nowVoluem.bgmVolume;
        mixer.SetFloat("SFX", Mathf.Log10(AudioManager.Instance.nowVoluem.sfxVolume) * 20);
    }

    public void SetBGMVolume(float volume)
    {
        AudioManager.Instance.bgmAudioSource.volume = volume;

        AudioManager.Instance.nowVoluem.bgmVolume = volume;
        AudioManager.Instance.SaveData();
    }

    public void SetSFXSliderVolume(float sliderValue)
    {
        mixer.SetFloat("SFX", Mathf.Log10(sliderValue) * 20);

        AudioManager.Instance.nowVoluem.sfxVolume = sliderValue;
        AudioManager.Instance.SaveData();
    }
}
