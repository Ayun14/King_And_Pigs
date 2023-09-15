using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class AgentMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float jumpForce = 10f; // ���� ������ ������ ����

    private Vector3 _velocity;

    private Rigidbody2D _rigid;

    private void Start()
    {
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

    // x �̵� ���� ����. �ܺο��� ȣ��
    public void MoveTo(float x)
    {
        _velocity.x = x * moveSpeed;
    }

    public void JumpTo()
    {
        _rigid.velocity = new Vector2(_rigid.velocity.x, jumpForce);
    }
}
