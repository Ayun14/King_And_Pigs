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

    [SerializeField] private LayerMask groundLayer; // 땅 레이어를 지정할 변수

    private bool isGrounded = true; // 캐릭터가 땅에 있는지 여부를 확인할 변수

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
    }

    private void MovementInput()
    {
        float x = Input.GetAxisRaw("Horizontal");

        if (!_teleportManager.isControl) // 움직이면서 W를 눌렀을 때도 정지할 수 있게
            isPlayerMovementInput?.Invoke(0);
        else
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

    private void DoorTeleportInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            isDoorTeleportInput?.Invoke();
        }
    }
}
