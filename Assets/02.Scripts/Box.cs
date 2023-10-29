using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, IInteraction
{
    //[SerializeField] private GameObject[] itemPrefabs;
    // 랜덤으로 아이템도 생성해야함

    private ExplosiveEffect _explosive;
    private Animator _animator;

    private void Start()
    {
        _explosive = GetComponent<ExplosiveEffect>();
        _animator = GetComponent<Animator>();
    }

    IEnumerator BoxBurstRoutine()
    {
        _animator.SetBool("Shoot", true);
        yield return new WaitForSeconds(0.2f);

        _explosive.Explosion();

        Destroy(gameObject);
    }

    public void IsInteraction()
    {
        StartCoroutine(BoxBurstRoutine());
    }
}
