using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class CrabbyBoss : MonoBehaviour, IInteraction
{
    private enum State
    {
        Idle, Walk, Attack, JumpAttack, RushAttack
    }
    private State state;

    [SerializeField] private float _maxBossHP = 30;
    private float _bossHP;

    [Header("Walk")]
    [SerializeField] private float moveSpeed;

    [Header("Attack")]
    [SerializeField] private GameObject attackEffectPrefab;

    [Header("JumpAttack")]
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject leftJumpAttackEffectPrefab;
    [SerializeField] private GameObject rightJumpAttackEffectPrefab;
    [SerializeField] private Transform leftEffectSpawn;
    [SerializeField] private Transform rightEffectSpawn;

    [Header("RushAttack")]
    [SerializeField] private GameObject player;
    [SerializeField] private float rushPower = 15;

    [Header("Clear")]
    [SerializeField] private GameObject hpBar;
    [SerializeField] private GameObject hpBarFill;
    [SerializeField] private GameObject door;

    private bool _isDead;
    private bool isWalking = false;
    private bool _isGrounded;

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

        state = State.Idle; // ó���� Idle ����
        _rigid = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
        _impuseSource = GetComponent<CinemachineImpulseSource>();

        _bossHP = _maxBossHP;
        HpUpdate(_bossHP / _maxBossHP);

        Invoke("Thinking", 3.5f);
    }

    private void ChangeState(State newState)
    {
        state = newState;
    }

    private void Update()
    {
        transform.position += _direction.normalized * moveSpeed * Time.deltaTime;
    }

    private void Thinking() // ���� ����
    {
        if (_isDead) return;

        int randState = Random.Range(1, 5); // Idle ����

        if (randState == _beforeRandState)
        {
            Thinking();
            return;
        }

        _beforeRandState = randState;

        ChangeState((State)randState);
        Debug.Log((State)randState);

        StartCoroutine(state.ToString()); // ���� ���� �̸��� �ڷ�ƾ ����
    }

    private IEnumerator Walk() // ���� �ð��� ������ ������ �ٲٸ鼭 �ɾ�ٴ�
    {
        if (_isDead) yield break;

        yield return new WaitForSeconds(1f);

        isWalking = false;
        _animator.SetBool("Walk", true);
        int directionChangeCnt = Random.Range(3, 6);

        while (!isWalking)
        {
            while (directionChangeCnt > 0)
            {
                if (directionChangeCnt % 2 == 0)
                {
                    _direction = Vector3.right;
                    transform.localScale = new Vector2(-2, 2);
                }
                else
                {
                    _direction = Vector3.left;
                    transform.localScale = new Vector2(2, 2);
                }

                yield return new WaitForSeconds(3f);
                directionChangeCnt--;
            }

            isWalking = true;
        }

        _direction = Vector3.zero;
        _animator.SetBool("Walk", false);

        yield return new WaitForSeconds(2f);
        Thinking(); // �ٸ� ���� ����
    }

    private IEnumerator Attack() // �Ϲ� ����, ��ƼŬ ������
    {
        if (_isDead) yield break;

        yield return new WaitForSeconds(1f);
        _animator.SetTrigger("Attack");

        CameraShake.Instance.CameraShaking(_impuseSource, 0.2f);

        GameObject effect = Instantiate(attackEffectPrefab, transform.position, Quaternion.identity);
        Destroy(effect, 0.5f);

        yield return new WaitForSeconds(5f);
        Thinking();
    }

    private IEnumerator JumpAttack()
    {
        if (_isDead) yield break;

        // �����ѵ� �����ͼ� ��ǳ�� ����µ� �װſ� ������ �������� �Դ�
        yield return new WaitForSeconds(1f);

        _rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        _animator.SetBool("Jump", true);

        yield return new WaitForSeconds(1f);

        transform.DOMove(player.transform.position, 1f); // �÷��̾� ��ġ�� �̵�

        bool isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 3f, groundLayer);
        while (!isGrounded)
        {
            Debug.DrawRay(transform.position, Vector2.down * 3f, Color.blue);
            isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 3f, groundLayer);
            yield return null;
            Debug.Log(isGrounded);
        }
        CameraShake.Instance.CameraShaking(_impuseSource, 0.5f); // ���� ���� ������
        _animator.SetBool("Jump", false);

        Instantiate(leftJumpAttackEffectPrefab, leftEffectSpawn.position, Quaternion.identity);
        Instantiate(rightJumpAttackEffectPrefab, rightEffectSpawn.position, Quaternion.identity);

        yield return new WaitForSeconds(5f);

        Thinking();
    }

    private IEnumerator RushAttack()
    {
        if (_isDead) yield break;

        // �÷��̾� �ִ� ������ ����
        int rushCnt = 2;
        while (rushCnt > 0)
        {
            yield return new WaitForSeconds(1.5f);
            Vector2 directionToPlayer = player.transform.position - transform.position;
            float direction = Mathf.Sign(transform.localScale.x);
            float dotProduct = Vector2.Dot(directionToPlayer.normalized, Vector2.right * direction);

            if (dotProduct > 0) // ������
            {
                _rigid.AddForce(Vector2.right * rushPower, ForceMode2D.Impulse);
                _animator.SetBool("Rush", true);
            }
            else if (dotProduct < 0) // ����
            {
                _rigid.AddForce(Vector2.left * rushPower, ForceMode2D.Impulse);
                _animator.SetBool("Rush", true);
            }

            CameraShake.Instance.CameraShaking(_impuseSource, 0.1f);
            yield return new WaitForSeconds(1.5f);
            _animator.SetBool("Rush", false);
            _rigid.velocity = Vector2.zero;

            rushCnt--;
        }

        yield return new WaitForSeconds(2f);
        Thinking();
    }

    public void IsInteraction(Transform trm) // �÷��̾�� �¾��� ��
    {
        _bossHP--;
        HpUpdate(_bossHP / _maxBossHP);

        if (_bossHP <= 0)
        {
            StopAllCoroutines();
            StartCoroutine(DeadRoutine());

            CameraShake.Instance.CameraShaking(_impuseSource, 0.2f);
            Vector2 difference = (transform.position - transform.position).normalized * 50 * _rigid.mass;
            _rigid.AddForce(difference, ForceMode2D.Impulse);
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
                yield return new WaitForSeconds(0.2f);

                _rigid.gravityScale = 0;
                _collider.enabled = false;
                door.SetActive(true);
                _isDead = true;
            }
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // ������ ������ �ֱ�
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
