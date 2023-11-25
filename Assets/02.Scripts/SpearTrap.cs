using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpearTrap : MonoBehaviour
{
    public void MoveAttack(float x)
    {
        transform.DOMoveX(x, 0.1f).OnComplete(() =>
        {
            AudioManager.Instance.PlaySFX(AudioManager.Sfx.CaptainAttack);
            transform.DOMove(new Vector2(x, -11), 0.6f).OnComplete(() =>
            {
                transform.DOMove(new Vector2(x, -17), 0.6f);
            });
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.TakeDamage(1f);
        }
    }
}
