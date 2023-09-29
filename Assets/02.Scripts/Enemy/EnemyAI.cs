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

    private void Update()
    {
        #region 전 코드 
        //if (player == null)
        //    return;

        //float distance = Vector2.Distance(player.position, transform.position);

        //if (distance < chaseDistanceThreshold)
        //{
        //    OnPointerInput?.Invoke(player.position - transform.position); // 방향

        //    if (distance <= attackDistanceThreshold)
        //    {
        //        // 플레이어 공격
        //        OnMovementInput?.Invoke(Vector2.zero);

        //        if (passedTime >= attackDelay)
        //        {
        //            passedTime = 0;
        //            OnAttack?.Invoke();
        //        }
        //    }
        //    else
        //    {
        //        // 플레이어 추적
        //        Vector2 direction = player.position - transform.position;
        //        OnMovementInput?.Invoke(direction.normalized);
        //    }
        //}
        //if (passedTime < attackDelay)
        //{
        //    passedTime += Time.deltaTime;
        //}
        #endregion

        MoveToNextPoint();
    }

    private void MoveToNextPoint() // 정해진 부분을 왔다갔다 이동
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
