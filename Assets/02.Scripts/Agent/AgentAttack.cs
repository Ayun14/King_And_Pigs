using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAttack : MonoBehaviour
{
    [SerializeField] private Transform pos;
    [SerializeField] private Vector2 colliderSize;

    public void AttackJudgment()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, colliderSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Obstacle")
            {
                // enemy �鿡�� �ִϸ��̼�, ������, �˺� �ֱ�
            }
        }
    }

    private void OnDrawGizmos() // ���� �ڽ� �׸���
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos.position, colliderSize);
    }
}
