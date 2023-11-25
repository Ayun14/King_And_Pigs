using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentRender : MonoBehaviour
{
    [SerializeField] private GameObject player;

    public bool _isDead;

    protected SpriteRenderer _spriteRenderer;
    protected Animator _animator;

    protected virtual void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    public void FaceDirection(float x)
    {
        if (x < 0)
        {
            player.transform.localScale = new Vector3(-1, 1, 1);
            _animator.SetBool("Walk", true);
        }
        else if (x > 0)
        {
            player.transform.localScale = new Vector3(1, 1, 1);
            _animator.SetBool("Walk", true);
        }
        else
            _animator.SetBool("Walk", false);
    }

    public void HitAnimation()
    {
        _animator.Play("Hit");
        AudioManager.Instance.PlaySFX(AudioManager.Sfx.PlayerHit);
    }

    public void DeadAnimation()
    {
        _animator.Play("Dead");
        _isDead = true;
    }

    public void AttackAnimation()
    {
        _animator.SetTrigger("Attack");
    }
}
