using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EnemyHealth : MonoBehaviour
{
    public Action<Transform, float> isKnockBack;

    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private float maxHP;
    private float _currnetHP;

    public float MaxHP => maxHP;
    public float CurrentHP => _currnetHP;

    public bool isDead = false;

    private HPViewer _hpViewer;
    private Rigidbody2D _rigid;

    private CinemachineImpulseSource _impulseSource;

    private void Start()
    {
        _currnetHP = maxHP;

        boxCollider.enabled = true;

        _hpViewer = GetComponentInChildren<HPViewer>();
        _rigid = GetComponent<Rigidbody2D>();

        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void TakeDamage(float damage, Transform targetPos)
    {
        if (isDead) return;

        _currnetHP -= damage;
        _hpViewer.HpUpdate(_currnetHP / maxHP);
        Debug.Log("enemy hp : "+ _currnetHP);

        StartCoroutine(DeadCheak(targetPos));
    }

    IEnumerator DeadCheak(Transform targetPos)
    {
        if (_currnetHP <= 0)
        {
            isDead = true;
            _rigid.gravityScale = 0;
            boxCollider.enabled = false;

            CameraShake.Instance.CameraShaking(_impulseSource, 1.5f);

            isKnockBack?.Invoke(targetPos, 50f);

            yield return new WaitForSeconds(2f);

            Destroy(gameObject);
        }
    }


}