using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabbyBoss : MonoBehaviour, IInteraction
{
    private enum State
    {
        Idle, Walk, Attack 
    }
    private State state;

    public float PlayerHP
    {
        get => _playerHP;
        set => _playerHP = Mathf.Min(value, 30);
    }
    private float _playerHP = 30;

    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject attackEffectPrefab;

    private bool isWalking = false;

    private Vector3 _direction = Vector3.zero;
    private Animator _animator;

    private void Start()
    {
        state = State.Idle; // 처음엔 Idle 상태
        _animator = GetComponent<Animator>();
    }

    private void Thinking() // 패턴 생각
    {
        int randState = Random.Range(1, 3); // Idle 빼고

        switch ((State)randState)
        {
            case State.Walk:
                break;
            case State.Attack:
                break;
        }
    }

    private IEnumerator Walk() // 랜덤 시간이 지나면 방향을 바꾸면서 걸어다님
    {
        while (!isWalking)
        {
            int rand = Random.Range(0, 2);

            if (rand == 0)
            {
                _direction = Vector3.right * moveSpeed * Time.deltaTime;
            }
            else
            {
                _direction = Vector3.left * moveSpeed * Time.deltaTime;
            }
            isWalking = true;
        }

        transform.position += _direction.normalized;
        yield return new WaitForSeconds(5f); // 랜덤한 초로 바꾸기
        isWalking = false;
    }

    private IEnumerator Attack() // 일반 공격, 파티클 생성함
    {
        //공격 애니메이션
        GameObject effect = Instantiate(attackEffectPrefab, transform.position, Quaternion.identity);
        Destroy(effect, 0.5f);

        yield return null;
        Invoke("Thinking", 3f); // 다시 생각
    }

    public void IsInteraction(Transform trm) // 플레이어에게 맞았을 때
    {
        if (_playerHP <= 0)
        {
            // 죽은거
        }
        else
        {
            // hit
        }
    }
}
