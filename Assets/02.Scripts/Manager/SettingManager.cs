using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SettingManager : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private Image settingImage;

    public void StartButtonEvent()
    {
        fadeImage.rectTransform.DOAnchorPosX(-90, 1f).OnComplete(() =>
        {
            SceneManager.LoadScene(1);
        });
    }

    public void SettingButtonEvent()
    {
        fadeImage.rectTransform.DOAnchorPosX(-90, 1f).OnComplete(() =>
        {
            settingImage.rectTransform.DOAnchorPosY(0, 1f);
        });
    }

    public void SettingOutButtonEvent()
    {
        settingImage.rectTransform.DOAnchorPosY(1080, 1f).OnComplete(() =>
        {
            fadeImage.rectTransform.DOAnchorPosX(2100, 1f);
        });
    }

    public void ExitButtonEvent()
    {
        fadeImage.rectTransform.DOAnchorPosX(-90, 1f).OnComplete(() =>
        {
            Application.Quit();
        });
    }

    public void ButtonClickSound()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Sfx.UIClick);
    }
}
