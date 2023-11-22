using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private float currentDelay = 0;
    private float damageDelay = 1f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();

        if (collision.gameObject.CompareTag("Player"))
            GameManager.Instance.TakeDamage(1);
        else if (enemy)
            enemy.TakeDamage(1f, transform);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        StayDamage(collision);
    }

    private void StayDamage(Collision2D collision)
    {
        while (currentDelay < damageDelay)
        {
            currentDelay += Time.deltaTime;

            if (currentDelay >= damageDelay)
            {
                EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();

                if (collision.gameObject.CompareTag("Player"))
                    GameManager.Instance.TakeDamage(1);
                else if (enemy)
                    enemy.TakeDamage(1f, transform);

                currentDelay = 0;
            }
        }
    }
}
