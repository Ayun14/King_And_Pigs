using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cannon : MonoBehaviour, IInteraction
{
    public enum CannonType
    {
        Left,
        Right
    }

    [SerializeField] private GameObject cannonBallPrefab;
    [SerializeField] private Transform ballSpawnPos;
    public CannonType cannonType;

    private float _delayTime = 3f;
    private float currentTime = 0;

    private bool isLive = true;

    private BoxCollider2D _collider;
    private Rigidbody2D _rigid;
    private Animator _animator;

    private void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _rigid = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CannonShoot();
    }

    private void CannonShoot()
    {
        if (!isLive) return;

        currentTime += Time.deltaTime;

        if (currentTime > _delayTime)
        {
            _animator.SetTrigger("Shoot");
            Instantiate(cannonBallPrefab, ballSpawnPos.position, Quaternion.identity, transform);
            AudioManager.Instance.PlaySFX(AudioManager.Sfx.Cannon);
            currentTime = 0;
        }
    }

    public void IsInteraction(Transform trm) // 플레이어 한태 맞으면
    {
        isLive = false;

        Vector3 direction = (transform.position - trm.position).normalized * 15f * _rigid.mass;
        _rigid.AddForce(direction, ForceMode2D.Impulse);

        transform.DORotate(new Vector3(0, 0, -15), 0.5f);

        StartCoroutine(IsInteractionRoutine());
    }

    IEnumerator IsInteractionRoutine()
    {
        _animator.Play("CannonBreak");

        yield return new WaitForSeconds(2f);

        _rigid.gravityScale = 0;
        _collider.enabled = false;
    }
}
