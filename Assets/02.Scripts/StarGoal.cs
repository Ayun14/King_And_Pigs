using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGoal : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject effectPrefab;

    private void Start()
    {
        door.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Star>())
        {
            Destroy(collision);

            GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 0.4f);

            door.SetActive(true); // 문 나오는 효과 좀 주기
            Destroy(gameObject);
        }
    }
}
