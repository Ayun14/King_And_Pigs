using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CannonBallMovement : MonoBehaviour
{
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private Vector2 colliderSize;
    [SerializeField] private float ballLifeTime = 4.5f;
    [SerializeField] private float damage = 1f; // ����� 2�� ��

    private float _currentBallLifeTime = 0;
    private float _moveSpeed = 6f;

    private Rigidbody2D _rigid;
    private Cannon _cannon;
    private Vector3 _velocity;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _cannon = GetComponentInParent<Cannon>();
    }

    private void Update()
    {
        BallMovement();
        BallDamageCheak();
    }

    private void BallDamageCheak()
    {
        _currentBallLifeTime += Time.deltaTime;
        if (_currentBallLifeTime >= ballLifeTime) // ���� �ð� ������ ����
        {
            EffectInstantiate();
            _currentBallLifeTime = 0;
        }
    }

    private void EffectInstantiate()
    {
        GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(transform.position, colliderSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            EnemyHealth enemyHeath = collider.GetComponent<EnemyHealth>();

            if (enemyHeath)
            {
                Debug.Log("������ ����");
                enemyHeath.TakeDamage(damage, this.transform);
            }
            // �÷��̾�� ����� �ֱ�
            Debug.Log("�÷��̾�� ����");
        }

        Destroy(effect, 0.5f);
        Destroy(gameObject);
    }

    private void BallMovement()
    {
        if (_cannon.cannonType == Cannon.CannonType.Left) // �������� ��
            _velocity = Vector3.left * _moveSpeed;
        else if (_cannon.cannonType == Cannon.CannonType.Right) // ���������� ��
            _velocity = Vector3.right * _moveSpeed;

        Vector3 currentVelocity = _velocity * Time.deltaTime;

        transform.position += currentVelocity;
    }

    public void GetKnockBack(Transform damageSource, float knockbackThrust)
    {
        Vector2 difference = (transform.position - damageSource.position).normalized * knockbackThrust * _rigid.mass;
        difference.y = 0; // x�࿡ �����ϱ� ����
        _rigid.gravityScale = 0;

        _rigid.AddForce(difference, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EffectInstantiate();
        }
    }
}
