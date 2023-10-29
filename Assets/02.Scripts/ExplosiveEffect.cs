using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveEffect : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private float force = 30f;

    public void Explosion()
    {
        foreach (GameObject prefab in prefabs)
        {
            Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(0.1f, 1f)).normalized;

            GameObject clone = Instantiate(prefab, transform.position, Quaternion.identity);
            Rigidbody2D rigid = clone.GetComponent<Rigidbody2D>();

            rigid.AddForce(randomDirection * force, ForceMode2D.Impulse);
            Debug.Log("¹Ú½º Æ¨±â±â");
        }
    }
}
