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
    private EnemyAI _enemyAI;

    private void Start()
    {
        // EnemyAI ��ũ��Ʈ�� OnAttack �׼��� ����
        _animator = GetComponentInParent<Animator>();
        _enemyAI = GetComponent<EnemyAI>();

        _enemyAI.onPlayerAttack += AttackPlayer;
    }

    private void OnDestroy()
    {
        _enemyAI.onPlayerAttack -= AttackPlayer;
    }

    public void AttackPlayer()
    {
        isAttack = true;
        Debug.Log("������ �ֱ�");
        _animator.SetTrigger("Attack");
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(attackCoolTime);
        isAttack = false;
    }
}
