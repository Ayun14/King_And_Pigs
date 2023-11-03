using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyAttack : MonoBehaviour
{
    public UnityEvent isEnemyAttack;
    [SerializeField] private float damage = 1;
    [SerializeField] private float attackCoolTime = 1.0f;

    private bool isAttack = false;

    private Animator _animator;
    private EnemyAI _enemyAI;

    private void Start()
    {
        // EnemyAI 스크립트의 OnAttack 액션을 구독
        _enemyAI = GetComponent<EnemyAI>();
        _animator = GetComponentInParent<Animator>();

        _enemyAI.isTargetAttack += AttackPlayer;
    }

    private void OnDestroy()
    {
        _enemyAI.isTargetAttack -= AttackPlayer;
    }

    public void AttackPlayer()
    {
        if (isAttack) return;

        StartCoroutine(AttackRoutine());

        isAttack = true;
        isEnemyAttack?.Invoke();

        GameManager.Instance.TakeDamage(damage); // 플레이어에게 대미지

        _animator.SetBool("Idle", true);
        _animator.SetTrigger("Attack");
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(attackCoolTime);
        isAttack = false;
    }
}
