using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyAttack : MonoBehaviour
{
    public UnityAction isAttackState; // 공격일 때

    private int _enemyDamage = 100;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isAttackState?.Invoke();
            GameManager._instance.TakeDamage(_enemyDamage);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 원래대로 왔다갔다 거리는 State로 바꾸기
    }
}
