using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();

        if (collision.gameObject.CompareTag("Player"))
            GameManager.Instance.TakeDamage(1);
        else if (enemy)
            enemy.TakeDamage(1f, transform);
    }
}
