using System;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Action OnAttack; // 액션 사용 

    [SerializeField] private float speed = 2.0f;
    private float _attackDistance = 0.01f;

    public bool isFindPlayer = false; // 플레이어를 찾았나?

    private Transform _player;
    private Animator _animator;
    private EnemyAttack _enemyAttack;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _animator = GetComponentInParent<Animator>();
        _enemyAttack = GetComponent<EnemyAttack>();
    }

    private void Update()
    {
        MoveToPlayer();
    }

    private void MoveToPlayer()
    {
        if (isFindPlayer)
        {
            transform.position = Vector2.MoveTowards(transform.position, _player.position, speed * Time.deltaTime);

            // 공격 거리에 도달하면 공격 시작
            if (Vector2.Distance(transform.position, _player.position) <= _attackDistance)
            {
                // 공격 액션 호출
                if (!_enemyAttack.isAttack)
                {
                    OnAttack?.Invoke();
                }

                _animator.SetTrigger("Idle");
            }
            else
                transform.position = Vector2.MoveTowards(transform.position, _player.position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!_enemyAttack.isAttack)
            {
                // 찾았다는 표시 (UI띄우거나)
                isFindPlayer = true;
            }
        }
    }
}
