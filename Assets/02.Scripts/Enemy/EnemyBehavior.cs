using System;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Action OnAttack; // �׼� ��� 

    [SerializeField] private float speed = 2.0f;
    private float _attackDistance = 0.01f;

    public bool isFindPlayer = false; // �÷��̾ ã�ҳ�?

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

            // ���� �Ÿ��� �����ϸ� ���� ����
            if (Vector2.Distance(transform.position, _player.position) <= _attackDistance)
            {
                // ���� �׼� ȣ��
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
                // ã�Ҵٴ� ǥ�� (UI���ų�)
                isFindPlayer = true;
            }
        }
    }
}
