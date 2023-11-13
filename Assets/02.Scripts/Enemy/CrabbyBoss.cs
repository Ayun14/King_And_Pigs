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

    public float BossHP
    {
        get => _bossHP;
        set => _bossHP = Mathf.Min(value, 30);
    }
    private float _bossHP = 30;

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
    //[SerializeField] private GameObject hpBar;
    //[SerializeField] private GameObject door;

    private bool isWalking = false;

    private Vector3 _direction;
    private Rigidbody2D _rigid;
    private Animator _animator;
    private CinemachineImpulseSource _impuseSource;

    private void Start()
    {
        //door.SetActive(false);
        state = State.Idle; // 처음엔 Idle 상태
        _rigid = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _impuseSource = GetComponent<CinemachineImpulseSource>();

        Thinking(); // 첫 패턴 생각
    }

    private void ChangeState(State newState)
    {
        state = newState;
    }

    private void Update()
    {
        transform.position += _direction.normalized * moveSpeed * Time.deltaTime;
    }

    private void Thinking() // 패턴 생각
    {
        int randState = Random.Range(1, 5); // Idle 빼고
        ChangeState((State)randState);
        Debug.Log((State)randState);

        StartCoroutine(state.ToString()); // 지금 상태 이름의 코루틴 실행
    }

    private IEnumerator Walk() // 랜덤 시간이 지나면 방향을 바꾸면서 걸어다님
    {
        yield return new WaitForSeconds(1f);

        isWalking = false;
        _animator.SetBool("Walk", true);
        int directionChangeCnt = Random.Range(3, 5);

        while (!isWalking)
        {
            while (directionChangeCnt > 0)
            {
                if (directionChangeCnt % 2 == 0)
                {
                    _direction = Vector3.right;
                    transform.localScale = new Vector2(-3, 3);
                }
                else
                {
                    _direction = Vector3.left;
                    transform.localScale = new Vector2(3, 3);
                }

                yield return new WaitForSeconds(3f);
                directionChangeCnt--;
            }

            isWalking = true;
        }

        _direction = Vector3.zero;
        _animator.SetBool("Walk", false);
        Thinking(); // 다른 패턴 생각
    }

    private IEnumerator Attack() // 일반 공격, 파티클 생성함
    {
        yield return new WaitForSeconds(1f);
        _animator.SetTrigger("Attack");

        GameObject effect = Instantiate(attackEffectPrefab, transform.position, Quaternion.identity);
        Destroy(effect, 0.5f);

        yield return new WaitForSeconds(5f);
        Thinking();
    }

    private IEnumerator JumpAttack()
    {
        // 점프한뒤 내려와서 폭풍을 만드는데 그거에 닿으면 데미지를 입는
        yield return new WaitForSeconds(1f);

        _rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        _animator.SetBool("Jump", true);

        yield return new WaitForSeconds(1f);

        transform.DOMove(player.transform.position, 1f); // 플레이어 위치로 이동

        bool isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 3f, groundLayer);
        while (!isGrounded)
        {
            Debug.DrawRay(transform.position, Vector2.down * 3f, Color.blue);
            isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 3f, groundLayer);
            yield return null;
            Debug.Log(isGrounded);
        }
        CameraShake.Instance.CameraShaking(_impuseSource, 1f); // 메인 가서 만지기
        _animator.SetBool("Jump", false);

        Instantiate(leftJumpAttackEffectPrefab, leftEffectSpawn.position, Quaternion.identity);
        Instantiate(rightJumpAttackEffectPrefab, rightEffectSpawn.position, Quaternion.identity);

        yield return new WaitForSeconds(5f);

        Thinking();
    }

    private IEnumerator RushAttack()
    {
        // 플레이어 있는 쪽으로 돌진
        int rushCnt = 2;
        while (rushCnt > 0)
        {
            yield return new WaitForSeconds(1.5f);
            Vector2 directionToPlayer = player.transform.position - transform.position;
            float direction = Mathf.Sign(transform.localScale.x);
            float dotProduct = Vector2.Dot(directionToPlayer.normalized, Vector2.right * direction);

            if (dotProduct > 0) // 오른쪽
            {
                _rigid.AddForce(Vector2.right * rushPower, ForceMode2D.Impulse);
                _animator.SetBool("Rush", true);
            }
            else if (dotProduct < 0) // 왼쪽
            {
                _rigid.AddForce(Vector2.left * rushPower, ForceMode2D.Impulse);
                _animator.SetBool("Rush", true);
            }

            yield return new WaitForSeconds(1.5f);
            _animator.SetBool("Rush", false);
            _rigid.velocity = Vector2.zero;

            rushCnt--;
        }

        yield return new WaitForSeconds(2f);
        Thinking();
    }

    public void IsInteraction(Transform trm) // 플레이어에게 맞았을 때
    {
        _bossHP--;

        if (_bossHP <= 0)
        {
            _animator.SetTrigger("Dead");
            //door.SetActive(true);
        }
        else
        {
            _animator.SetTrigger("Hit");
        }
    }

    //public void HpUpdate(float normalizedScale)
    //{
    //    Vector3 scale = hpBar.transform.localScale;
    //    scale.x = Mathf.Clamp(normalizedScale, 0, 1f);
    //    hpBar.transform.localScale = scale;
    //}
}
