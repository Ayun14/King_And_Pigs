using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SeashellBoss : MonoBehaviour, IInteraction
{
    private enum State
    {
        Idle, JumpBiteAttack, Fire, OpenFire
    }
    private State state;

    [SerializeField] private float _maxBossHP;
    private float _bossHP;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float moveSpeed;

    [Header("JumpBiteAttack")]
    [SerializeField] private Transform player;
    private float _currentAttackTime = 0;
    private float _attackTime = 8f; // ���� ���� �ð�
    private float _attackDelay = 1.7f;
    private Vector3 _forecDirection; // add force�� �� ���� ���� (Fire�� ���)

    [Header("Fire")]
    [SerializeField] private GameObject pearlPrefab;
    [SerializeField] private Transform pearlSpawnPos;
    [SerializeField] private int fireCnt = 3;
    private float _currentScaleX = 0;
    private float _pearlEuler; // ���� ȸ��
    private bool _isGroundedLeft;
    private bool _isGroundedRight;

    [Header("OpenFire")]
    [SerializeField] private int openFireCnt = 20;

    [Header("Clear")]
    [SerializeField] private GameObject hpBar;
    [SerializeField] private GameObject hpBarFill;
    [SerializeField] private GameObject door;

    private bool _isDead;
    private bool _isGrounded;

    private int _beforeRandState = 0;

    private ExplosiveEffect _deadEffect;
    private Rigidbody2D _rigid;
    private Animator _animator;
    private BoxCollider2D _collider;
    private CinemachineImpulseSource _impuseSource;

    private void Start()
    {
        door.SetActive(false);
        hpBar.SetActive(true);

        state = State.Idle; // ó���� Idle ����

        _deadEffect = GetComponent<ExplosiveEffect>();
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

    private void Thinking() // ���� ����
    {
        if (_isDead) return;

        int randState = Random.Range(1, 4); // Idle ����

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

    IEnumerator JumpBiteAttack()
    {
        while (_currentAttackTime < _attackTime)
        {
            _currentAttackTime += Time.deltaTime;

            Vector2 directionToPlayer = player.position - transform.position;
            float direction = Mathf.Sign(transform.localScale.x);
            float dotProduct = Vector2.Dot(directionToPlayer.normalized, Vector2.right * direction);

            if (dotProduct > 0) // ������
            {
                _forecDirection = Vector3.right + Vector3.up;

                transform.localScale = new Vector3(-2, 2, 2);
                _animator.SetTrigger("Bite");
            }
            else if (dotProduct < 0) // ����
            {
                _forecDirection = Vector3.left + Vector3.up;

                transform.localScale = new Vector3(2, 2, 2);
                _animator.SetTrigger("Bite");
            }

            yield return new WaitForSeconds(_attackDelay);
        }

        Thinking();
    }

    public void JumpBiteAttackAnimationEvent() // ���� �ϴ� �ִϸ��̼��ϴ� ���� add force�ֱ�
    {
        _rigid.AddForce(_forecDirection * Mathf.PI * 4, ForceMode2D.Impulse);
        AudioManager.Instance.PlaySFX(AudioManager.Sfx.BossAttack);
    }

    IEnumerator Fire()
    {
        yield return null;

        Vector2 directionToPlayer = player.position - transform.position;
        float direction = Mathf.Sign(transform.localScale.x);
        float dotProduct = Vector2.Dot(directionToPlayer.normalized, Vector2.right * direction);

        if (dotProduct > 0) // ������
        {
            transform.localScale = new Vector3(2, 2, 2);
            _currentScaleX = transform.localScale.x;

            _forecDirection = Vector3.left + Vector3.up;
        }
        else if (dotProduct < 0) // ����
        {
            transform.localScale = new Vector3(-2, 2, 2);
            _currentScaleX = transform.localScale.x;

            _forecDirection = Vector3.right + Vector3.up;
        }

        while (true) // �̰� ��ġ��
        {
            _isGroundedRight = Physics2D.Raycast(transform.position, Vector2.right, 3f, groundLayer);
            _isGroundedLeft = Physics2D.Raycast(transform.position, Vector2.left, 3f, groundLayer);
            Debug.DrawRay(transform.position, Vector2.left * 3f, Color.blue);
            Debug.DrawRay(transform.position, Vector2.right * 3f, Color.blue);

            if (_isGroundedRight || _isGroundedLeft)
            {
                transform.localScale = new Vector3(-_currentScaleX, 2, 2);
                yield return new WaitForSeconds(1.5f);
                break;
            }

            JumpBiteAttackAnimationEvent();
            yield return new WaitForSeconds(1f);
            yield return null;
        }

        for (int i = 0; i < fireCnt; i++)
        {
            _animator.SetTrigger("Fire");
            yield return new WaitForSeconds(1f);
        }

        if (_currentScaleX > 0)
        {
            transform.localScale = new Vector3(-2, 2, 2);
            _forecDirection = Vector3.right + Vector3.up;
        }
        else
        {
            transform.localScale = new Vector3(2, 2, 2);
            _forecDirection = Vector3.left + Vector3.up;
        }

        for (int i = 0; i < 3; i++)
        {
            JumpBiteAttackAnimationEvent();
            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(2f);
        Thinking();
    }

    public void FireAnimationEvent()
    {
        if (_currentScaleX > 0) // �������� ���
            _pearlEuler = 0;
        else // ���������� ���
            _pearlEuler = 180;
        Instantiate(pearlPrefab, pearlSpawnPos.position, Quaternion.Euler(new Vector3(0, 0, _pearlEuler)));
        AudioManager.Instance.PlaySFX(AudioManager.Sfx.SeaShellAttack);
    }

    IEnumerator OpenFire()
    {
        yield return null;
        _animator.SetBool("OpenFire", true);
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < openFireCnt; i++)
        {
            int angle = Random.Range(0, 180);
            Instantiate(pearlPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, angle)));
            AudioManager.Instance.PlaySFX(AudioManager.Sfx.SeaShellAttack);
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(1f);
        _animator.SetBool("OpenFire", false);
        yield return new WaitForSeconds(2f);

        Thinking();
    }

    public void CameraShakeAnimationEvent()
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
                _deadEffect.Explosion(); // ���� ���� ������ & ������

                GameManager.Instance.DoSlowMotion();

                door.SetActive(true);
                _isDead = true;

                gameObject.SetActive(false);
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
