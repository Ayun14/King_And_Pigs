using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    private int _playerHP;
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
    }
}
