using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CaptainBoss : MonoBehaviour, IInteraction
{
    private enum State
    {
        Idle, WalkAttack, SeriesRushAttack, JumpKnifeAttack
    }
    private State state;

    [SerializeField] private float _maxBossHP = 30;
    private float _bossHP;

    [SerializeField] private Transform pos;
    [SerializeField] private LayerMask groundLayer;

    [Header("WalkAttack")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector2 walkAttackColliderSize;

    [Header("SeriesRushAttack")]
    [SerializeField] private Transform player;
    [SerializeField] private float rushPower;
    [SerializeField] private float attackPower;
    [SerializeField] private Vector2 attack2ColliderSize;
    [SerializeField] private Vector2 attack3ColliderSize;

    [Header("JumpKnifeAttack")]
    [SerializeField] private float jumpPower;
    [SerializeField] private GameObject knifePrefab;
    [SerializeField] private int knifesCnt = 3; // 생성할 Knife 개수

    [Header("Clear")]
    [SerializeField] private GameObject hpBar;
    [SerializeField] private GameObject hpBarFill;
    [SerializeField] private GameObject door;

    private bool _isDead;
    private bool _isGrounded;
    private bool _isWalking;

    private int _beforeRandState = 0;
    private Vector3 _direction;
    private Vector2 _colliderSize = Vector2.zero;

    private Rigidbody2D _rigid;
    private Animator _animator;
    private BoxCollider2D[] _colliders;
    private CinemachineImpulseSource _impuseSource;

    private void Start()
    {
        door.SetActive(false);
        hpBar.SetActive(true);

        state = State.Idle; // 처음엔 Idle 상태
        _rigid = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _colliders = GetComponents<BoxCollider2D>();
        _impuseSource = GetComponent<CinemachineImpulseSource>();

        _bossHP = _maxBossHP;
        HpUpdate(_bossHP / _maxBossHP);

        Invoke("Thinking", 3.5f);
    }

    private void Update()
    {
        transform.position += _direction.normalized * moveSpeed * Time.deltaTime;
    }

    private void ChangeState(State newState)
    {
        state = newState;
    }

    private void Thinking() // 패턴 생각
    {
        if (_isDead) return;

        int randState = Random.Range(1, 4); // Idle 빼고

        if (randState == _beforeRandState)
        {
            Thinking();
            return;
        }

        _beforeRandState = randState;

        ChangeState((State)randState);
        Debug.Log((State)randState);

        StartCoroutine(state.ToString()); // 지금 상태 이름의 코루틴 실행
    }

    IEnumerator WalkAttack()
    {
        yield return new WaitForSeconds(1f);

        _isWalking = false;
        _animator.SetBool("Walk", true);
        int directionChangeCnt = Random.Range(3, 7);

        while (!_isWalking)
        {
            while (directionChangeCnt > 0)
            {
                if (directionChangeCnt % 2 == 0)
                {
                    _direction = Vector3.right;
                    transform.localScale = new Vector2(2.5f, 2.5f);
                }
                else
                {
                    _direction = Vector3.left;
                    transform.localScale = new Vector2(-2.5f, 2.5f);
                }

                yield return new WaitForSeconds(3f);

                _direction = Vector3.zero; // 잠깐 멈추고
                _animator.SetBool("Attack", true);
                _colliderSize = walkAttackColliderSize; // _colliderSize 변경
                AttackJudgment(); // 공격 판정
                CameraShake.Instance.CameraShaking(_impuseSource, 0.2f); // 메인 가서 조절

                yield return new WaitForSeconds(0.5f);
                _animator.SetBool("Attack", false);

                directionChangeCnt--;
            }

            _isWalking = true;
        }

        _direction = Vector3.zero;
        _animator.SetBool("Walk", false);

        yield return new WaitForSeconds(2f);
        Thinking();
    }

    IEnumerator SeriesRushAttack() // 연속 돌진 공격
    {
        // 플레이어 있는 쪽으로 돌진
        int rushCnt = 2;
        while (rushCnt > 0)
        {
            yield return new WaitForSeconds(2f);
            Vector2 directionToPlayer = player.transform.position - transform.position;
            float direction = Mathf.Sign(transform.localScale.x);
            float dotProduct = Vector2.Dot(directionToPlayer.normalized, Vector2.right * direction);

            if (dotProduct > 0) // 오른쪽
            {
                _rigid.AddForce(Vector2.right * rushPower, ForceMode2D.Impulse);
                transform.localScale = new Vector2(2.5f, 2.5f);
                _animator.SetBool("Walk", true);
            }
            else if (dotProduct < 0) // 왼쪽
            {
                _rigid.AddForce(Vector2.left * rushPower, ForceMode2D.Impulse);
                transform.localScale = new Vector2(-2.5f, 2.5f);
                _animator.SetBool("Walk", true);
            }

            CameraShake.Instance.CameraShaking(_impuseSource, 0.1f);
            yield return new WaitForSeconds(0.3f);


            SeriesRushAttackChange("Attack", walkAttackColliderSize);
            yield return new WaitForSeconds(0.3f);


            SeriesRushAttackChange("Attack2", attack2ColliderSize);
            yield return new WaitForSeconds(0.4f);


            SeriesRushAttackChange("Attack3", attack3ColliderSize);

            // 리셋
            _rigid.velocity = Vector2.zero;
            _animator.SetBool("Walk", false);
            _animator.SetBool("Attack", false);
            rushCnt--;
        }

        yield return new WaitForSeconds(2f);
        Thinking();
    }

    private void SeriesRushAttackChange(string animatorName, Vector2 colliderSize)
    {
        CameraShake.Instance.CameraShaking(_impuseSource, 0.2f);

        _animator.SetBool(animatorName, true);
        _colliderSize = colliderSize; // _colliderSize 변경
        AttackJudgment();
    }

    IEnumerator JumpKnifeAttack()
    {
        int jumpAttackCnt = 2;

        while (jumpAttackCnt > 0)
        {
            float startAngle = -150; // 첫 번째 Knife의 각도
            float angleStep = 45; // 각 Knife 간의 각도 간격
            transform.localScale = new Vector2(2.5f, 2.5f);

            if (jumpAttackCnt == 1) // 왼쪽으로 쏠 경우
            {
                Debug.Log("left knife Attack");
                transform.localScale = new Vector2(-2.5f, 2.5f);
                startAngle = -30; // 첫 번째 Knife의 각도
                angleStep = -45; // 각 Knife 간의 각도 간격
            }

            yield return new WaitForSeconds(1f);

            _rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            _animator.SetBool("Jump", true);

            yield return new WaitForSeconds(1f);

            _rigid.gravityScale = 0;
            _rigid.velocity = Vector2.zero;

            _animator.SetTrigger("JumpAttack");

            CameraShake.Instance.CameraShaking(_impuseSource, 0.1f);
            for (int i = 1; i <= knifesCnt; i++)
            {
                float currentAngle = angleStep * i + startAngle + i;

                Instantiate(knifePrefab, transform.position, Quaternion.Euler(0, 0, currentAngle));
            }
            yield return new WaitForSeconds(0.5f);

            CameraShake.Instance.CameraShaking(_impuseSource, 0.1f);
            for (int i = 1; i <= knifesCnt; i++)
            {
                float currentAngle = angleStep * i + startAngle;

                Instantiate(knifePrefab, transform.position, Quaternion.Euler(0, 0, currentAngle));
            }

            yield return null;

            _rigid.gravityScale = 4;
            _animator.SetBool("Jump", false);

            bool isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 3f, groundLayer);
            while (!isGrounded)
            {
                isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 3f, groundLayer);
                yield return null;
            }

            jumpAttackCnt--;
        }

        yield return new WaitForSeconds(2f);
        Thinking();
    }

    public void AttackJudgment()
    {
        // 공격에 따라 _colliderSize 조절해주기
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, _colliderSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.CompareTag("Player"))
            {
                GameManager.Instance.TakeDamage(1);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos.position, _colliderSize);
    }

    public void IsInteraction(Transform trm)
    {
        _bossHP--;
        HpUpdate(_bossHP / _maxBossHP);

        if (_bossHP <= 0)
        {
            StopAllCoroutines();
            StartCoroutine(DeadRoutine());
        }
        else
        {
            _animator.SetTrigger("Hit");
        }
    }

    IEnumerator DeadRoutine()
    {
        while (!_isDead)
        {
            _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 3f, groundLayer);

            if (_isGrounded)
            {
                _animator.SetTrigger("Dead");

                CameraShake.Instance.CameraShaking(_impuseSource, 0.2f);
                Vector2 difference = (transform.position - transform.position).normalized * 50 * _rigid.mass;
                _rigid.AddForce(difference, ForceMode2D.Impulse);

                yield return new WaitForSeconds(0.2f);

                _rigid.gravityScale = 0;

                foreach (var item in _colliders)
                {
                    item.enabled = false;
                }

                door.SetActive(true);
                _isDead = true;
            }
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // 닿으면 데미지 주기
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.TakeDamage(1);
        }
    }

    public void HpUpdate(float normalizedScale)
    {
        Vector3 scale = hpBarFill.transform.localScale;
        scale.x = Mathf.Clamp(normalizedScale, 0, 1f);
        hpBarFill.transform.localScale = scale;
    }
}
