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
                // enemy 들에게 애니메이션, 데미지, 넉벡 주기
            }
        }
    }

    private void OnDrawGizmos() // 판정 박스 그리기
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos.position, colliderSize);
    }
}
