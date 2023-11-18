using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    private float _moveSpeed = 20f;

    private void Update()
    {
        transform.Translate(_moveSpeed * Vector2.right * Time.deltaTime);

        if (Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("플레이어한테 knife가 데미지줌");
            //GameManager.Instance.TakeDamage(1);
        }
    }
}
