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
        state = State.Idle; // ó���� Idle ����
        _animator = GetComponent<Animator>();
    }

    private void Thinking() // ���� ����
    {
        int randState = Random.Range(1, 3); // Idle ����

        switch ((State)randState)
        {
            case State.Walk:
                break;
            case State.Attack:
                break;
        }
    }

    private IEnumerator Walk() // ���� �ð��� ������ ������ �ٲٸ鼭 �ɾ�ٴ�
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
        yield return new WaitForSeconds(5f); // ������ �ʷ� �ٲٱ�
        isWalking = false;
    }

    private IEnumerator Attack() // �Ϲ� ����, ��ƼŬ ������
    {
        //���� �ִϸ��̼�
        GameObject effect = Instantiate(attackEffectPrefab, transform.position, Quaternion.identity);
        Destroy(effect, 0.5f);

        yield return null;
        Invoke("Thinking", 3f); // �ٽ� ����
    }

    public void IsInteraction(Transform trm) // �÷��̾�� �¾��� ��
    {
        if (_playerHP <= 0)
        {
            // ������
        }
        else
        {
            // hit
        }
    }
}
