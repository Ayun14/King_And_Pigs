using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PinkStarEnemyAI : MonoBehaviour, IInteraction
{
    [SerializeField] private float rushPower;
    [SerializeField] private LayerMask groundLayer;

    public bool isAttacking;
    private bool isGrounded;
    private bool isPlayer;

    private RangeInCheck _rangeInCheck;
    private Animator _animator;
    private Rigidbody2D _rigid;
    private EnemyHealth _enemyHealth;

    private void Awake()
    {
        _rangeInCheck = GetComponentInParent<RangeInCheck>();
        _animator = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody2D>();
        _enemyHealth = GetComponent<EnemyHealth>();
    }

    private void Start()
    {
        _rangeInCheck.isPlayerRangeIn += PinkStarAttack;
    }

    private void OnDestroy()
    {
        _rangeInCheck.isPlayerRangeIn -= PinkStarAttack;
    }

    private void PinkStarAttack(Transform target)
    {
        if (_enemyHealth.isDead) return;

        // 초기 설정
        isPlayer = false;
        isGrounded = false;

        // 왼쪽에 있는지 오른쪽에 있는지 보고 돌진하기
        isAttacking = true;
        StartCoroutine(PinkStarAttackRoutine(target));
    }

    IEnumerator PinkStarAttackRoutine(Transform target)
    {
        Vector2 directionToPlayer = target.transform.position - transform.position;
        float direction = Mathf.Sign(transform.localScale.x);
        float dotProduct = Vector2.Dot(directionToPlayer.normalized, Vector2.right * direction);

        Debug.Log(dotProduct);

        while (!isGrounded && !isPlayer)
        {
            if (Physics2D.Raycast(transform.position, Vector2.right, 1.5f, groundLayer))
                isGrounded = true;
            else if (Physics2D.Raycast(transform.position, Vector2.left, 1.5f, groundLayer))
                isGrounded = true;

            // 벽에 닿지 않을 때까지 계속 돌진
            if (dotProduct > 0) // 오른쪽
            {
                _rigid.AddForce(Vector2.right * rushPower, ForceMode2D.Impulse);
                _animator.SetBool("Idle", true); // 공격 애니메이션 트루 한거임
            }
            else if (dotProduct < 0) // 왼쪽
            {
                _rigid.AddForce(Vector2.left * rushPower, ForceMode2D.Impulse);
                _animator.SetBool("Idle", true); // 공격 애니메이션 트루 한거임
            }

            yield return null;
        }

        _rigid.velocity = Vector2.zero;
        _animator.SetBool("Idle", false); // 공격 애니메이션 false 한거임

        // 조금 랜덤 방향으로 튕기기
        Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(0.1f, 1f)).normalized;
        _rigid.AddForce(randomDirection * 10, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1.5f);
        
        isAttacking = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayer = true;
            GameManager.Instance.TakeDamage(1);
        }
    }

    public void IsInteraction(Transform trm) // 플레이어한태 맞았을 때
    {
        _enemyHealth.TakeDamage(1f, trm); // 체력--
    }
}
