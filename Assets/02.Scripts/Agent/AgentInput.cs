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
    public UnityEvent isDashInput;

    [SerializeField] private LayerMask groundLayer; // 땅 레이어를 지정할 변수

    private bool isGrounded = true; // 캐릭터가 땅에 있는지 여부를 확인할 변수

    private Rigidbody2D _rigid;
    private AgentRender agentRender;
    private TeleportManager _teleportManager;
    private CinemachineImpulseSource _impulseSource;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        agentRender = GetComponentInChildren<AgentRender>();
        _teleportManager = GameObject.FindGameObjectWithTag("Door").GetComponent<TeleportManager>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        if (agentRender._isDead)
        {
            isPlayerMovementInput?.Invoke(0);
            return;
        }

        MovementInput();

        if (!_teleportManager.isControl) return;

        JumpInput();
        AttackInput();
        DashInput();
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
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1.2f, groundLayer); // 한 번만 점프 가능한 레이

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            isJumpInput?.Invoke();
            AudioManager.Instance.PlaySFX(AudioManager.Sfx.PlayerJump);
        }
    }

    private void AttackInput()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            isAttackInput?.Invoke();
            CameraShake.Instance.CameraShaking(_impulseSource, 0.2f);
            AudioManager.Instance.PlaySFX(AudioManager.Sfx.PlayerAttack);
        }
    }

    private void DashInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isDashInput?.Invoke();
        }
    }
}
