using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveEffect : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private GameObject[] itmePrefabs;
    [SerializeField] private float force = 30f;

    public void Explosion()
    {
        foreach (GameObject prefab in prefabs)
        {
            GameObject clone = Instantiate(prefab, transform.position, Quaternion.identity);
            IsExplosion(clone);
        }

        int rand = Random.Range(1, 50);
        if (rand < 5)
        {
            GameObject clone = Instantiate(itmePrefabs[0], transform.position, Quaternion.identity);
            IsExplosion(clone);
        }
        //else if (rand < 10)
        //{
        //    GameObject clone = Instantiate(itmePrefabs[1], transform.position, Quaternion.identity);
        //    IsExplosion(clone);
        //}
    }

    private void IsExplosion(GameObject clone)
    {
        Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(0.1f, 1f)).normalized;
        Rigidbody2D rigid = clone.GetComponent<Rigidbody2D>();
        rigid.AddForce(randomDirection * force, ForceMode2D.Impulse);
    }
}
