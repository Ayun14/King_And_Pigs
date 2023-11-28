using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : Singleton<GameManager>
{
    public UnityEvent isTakeDamages;
    public UnityEvent onPlayerDead;

    [SerializeField] private Image fadePanel;

    private float _playerHP = 3;
    public float PlayerHP
    { 
        get => _playerHP;
        set => _playerHP = Mathf.Min(value, 3);
    }

    [SerializeField] private Image[] heartIcons;
    [SerializeField] private Image gameOverImage;
    [SerializeField] private Image settingImage;
    [SerializeField] private string nowSceneName;

    [Header("슬로우모션")]
    [SerializeField] private float slowFactor = 0.05f;
    [SerializeField] private float slowLength = 2f;

    private Animator _hearIcontAnimator;

    protected override void Awake()
    {
        base.Awake();

        FadeIn(0.1f);
        FadeOut(1.5f);
    }

    private void Update()
    {
        Time.timeScale += (1f / slowLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void TakeDamage(float damage)
    {
        if (_playerHP < 0) return;

        _playerHP -= damage;
        ReductionHeartIcon();
        Debug.Log("플레이어 HP : " + _playerHP);

        isTakeDamages?.Invoke(); // hit animation 실행하기
        DeadCheak();
    }

    public void ReductionHeartIcon() // heartIcon 감소 시킬 때
    {
        for (int i = 0; i < heartIcons.Length; i++)
        {
            if (i == _playerHP)
            {
                _hearIcontAnimator = heartIcons[i].GetComponent<Animator>();
                _hearIcontAnimator.SetTrigger("HeartBreak");
            }

            heartIcons[i].enabled = false;
        }
        for (int i = 0; i < _playerHP; i++)
        {
            heartIcons[i].enabled = true;
        }
    }

    public void RefillHeartIcon() // heartIcon 다시 채울 때
    {
        if (_playerHP < 0) return;

        _playerHP += 1;
        AudioManager.Instance.PlaySFX(AudioManager.Sfx.LifeUpdate);
        Debug.Log("플레이어 HP : " + _playerHP);

        for (int i = 0; i < heartIcons.Length; i++)
        {
            if (i == _playerHP - 1)
            {
                heartIcons[i].enabled = true;

                _hearIcontAnimator = heartIcons[i].GetComponent<Animator>();
                _hearIcontAnimator.SetTrigger("HeartFill");
            }
        }

        if (_playerHP > 3)
        {
            _playerHP = 3;
        }
    }

    private void DeadCheak()
    {
        if (_playerHP <= 0)
        {
            onPlayerDead?.Invoke();
            StartCoroutine(DeadRoutine());
        }
    }

    IEnumerator DeadRoutine()
    {
        DoSlowMotion();
        yield return new WaitForSeconds(2f);
        fadePanel.rectTransform.DOAnchorPosX(-90, 1).OnComplete(() =>
        {
            gameOverImage.rectTransform.DOAnchorPosY(0, 1f);
            AudioManager.Instance.PlaySFX(AudioManager.Sfx.GameOver);
        });
    }

    public void RetryButtonEvent()
    {
        gameOverImage.rectTransform.DOAnchorPosY(1080, 0.9f).OnComplete(() =>
        {
            switch (nowSceneName)
            {
                case "Tutorial Stage":
                    AudioManager.Instance.PlayBGM(AudioManager.BGM.Stage1);
                    break;
                case "Stage 1":
                    AudioManager.Instance.PlayBGM(AudioManager.BGM.Stage1);
                    break;
                case "Stage 2":
                    AudioManager.Instance.PlayBGM(AudioManager.BGM.Stage2);
                    break;
                case "Stage 3":
                    AudioManager.Instance.PlayBGM(AudioManager.BGM.Stage3);
                    break;
            }
            SceneManager.LoadScene(nowSceneName);
        });
    }

    public void StageSelectButtonEvent()
    {
        gameOverImage.rectTransform.DOAnchorPosY(1080, 0.9f).OnComplete(() =>
        {
            SceneManager.LoadScene(1);
            AudioManager.Instance.PlayBGM(AudioManager.BGM.Start);
        });
    }

    public void LobbyButtonEvent()
    {
        gameOverImage.rectTransform.DOAnchorPosY(1080, 0.9f).OnComplete(() =>
        {
            SceneManager.LoadScene(0);
            AudioManager.Instance.PlayBGM(AudioManager.BGM.Start);
        });
    }

    public void SettingInButtonEvent()
    {
        fadePanel.rectTransform.DOAnchorPosX(-90, 1).OnComplete(() =>
        {
            settingImage.rectTransform.DOAnchorPosY(0, 1).OnComplete(() =>
            {
                Time.timeScale = 0;
            });
        });
    }

    public void SettingOutButtonEvent()
    {
        settingImage.rectTransform.DOAnchorPosY(1080, 1).OnComplete(() =>
        {
            fadePanel.rectTransform.DOAnchorPosX(2100, 1).OnComplete(() =>
            {
                Time.timeScale = 1;
            });
        });
    }

    public void FadeIn(float fadeTime)
    {
        fadePanel.rectTransform.DOAnchorPosX(-90, fadeTime);
    }

    public void FadeOut(float fadeTime)
    {
        fadePanel.rectTransform.DOAnchorPosX(2100, fadeTime);
    }

    public void DoSlowMotion()
    {
        Time.timeScale = slowFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void ButtonClickSound()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Sfx.UIClick);
    }
}
