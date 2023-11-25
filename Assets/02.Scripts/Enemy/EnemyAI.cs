using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Pathfinding;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyAI : MonoBehaviour
{
    public enum EnemyState
    {
        MoveTo,
        FollowingTarget,
        Attack
    }
    private EnemyState _currentState;
    public EnemyState CurrentState { get => _currentState; }

    public event Action isTargetAttack;
    public event Action isFindTarget;

    [SerializeField] private Transform target;

    private bool targetInDistance = false; // �÷��̾ ���Գ�?
    private bool attackThePlayer = false; // �Դٰ��� �ϰ� �ֳ�

    private EnemyHealth _enemyHealth;
    private Animator _animator;

    #region MoveToNextPos
    [Header("MoveToNextPos")]
    [SerializeField] private List<Transform> points; // List���
    [SerializeField] private int nextID = 0;
    [SerializeField] private float moveSpeed = 2;

    private int _idChangeValue = 1;
    #endregion
    
    private void Reset()
    {
        Init();
    }

    private void Init()
    {
        GetComponent<BoxCollider2D>().isTrigger = false;

        // root object ����
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
        _enemyHealth = GetComponent<EnemyHealth>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (target != null && !_enemyHealth.isDead) // �÷��̾� null�Ͻ� return
        {
            if (Vector2.Distance(transform.position, target.transform.position) < 2f)
            {
                attackThePlayer = true;
                _currentState = EnemyState.Attack;
                isTargetAttack?.Invoke();
            }
            else attackThePlayer = false;


            if (attackThePlayer) return; // �������̸� return

            //if (Vector2.Distance(points[nextID].position, points[nextID + 1].position) <= 0.01f)
            //{
            //    _animator.SetBool("Idle", true);
            //    return;
            //}

            if (!targetInDistance)
            {
                _currentState = EnemyState.MoveTo;
                MoveToNextPoint();
            }
            else if (targetInDistance)
            {
                _currentState = EnemyState.FollowingTarget;
                _animator.SetBool("Idle", false);
                isFindTarget?.Invoke();
            }
        }
    }

    private void MoveToNextPoint() // ������ �κ��� �Դٰ��� �̵�
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

    private void OnTriggerEnter2D(Collider2D collision) // �÷��̾� ���� ���� Collider
    {
        // �÷��̾� ����
        if (collision.CompareTag("Player"))
        {
            if (!targetInDistance)
            {
                targetInDistance = true;
            }
        }
    }
}
