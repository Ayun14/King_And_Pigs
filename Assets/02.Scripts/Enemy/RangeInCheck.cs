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
        if (collision.CompareTag("Player") && !_pinkStar.isAttacking) // ���� ���� �ƴ� ����
        {
            isPlayerRangeIn?.Invoke(collision.transform); // �÷��̾ ���Դٰ� ��ȣ����
            Debug.Log("�÷��̾� Enter Range In");
        }
    }
}