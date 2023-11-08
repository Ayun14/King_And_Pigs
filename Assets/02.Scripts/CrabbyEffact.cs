using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabbyEffact : MonoBehaviour
{
    [SerializeField] private int damage = 2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.TakeDamage(damage);
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        GameManager.Instance.TakeDamage(2f);
    //    }
    //}
}
