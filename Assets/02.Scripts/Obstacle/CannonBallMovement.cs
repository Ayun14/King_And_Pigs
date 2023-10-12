using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallMovement : MonoBehaviour
{
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private LayerMask groundLayer; // �� ���̾ ������ ����

    private float _moveSpeed = 8f;

    private SpriteRenderer _cannonSpriteRenderer;
    private Vector3 _velocity;

    private void Start()
    {
        _cannonSpriteRenderer = GetComponentInParent<SpriteRenderer>();
    }

    private void Update()
    {
        BallMovement();

        #region effect ����
        if (Physics2D.Raycast(transform.position, Vector2.down, 0.01f, groundLayer)) // ���� ������
            EffectInstantiate();
        #endregion
    }

    private void EffectInstantiate()
    {
        GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
        Destroy(effect, 0.5f);
        Destroy(gameObject);
    }

    private void BallMovement()
    {
        if (_cannonSpriteRenderer.flipX == false) // �������� ��
            _velocity = Vector3.left * _moveSpeed;
        else if (_cannonSpriteRenderer.flipX == true) // ���������� ��
            _velocity = Vector3.right * _moveSpeed;

        Vector3 currentVelocity = _velocity * Time.deltaTime;

        transform.position += currentVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            EffectInstantiate();
        }
    }
}
