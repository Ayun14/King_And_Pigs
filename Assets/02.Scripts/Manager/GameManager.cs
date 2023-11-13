using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : Singleton<GameManager>
{
    public UnityEvent isTakeDamages; 

    [SerializeField] private Image fadePanel;

    private float _playerHP = 3;
    public float PlayerHP
    { 
        get => _playerHP;
        set => _playerHP = Mathf.Min(value, 3);
    }

    [SerializeField] private Image[] heartIcons;

    private Animator _hearIcontAnimator;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this);
    }

    public void TakeDamage(float damage)
    {
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

                StartCoroutine(WaitRoutine());
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
    }

    IEnumerator WaitRoutine()
    {
        yield return new WaitForSeconds(0.7f);
    }

    private void DeadCheak()
    {
        if (_playerHP < 0)
        {
            Debug.Log("플레이어 사망");
        }
    }

    public void FadeIn(float fadeTime)
    {
        fadePanel.rectTransform.DOAnchorPosX(-90, fadeTime);
    }

    public void FadeOut(float fadeTime)
    {
        fadePanel.rectTransform.DOAnchorPosX(2100, fadeTime);
    }
}
