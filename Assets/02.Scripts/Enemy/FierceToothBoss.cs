using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class FierceToothBoss : MonoBehaviour, IInteraction
{
    private enum State
    {
        Idle, WalkAttack, JumpDashAttack, RayAttack, CenterAttack
    }
    private State state;

    [SerializeField] private float _maxBossHP = 65;
    private float _bossHP;

    [SerializeField] private LayerMask groundLayer;

    [Header("WalkAttack")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform player;
    private int _walkAttackCnt = 12;

    [Header("JumpDashAttack")]
    [SerializeField] private float jumpPower;
    [SerializeField] private float dashPower;
    private Vector2 _attackDirection = Vector2.zero;

    [Header("RayAttack")]
    [SerializeField] private GameObject rayPrefab;
    [SerializeField] private GameObject rayWarning;
    [SerializeField] private Transform raySpawnPos;
    private Vector3 _forecDirection;

    [Header("CenterAttack")]
    [SerializeField] private GameObject spearTrapObj;
    [SerializeField] private GameObject warningZonePrefab;
    [SerializeField] private Transform warningSpawnPos;
    [SerializeField] private int centerAttackCnt;
    [SerializeField] private float jumpPower2;
    [SerializeField] private float spawnMinX;
    [SerializeField] private float spawnMaxX;
    [SerializeField] private Transform _centerPos;
    private SpearTrap spearTrap;

    [Header("Clear")]
    [SerializeField] private GameObject hpBar;
    [SerializeField] private GameObject hpBarFill;
    [SerializeField] private GameObject door;

    private bool _isDead;
    private bool _isGrounded;
    private bool _isWalkAttack;
    private bool _isJumpDashAttack;
    private bool _isGroundedRight;
    private bool _isCenterAttack;

    private int _beforeRandState = 0;
    private Vector3 _direction;

    private Rigidbody2D _rigid;
    private Animator _animator;
    private BoxCollider2D _collider;
    private CinemachineImpulseSource _impuseSource;

    private void Start()
    {
        door.SetActive(false);
        hpBar.SetActive(true);

        state = State.Idle; // 처음엔 Idle 상태

        _rigid = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
        _impuseSource = GetComponent<CinemachineImpulseSource>();
        spearTrap = spearTrapObj.GetComponent<SpearTrap>();

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

        int randState = Random.Range(1, 5); // Idle 빼고

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

    private bool DotProduct() // 플레이어가 왼쪽에 있는지 오른쪽에 있는지 알려줌
    {
        Vector2 directionToPlayer = player.transform.position - transform.position;
        float direction = Mathf.Sign(transform.localScale.x);
        float dotProduct = Vector2.Dot(directionToPlayer.normalized, Vector2.right * direction);

        if (dotProduct >= 0) // 오른쪽
            return true;
        else if (dotProduct < 0) // 왼쪽
            return false;

        return false;
    }

    IEnumerator WalkAttack()
    {
        _animator.SetBool("Walk", true);

        for (int i = 0; i < _walkAttackCnt; i++)
        {
            _animator.SetTrigger("Attack");
            AudioManager.Instance.PlaySFX(AudioManager.Sfx.BossAttack);
            _direction = Vector2.zero;

            yield return new WaitForSeconds(0.5f);

            if (DotProduct()) // 오른쪽
            {
                transform.localScale = new Vector2(-2, 2);
                _direction = Vector2.right;
            }
            else
            {
                transform.localScale = new Vector2(2, 2);
                _direction = Vector2.left;
            }
            yield return new WaitForSeconds(1f);
        }

        _animator.SetBool("Walk", false);
        _direction = Vector2.zero;

        yield return new WaitForSeconds(2f);
        Thinking();
    }

    IEnumerator JumpDashAttack()
    {
        for (int i = 0; i < 2; i++)
        {
            _animator.SetBool("Jump", true);

            yield return new WaitForSeconds(0.5f);

            _rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

            if (DotProduct())
            {
                _attackDirection = Vector2.right;
                transform.localScale = new Vector2(-2, 2);
            }
            else
            {
                _attackDirection = Vector2.left;
                transform.localScale = new Vector2(2, 2);
            }

            _animator.SetTrigger("Attack");
            AudioManager.Instance.PlaySFX(AudioManager.Sfx.BossAttack);
            yield return new WaitForSeconds(0.5f);
            _rigid.velocity = Vector2.zero;

            _rigid.AddForce(_attackDirection * dashPower, ForceMode2D.Impulse);

            while (!_isJumpDashAttack)
            {
                _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 2, groundLayer);

                if (_isGrounded)
                {
                    _animator.SetBool("Jump", false);
                    _rigid.velocity = Vector2.zero;
                    _isJumpDashAttack = true;
                }
                yield return null;
            }

            yield return new WaitForSeconds(1f);
        }
        _isJumpDashAttack = false;
        Thinking();
    }

    IEnumerator RayAttack()
    {
        transform.localScale = new Vector3(-2, 2, 2);
        _direction = Vector3.right;
        Debug.Log(_forecDirection);

        while (true)
        {
            _animator.SetBool("Walk", true);

            _isGroundedRight = Physics2D.Raycast(transform.position, Vector2.right, 3f, groundLayer);
            Debug.DrawRay(transform.position, Vector2.right * 3f, Color.blue);
            
            if (_isGroundedRight)
            {
                transform.localScale = new Vector3(-transform.localScale.x, 2, 2);
                _direction = Vector3.zero;
                _animator.SetBool("Walk", false);
                rayWarning.SetActive(true);
                yield return new WaitForSeconds(0.8f);
                rayWarning.SetActive(false);
                yield return new WaitForSeconds(0.7f);
                break;
            }
            yield return null;
        }
        _animator.SetBool("RayAttack", true);

        GameObject ray = Instantiate(rayPrefab, raySpawnPos.position, Quaternion.identity);
        RayPrefab rayComponent = ray.GetComponent<RayPrefab>();

        yield return new WaitForSeconds(2f);

        rayComponent.RayOffAnimator();
        _animator.SetBool("RayAttack", false);
        yield return new WaitForSeconds(0.5f);
        Destroy(ray);

        transform.localScale = new Vector3(2, 2, 2);
        _direction = Vector3.left;
        _animator.SetBool("Walk", true);

        yield return new WaitForSeconds(2f);
        _animator.SetBool("Walk", false);
        _direction = Vector3.zero;

        yield return new WaitForSeconds(1f);
        Thinking();
    }

    IEnumerator CenterAttack()
    {
        _animator.SetBool("Walk", true);
        _rigid.gravityScale = 0;
        transform.DOMoveX(_centerPos.position.x, 2f).OnComplete(() =>
        {
            _animator.SetBool("Walk", false);
        });

        yield return new WaitForSeconds(2f);

        _animator.SetBool("CenterJump", true);
        transform.DOMoveY(_centerPos.position.y, 1.5f).OnComplete(() =>
        {
            _animator.SetTrigger("Center");
        });

        for (int i = 0; i < centerAttackCnt; i++)
        {
            if (i == 0)
                yield return new WaitForSeconds(1.5f);

            yield return new WaitForSeconds(1.5f);
            float playerX = player.transform.position.x;
            Vector3 warningPos = new Vector3(player.position.x, warningSpawnPos.position.y);
            GameObject warning = Instantiate(warningZonePrefab, warningPos, Quaternion.identity);

            yield return new WaitForSeconds(0.6f);
            Destroy(warning);

            spearTrap.MoveAttack(playerX);
        }
        _animator.SetTrigger("Fall");
        _rigid.gravityScale = 1;

        while (!_isCenterAttack)
        {
            _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 2f, groundLayer);

            if (_isGrounded)
            {
                _animator.SetBool("CenterJump", false);
                _isCenterAttack = true;
            }
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        Thinking();
    }

    public void CameraShakeAniamtionEvent()
    {
        CameraShake.Instance.CameraShaking(_impuseSource, 0.2f);
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
            _animator.Play("FierceTooth Hit");
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

                GameManager.Instance.DoSlowMotion();

                yield return new WaitForSeconds(0.2f);

                _rigid.velocity = Vector2.zero;
                _direction = Vector2.zero;

                _rigid.gravityScale = 0;
                _collider.enabled = false;

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
