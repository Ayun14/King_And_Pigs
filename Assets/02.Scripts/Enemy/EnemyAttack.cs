using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackCoolTime = 1.0f;
    [SerializeField] private Transform pos;
    [SerializeField] private Vector2 colliderSize;

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

        _animator.SetBool("Idle", true);
        _animator.SetTrigger("Attack");
        AudioManager.Instance.PlaySFX(AudioManager.Sfx.PigAttack);
        AttackJudgment();
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(attackCoolTime);
        isAttack = false;
    }
    public void AttackJudgment()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, colliderSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.CompareTag("Player"))
            {
                GameManager.Instance.TakeDamage(1);
            }
        }
    }

    private void OnDrawGizmos() // 판정 박스 그리기
    {
        if (pos != null && colliderSize != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(pos.position, colliderSize);
        }
    }
}
