using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyRangeCheak : MonoBehaviour
{
    public UnityEvent<Vector3> isPlayerRangeIn;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //float distance = Vector2.Distance(transform.position, collision.transform.position);
            Vector3 direction = collision.transform.position - transform.position;
            direction.Normalize();

            isPlayerRangeIn?.Invoke(direction);  //����
            Debug.Log("�÷��̾� ����");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerRangeIn?.Invoke(Vector3.zero);
            Debug.Log("�÷��̾� ����");
        }
    }
}
