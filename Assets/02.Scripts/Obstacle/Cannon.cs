using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private GameObject cannonBallPrefab;

    private float _delayTime = 3f;
    private float currentTime = 0;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CannonShoot();
    }

    private void CannonShoot()
    {
        currentTime += Time.deltaTime;

        if (currentTime > _delayTime)
        {
            _animator.SetBool("Shoot", true);
            Instantiate(cannonBallPrefab, transform.position, Quaternion.identity, transform);
            currentTime = 0;
        }
        _animator.SetBool("Shoot", false);
    }
}
