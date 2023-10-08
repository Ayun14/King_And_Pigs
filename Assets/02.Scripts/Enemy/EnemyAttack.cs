using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyAttack : MonoBehaviour
{
    //public UnityAction AttackState;

    [SerializeField] private float attackCoolTime = 1.0f;

    public bool isAttack;

    private Animator _animator;
    private EnemyBehavior _enemyBehavior;

    private void Start()
    {
        // EnemyBehavior 스크립트의 OnAttack 액션을 구독
        _animator = GetComponentInParent<Animator>();
        _enemyBehavior = GetComponent<EnemyBehavior>();

        _enemyBehavior.OnAttack += AttackPlayer;
    }

    private void OnDestroy()
    {
        _enemyBehavior.OnAttack -= AttackPlayer;
    }

    public void AttackPlayer()
    {
        isAttack = true;
        Debug.Log("공격 공격!");
        _animator.SetTrigger("Attack");
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(attackCoolTime);
        isAttack = false;
    }
}
