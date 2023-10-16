using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Pathfinding;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyAI : MonoBehaviour
{
    public event Action onPlayerAttack;

    #region A*
    [Header("Pathfinding")]
    [SerializeField] private Transform target;
    [SerializeField] private float activateDistance = 50f;
    [SerializeField] private float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float speed = 200f, jumpForce = 100f;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;
    public float jumpModifier = 0.3f;
    public float jumpCheckOffset = 0.1f;

    [Header("Custom Behavior")]
    public bool followEnabled = true;
    public bool jumpEnabled = true, isJumping, isInAir;
    public bool directionLookEnabled = true;
    public bool targetInDistance = false; // �÷��̾ ���Գ�?
    public bool attackThePlayer = false; // �Դٰ��� �ϰ� �ֳ�

    [SerializeField] Vector3 startOffset;

    private Path path;
    private int currentWaypoint = 0;
    [SerializeField] public RaycastHit2D isGrounded;
    Seeker seeker;
    Rigidbody2D _rigid;
    private bool isOnCoolDown;
    #endregion

    #region MoveToNextPos
    [Header("MoveToNextPos")]
    [SerializeField] private List<Transform> points;
    [SerializeField] private int nextID = 0;
    [SerializeField] private float moveSpeed = 2;

    private int _idChangeValue = 1;
    //private EnemyBehavior _enemyBehavior;
    #endregion

    private Animator _animator;
    private EnemyAttack _enemyAttack;

    private void Reset()
    {
        //Init();
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
        #region A*

        seeker = GetComponent<Seeker>();
        _rigid = GetComponent<Rigidbody2D>();
        isJumping = false;
        isInAir = false;
        isOnCoolDown = false;

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);

        #endregion

        _animator = GetComponent<Animator>();
        _enemyAttack = GetComponent<EnemyAttack>();
    }

    private void FixedUpdate()
    {
        if (target != null) // �÷��̾� null�Ͻ� return
        {
            if (Vector2.Distance(transform.position, target.transform.position) < 2f)
                attackThePlayer = true;
            else attackThePlayer = false;


            if (targetInDistance && attackThePlayer && !_enemyAttack.isAttack)
            {
                _animator.SetBool("Idle", true);
                onPlayerAttack?.Invoke();
            }

            if (attackThePlayer) return;

            if (!targetInDistance)
            {
                _animator.SetBool("Idle", false);
                MoveToNextPoint();
            }
            else if (targetInDistance && followEnabled)
            {
                _animator.SetBool("Idle", false);
                PathFollow();
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

    #region A* �ڵ�

    private void UpdatePath()
    {
        if (followEnabled && targetInDistance && seeker.IsDone())
        {
            seeker.StartPath(_rigid.position, target.position, OnPathComplete);
        }
    }

    private void PathFollow()
    {
        if (path == null)
        {
            return;
        }

        // Reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        StartCoroutine(FindThePlayerRoutine()); // �÷��̾� �߰� UI ���� �۾�

        // See if colliding with anything
        startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset, transform.position.z);
        isGrounded = Physics2D.Raycast(startOffset, -Vector3.up, 0.05f);

        // Direction Calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - _rigid.position).normalized;
        Vector2 force = direction * speed;

        // Jump
        if (jumpEnabled && isGrounded && !isInAir && !isOnCoolDown)
        {
            if (direction.y > jumpNodeHeightRequirement)
            {
                if (isInAir) return;
                isJumping = true;
                _rigid.velocity = new Vector2(_rigid.velocity.x, jumpForce);
                StartCoroutine(JumpCoolDown());

            }
        }
        if (isGrounded)
        {
            isJumping = false;
            isInAir = false;
        }
        else
        {
            isInAir = true;
        }

        // Movement
        _rigid.velocity = new Vector2(force.x, _rigid.velocity.y);

        // Next Waypoint
        float distance = Vector2.Distance(_rigid.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Direction Graphics Handling
        if (directionLookEnabled)
        {
            if (_rigid.velocity.x > 0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (_rigid.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    IEnumerator JumpCoolDown()
    {
        isOnCoolDown = true;
        yield return new WaitForSeconds(1f);
        isOnCoolDown = false;
    }

    #endregion

    IEnumerator FindThePlayerRoutine()
    {
        // UI ���°� �����
        transform.position = Vector2.MoveTowards(transform.position, transform.position, 0);
        yield return new WaitForSeconds(1f);
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
            // state�� ������ ����, �i�ư���, �˹� ������ �����ϱ�
            // �÷��̾ �� �� �߰��ϸ� ���� �� ���� �i�ư��鼭 ������
            // �÷��̾ �߰����� �� ��� ���� UI�� ��� �� �i�ư�
        }
    }
}
