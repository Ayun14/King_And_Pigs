using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class AgentMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float jumpForce = 10f; // 점프 강도를 조절할 변수

    private bool isDashing = false;

    private float _dashTime = 0.5f;
    private float _dashCoolTime = 0.1f;
    private float _defultSpeed;
    private Vector3 _velocity;
    private Rigidbody2D _rigid;

    private void Start()
    {
        _defultSpeed = moveSpeed;
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Movement2D();
    }

    private void Movement2D()
    {
        Vector3 currentVelocity = _velocity * Time.deltaTime;

        transform.position += currentVelocity;
    }

    // x 이동 방향 설정
    public void MoveTo(float x)
    {
        _velocity.x = x * moveSpeed;
    }

    // jump 설정
    public void JumpTo()
    {
        _rigid.velocity = new Vector2(_rigid.velocity.x, jumpForce);
    }

    public void Dash()
    {
        if (isDashing) return;

        isDashing = true;
        moveSpeed = dashSpeed;
        StartCoroutine(DashRoutine());
    }

    IEnumerator DashRoutine()
    {
        yield return new WaitForSeconds(_dashTime);
        moveSpeed = _defultSpeed;
        yield return new WaitForSeconds(_dashCoolTime);
        isDashing = false;
    }
}
