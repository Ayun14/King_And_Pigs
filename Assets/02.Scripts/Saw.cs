using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    [SerializeField] private int damage = 2;

    [Header("MoveToNextPos")]
    [SerializeField] private List<Transform> points; // List사용
    [SerializeField] private int nextID = 0;
    [SerializeField] private float moveSpeed = 2;

    private int _idChangeValue = 1;

    private void Update()
    {
        MoveToNextPoint();
    }

    private void MoveToNextPoint() // 정해진 부분을 왔다갔다 이동
    {
        Transform goalPint = points[nextID];

        if (goalPint.transform.position.x > transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);

        transform.position = Vector2.MoveTowards(transform.position, goalPint.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, goalPint.position) < 1f)
        {
            if (nextID == points.Count - 1)
                _idChangeValue = -1;

            if (nextID == 0)
                _idChangeValue = 1;

            nextID += _idChangeValue;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealth enemy = collision.GetComponent<EnemyHealth>();

        if (enemy)
        {
            enemy.TakeDamage(damage, transform);
        }
        else if (collision.CompareTag("Player"))
        {
            GameManager.Instance.TakeDamage(damage);
        }
    }
}
