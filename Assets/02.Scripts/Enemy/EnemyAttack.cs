using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyAttack : MonoBehaviour
{
    //public UnityAction AttackState;

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

        isAttack = true;
        Debug.Log("데미지 주기");
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
