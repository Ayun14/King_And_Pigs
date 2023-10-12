using System;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Action OnAttack; // �׼� ��� 

    [SerializeField] private Transform playerTransform;
    [SerializeField] private float moveSpeed = 2.0f;
    private float _attackDistance = 0.01f;

    public bool isFindPlayer = false; // �÷��̾ ã�ҳ�?

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
                // �÷��̾��� ���� ��ġ�� �ε巴�� �̵�
                Vector3 targetPosition = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }

            // ���� �Ÿ��� �����ϸ� ���� ����
            if (Vector2.Distance(transform.position, playerTransform.position) <= _attackDistance)
            {
                // ���� �׼� ȣ��
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
                // ã�Ҵٴ� ǥ�� (UI���ų�)
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
                // ã�Ҵٴ� ǥ�� (UI���ų�)
                isFindPlayer = true;
            }
        }
    }
}
