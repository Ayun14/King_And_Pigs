using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentRender : MonoBehaviour
{
    [SerializeField] private GameObject player;

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
        _animator.SetTrigger("Hit");
    }

    public void AttackAnimation()
    {
        _animator.SetTrigger("Attack");
    }
}
