using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cannon : MonoBehaviour
{
    public UnityEvent<Vector3> isCannonShoot;

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

            if (!_spriteRenderer.flipX) // ¿ÞÂÊÀ¸·Î ½ò ¶§
                isCannonShoot?.Invoke(Vector3.left);
            else if (_spriteRenderer.flipX) // ¿À¸¥ÂÊÀ¸·Î ½ò ¶§
                isCannonShoot?.Invoke(Vector3.right);

            currentTime = 0;
        }
        _animator.SetBool("Shoot", false);
    }

    IEnumerator CannonRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_delayTime);


            _animator.SetBool("Shoot", true);
            yield return new WaitForSeconds(0.01f);


            Instantiate(cannonBallPrefab, transform.position, Quaternion.identity);

            yield return new WaitForSeconds(0.02f);
            _animator.SetBool("Shoot", false);

            yield return null;
        }
        yield return null;
    }
}
