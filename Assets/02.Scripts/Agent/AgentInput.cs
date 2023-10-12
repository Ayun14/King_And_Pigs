using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AgentInput : MonoBehaviour
{
    public UnityEvent<float> isPlayerMovementInput;
    public UnityEvent isJumpInput;
    public UnityEvent isDoorTeleportInput;
    public UnityEvent isAttackInput;
    public UnityEvent isHit;

    [SerializeField] private LayerMask groundLayer; // �� ���̾ ������ ����

    private bool isGrounded = true; // ĳ���Ͱ� ���� �ִ��� ���θ� Ȯ���� ����

    private TeleportManager _teleportManager;

    private void Start()
    {
        _teleportManager = GameObject.FindGameObjectWithTag("Door").GetComponent<TeleportManager>();
    }

    private void Update()
    {
        MovementInput();

        if (!_teleportManager.isControl) return;

        JumpInput();
        DoorTeleportInput();
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

    private void DoorTeleportInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            isDoorTeleportInput?.Invoke();
        }
    }

    private void AttackInput()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            isAttackInput?.Invoke();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            isHit?.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            isHit?.Invoke();
        }
    }
}
