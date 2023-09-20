using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AgentInput : MonoBehaviour
{
    public UnityEvent<float> isPlayerMovementInput;
    public UnityEvent isJumpInput;

    [SerializeField] private LayerMask groundLayer; // �� ���̾ ������ ����

    public bool isControl = true;
    private bool isGrounded = true; // ĳ���Ͱ� ���� �ִ��� ���θ� Ȯ���� ����

    private void Update()
    {
        if (!isControl) return;

        MovementInput();
        JumpInput();
    }

    private void MovementInput()
    {
        float x = Input.GetAxisRaw("Horizontal");

        isPlayerMovementInput?.Invoke(x);
    }

    private void JumpInput()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer); // �� ���� ���� ������ ����

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            isJumpInput?.Invoke();
            Debug.Log("���� ���� ����");
        }
    }
}
