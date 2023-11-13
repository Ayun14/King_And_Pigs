using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeInCheck : MonoBehaviour
{
    public Action<Transform> isPlayerRangeIn;

    private PinkStarEnemyAI _pinkStar;

    private void Start()
    {
        _pinkStar = GetComponentInChildren<PinkStarEnemyAI>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_pinkStar.isAttacking) // 공격 중이 아닐 때만
        {
            isPlayerRangeIn?.Invoke(collision.transform); // 플레이어가 들어왔다고 신호보냄
            Debug.Log("플레이어 Enter Range In");
        }
    }
}