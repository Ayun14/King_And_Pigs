using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AgentInput : MonoBehaviour
{
    public UnityEvent<float> isPlayerMovementInput;
    public UnityEvent isJumpInput;

    [SerializeField] private LayerMask groundLayer; // 땅 레이어를 지정할 변수

    public bool isControl = true;
    private bool isGrounded = true; // 캐릭터가 땅에 있는지 여부를 확인할 변수

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
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer); // 한 번만 점프 가능한 레이

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            isJumpInput?.Invoke();
            Debug.Log("땅임 점프 ㄱㄴ");
        }
    }
}
