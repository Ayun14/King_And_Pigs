using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KnockBack : MonoBehaviour
{
    public bool isKnockBack = false;

    private Rigidbody2D _rigid;
    private EnemyHealth _enemyHealth;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _enemyHealth = GetComponent<EnemyHealth>();

        _enemyHealth.isKnockBack += GetKnockBack;
    }

    private void OnDestroy()
    {
        _enemyHealth.isKnockBack -= GetKnockBack;
    }

    public void GetKnockBack(Transform damageSource, float knockbackThrust)
    {
        isKnockBack = true;
        Vector2 difference = (transform.position - damageSource.position).normalized * knockbackThrust * _rigid.mass;
        _rigid.AddForce(difference, ForceMode2D.Impulse);
    }
}
