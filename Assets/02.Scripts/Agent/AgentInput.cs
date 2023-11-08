using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class AgentInput : MonoBehaviour
{
    public UnityEvent<float> isPlayerMovementInput;
    public UnityEvent isJumpInput;
    public UnityEvent isAttackInput;

    [SerializeField] private LayerMask groundLayer; // �� ���̾ ������ ����

    private bool isGrounded = true; // ĳ���Ͱ� ���� �ִ��� ���θ� Ȯ���� ����

    private TeleportManager _teleportManager;

    private CinemachineImpulseSource _impulseSource;

    private void Start()
    {
        _teleportManager = GameObject.FindGameObjectWithTag("Door").GetComponent<TeleportManager>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        MovementInput();

        if (!_teleportManager.isControl) return;

        JumpInput();
        AttackInput();
    }

    private void MovementInput()
    {
        float x = Input.GetAxisRaw("Horizontal");

        if (!_teleportManager.isControl) // �����̸鼭 W�� ������ ���� ������ �� �ְ�
            isPlayerMovementInput?.Invoke(0);
        else
            isPlayerMovementInput?.Invoke(x);
    }

    private void JumpInput()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1.2f, groundLayer); // �� ���� ���� ������ ����

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            isJumpInput?.Invoke();
        }
    }

    private void AttackInput()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            isAttackInput?.Invoke();
            CameraShake.Instance.CameraShaking(_impulseSource, 0.2f);
        }
    }
}
