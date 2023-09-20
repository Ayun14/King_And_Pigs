using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    [SerializeField] private GameObject endDoorPos;
    [SerializeField] private GameObject player;

    private float _teleportTime = 0.5f;

    private bool _isDoorRange = false; // door 범위 안에 player가 있는지
    public bool isControl = true; // 플레이어 Input을 제어

    private Animator _animator;
    private Animator _endDoorAnimator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _endDoorAnimator = endDoorPos.GetComponent<Animator>();
    }

    public void TelePortMove()
    {
        if (_isDoorRange)
        {
            StartCoroutine(DoorInMoveRoutine());
        }
    }

    IEnumerator DoorInMoveRoutine() // In
    {
        _animator.SetBool("Door", true);
        isControl = false; // 플레이어 움직임 제어

        yield return new WaitForSeconds(_teleportTime);

        player.transform.position = endDoorPos.transform.position; // 텔레포트

        _animator.SetBool("Door", false);
        _endDoorAnimator.SetBool("Door", true);
        yield return new WaitForSeconds(_teleportTime);
        _endDoorAnimator.SetBool("Door", false);
        isControl = true;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enter");

        if (collision.CompareTag("Player"))
        {
            _isDoorRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Exit");

        if (collision.CompareTag("Player"))
        {
            _isDoorRange = false;
        }
    }
}
