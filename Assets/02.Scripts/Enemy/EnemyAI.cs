using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private List<Transform> points;
    [SerializeField] private int nextID = 0;
    [SerializeField] private float speed = 2;

    private int _idChangeValue = 1;
    private EnemyBehavior _enemyBehavior;

    private void Reset()
    {
        Init();
    }

    private void Init()
    {
        GetComponent<BoxCollider2D>().isTrigger = false;

        // root object 생성
        GameObject root = new GameObject(name + "_Root");
        root.transform.position = transform.position;
        transform.SetParent(root.transform);

        GameObject waypoints = new GameObject("Waypoints");
        waypoints.transform.SetParent(root.transform);

        GameObject p1 = new GameObject("Point1");
        p1.transform.SetParent(waypoints.transform);
        GameObject p2 = new GameObject("Point2");
        p2.transform.SetParent(waypoints.transform);

        points = new List<Transform>();
        points.Add(p1.transform);
        points.Add(p2.transform);
    }

    private void Start()
    {
        _enemyBehavior = GetComponentInChildren<EnemyBehavior>();
    }

    private void Update()
    {
        MoveToNextPoint();
    }

    private void MoveToNextPoint() // 정해진 부분을 왔다갔다 이동
    {
        if (!_enemyBehavior.isFindPlayer)
        {
            Transform goalPint = points[nextID];

            if (goalPint.transform.position.x > transform.position.x)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);

            transform.position = Vector2.MoveTowards(transform.position, goalPint.position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, goalPint.position) < 1f)
            {
                if (nextID == points.Count - 1)
                    _idChangeValue = -1;

                if (nextID == 0)
                    _idChangeValue = 1;

                nextID += _idChangeValue;
            }
        }
    } 
}
