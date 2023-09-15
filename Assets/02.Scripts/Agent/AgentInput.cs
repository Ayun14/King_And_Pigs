using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AgentInput : MonoBehaviour
{
    public UnityEvent<float> isPlayerMovementInput;
    public UnityEvent isJumpInput;

    private bool isGrounded; // ĳ���Ͱ� ���� �ִ��� ���θ� Ȯ���� ����

    private void Update()
    {
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
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.1f);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            isJumpInput?.Invoke();
    }
}
