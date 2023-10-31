using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AgentAttack : MonoBehaviour
{
    public Action<float, Transform> isAttack;

    [SerializeField] private Transform pos;
    [SerializeField] private Vector2 colliderSize;
    [SerializeField] private float damage = 1;

    public void AttackJudgment()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, colliderSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            EnemyHealth enemyHeath = collider.GetComponent<EnemyHealth>();
            CannonBallMovement cannonBall = collider.GetComponent<CannonBallMovement>();
            IInteraction iInteraction = collider.GetComponent<IInteraction>();

            if (enemyHeath)
            {
                Debug.Log("������ ����");
                enemyHeath.TakeDamage(damage, this.transform);
            }
            else if (cannonBall)
            {
                Debug.Log("��ź ������");
                cannonBall.GetKnockBack(this.transform, 30f);
            }
            else if (iInteraction != null)
            {
                iInteraction.IsInteraction(this.transform);
            }
        }
    }

    private void OnDrawGizmos() // ���� �ڽ� �׸���
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos.position, colliderSize);
    }
}
