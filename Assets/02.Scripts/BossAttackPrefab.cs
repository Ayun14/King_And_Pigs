using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 보스가 뭔갈 날릴 때 쓰는 스크립트 : 나이프, 진주
public class BossAttackPrefab : MonoBehaviour
{
    private float _moveSpeed = 20f;

    private void Update()
    {
        transform.Translate(_moveSpeed * Vector2.right * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.TakeDamage(1);
        }
        else if (collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
