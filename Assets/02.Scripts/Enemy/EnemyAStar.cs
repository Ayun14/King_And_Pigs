using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAStar : MonoBehaviour
{
    [SerializeField] private GameObject enemyUI;

    [Header("Pathfinding")]
    [SerializeField] private Transform target;
    [SerializeField] private float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float speed = 5f, jumpForce = 10f;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;
    public float jumpModifier = 0.3f;
    public float jumpCheckOffset = 0.1f;

    [Header("Custom Behavior")]
    public bool followEnabled = true;
    public bool jumpEnabled = true, isJumping, isInAir;
    public bool directionLookEnabled = true; // Flip
    public bool enemyUIPlaying = false; // enemy UI가 실행되고 있나

    [SerializeField] Vector3 startOffset;

    [SerializeField] public RaycastHit2D isGrounded;
    private Path path;
    private int currentWaypoint = 0;
    private Seeker seeker;
    private Rigidbody2D _rigid;
    private bool isOnCoolDown;

    private EnemyAI _enemyAI;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        _rigid = GetComponent<Rigidbody2D>();
        _enemyAI = GetComponent<EnemyAI>();
        isJumping = false;
        isInAir = false;
        isOnCoolDown = false;

        _enemyAI.isFindTarget += PathFollow;
        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void OnDestroy()
    {
        _enemyAI.isFindTarget -= PathFollow;
    }

    private void UpdatePath()
    {
        if (followEnabled && seeker.IsDone())
        {
            seeker.StartPath(_rigid.position, target.position, OnPathComplete);
        }
    }

    private void PathFollow()
    {
        if (path == null)
        {
            // 여기서 null
            Debug.Log("path = null");
            return;
        }

        // Reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        StartCoroutine(FindThePlayerRoutine()); // 플레이어 발견 UI 띄우는 작업

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

    IEnumerator FindThePlayerRoutine()
    {
        if (enemyUIPlaying) yield break;

        enemyUIPlaying = true;
        enemyUI.SetActive(true);
        _rigid.velocity = new Vector2(_rigid.velocity.x, _rigid.velocity.y);
        yield return new WaitForSeconds(0.6f);
        enemyUI.SetActive(false);
    }
}
