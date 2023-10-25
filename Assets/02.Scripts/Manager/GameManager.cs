using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    private int _playerHP = 1000;
    public int PlayerHP { get { return _playerHP; } }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        _playerHP -= damage;
        Debug.Log("플레이어 HP : "+ _playerHP);
    }
}
