using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    private float _playerHP = 3;
    public float PlayerHP { get { return _playerHP; } }

    [SerializeField] private Image[] heartIcons;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this);
    }

    public void TakeDamage(float damage)
    {
        _playerHP -= damage;
        UpdateHeartIcon();
        Debug.Log("플레이어 HP : "+ _playerHP);

        DeadCheak();
    }

    public void UpdateHeartIcon()
    {
        if (_playerHP < 0) return;

        for (int i = 0; i < heartIcons.Length; i++)
        {
            heartIcons[i].enabled = false;
        }
        for (int i = 0; i < _playerHP; i++)
        {
            heartIcons[i].enabled = true;
        }
    }
    private void DeadCheak()
    {
        if (_playerHP <= 0)
        {
            Debug.Log("플레이어 사망");
        }
    }
}
