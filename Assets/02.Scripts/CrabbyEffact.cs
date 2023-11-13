using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabbyEffact : MonoBehaviour
{
    private enum Effect
    {
        AttackEffect, LeftJumpAttackEffect, RightJumpAttackEffect
    }
    [SerializeField] private Effect effect;
    [SerializeField] private int damage = 2;

    private float moveSpeed = 12f;

    private void Update()
    {
        if (effect == Effect.LeftJumpAttackEffect)
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
        else if (effect == Effect.RightJumpAttackEffect)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.TakeDamage(damage);
        }
        else if (collision.CompareTag("Obstacle"))
        {
            if (effect == Effect.LeftJumpAttackEffect || effect == Effect.RightJumpAttackEffect)
                Destroy(gameObject);
        }
    }
}
