using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    [SerializeField] private GameObject endDoorPos;
    [SerializeField] private GameObject player;

    private float _teleportTime = 0.5f;

    private Animator _animator;
    private Animator _playerAnimator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _playerAnimator = player.GetComponent<Animator>();
    }

    IEnumerator TeleportMover()
    {
        yield return null;
        _animator.SetBool("Door", true);
        _playerAnimator.SetBool("Teleport", true);

        yield return new WaitForSeconds(_teleportTime);

        player.GetComponent<AgentInput>().isControl = false; // 플레이어 움직임 제어
        player.transform.position = endDoorPos.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(TeleportMover());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _animator.SetBool("Door", false);
        _playerAnimator.SetBool("Teleport", false);

        StartCoroutine(waitTeleportTime());

        player.GetComponent<AgentInput>().isControl = true;
    }

    IEnumerator waitTeleportTime()
    {
        yield return new WaitForSeconds(_teleportTime);
    }
}
