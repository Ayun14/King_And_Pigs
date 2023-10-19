using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AgentAttack : MonoBehaviour
{
    public UnityEvent isAttack;

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
                isAttack?.Invoke();
                Debug.Log("���� ����");
            }
        }
    }

    private void OnDrawGizmos() // ���� �ڽ� �׸���
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos.position, colliderSize);
    }
}
