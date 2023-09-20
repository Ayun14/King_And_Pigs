using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Rigidbody2D _rigid;
    private Animator _animator;
    private Vector3 _velocity;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        EnemyMove(_velocity);
    }

    public void EnemyMove(Vector3 direction)
    {
        _velocity = direction * Time.deltaTime;
        transform.position += _velocity.normalized;

        _rigid.velocity = transform.position * moveSpeed;
    }

    private void EnemyMoveTo()
    {
        _rigid.velocity *= moveSpeed;
    }
}
