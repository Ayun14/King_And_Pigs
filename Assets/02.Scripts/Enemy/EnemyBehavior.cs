using System;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Action OnAttack; // 액션 사용 

    [SerializeField] private Transform playerTransform;
    [SerializeField] private float moveSpeed = 2.0f;
    private float _attackDistance = 0.01f;

    public bool isFindPlayer = false; // 플레이어를 찾았나?

    private Animator _animator;
    private EnemyAttack _enemyAttack;

    private void Start()
    {
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
            if (playerTransform != null)
            {
                // 플레이어의 현재 위치로 부드럽게 이동
                Vector3 targetPosition = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }

            // 공격 거리에 도달하면 공격 시작
            if (Vector2.Distance(transform.position, playerTransform.position) <= _attackDistance)
            {
                // 공격 액션 호출
                if (!_enemyAttack.isAttack)
                {
                    OnAttack?.Invoke();
                    _animator.SetBool("IsIdle", true);
                }
            }
            else
                transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
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

    private void OnTriggerExit2D(Collider2D collision)
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
