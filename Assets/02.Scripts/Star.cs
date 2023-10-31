using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour, IInteraction
{
    private Rigidbody2D _rigid;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    public void IsInteraction(Transform trm)
    {
        Vector3 direction = (transform.position - trm.position).normalized * 15f * _rigid.mass;
        _rigid.AddForce(direction, ForceMode2D.Impulse);
    }
}
