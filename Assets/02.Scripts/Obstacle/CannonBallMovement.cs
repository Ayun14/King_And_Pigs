using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallMovement : MonoBehaviour
{
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private LayerMask groundLayer; // ¶¥ ·¹ÀÌ¾î¸¦ ÁöÁ¤ÇÒ º¯¼ö

    private float _moveSpeed = 8f;

    private Cannon _cannon;
    private Vector3 _velocity;

    private void Start()
    {
        _cannon = GetComponentInParent<Cannon>();
    }

    private void Update()
    {
        BallMovement();

        #region effect »ý¼º
        if (Physics2D.Raycast(transform.position, Vector2.down, 0.01f, groundLayer)) // ¶¥¿¡ ´êÀ¸¸é
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
        if (_cannon.cannonType == Cannon.CannonType.Left) // ¿ÞÂÊÀ¸·Î ½ô
            _velocity = Vector3.left * _moveSpeed;
        else if (_cannon.cannonType == Cannon.CannonType.Right) // ¿À¸¥ÂÊÀ¸·Î ½ô
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
