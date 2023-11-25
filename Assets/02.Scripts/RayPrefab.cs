using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayPrefab : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        AudioManager.Instance.PlaySFX(AudioManager.Sfx.ToothAttack);
    }

    public void RayOffAnimator()
    {
        _animator.SetTrigger("Off");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.TakeDamage(1f);
        }
    }
}
