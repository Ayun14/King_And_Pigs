using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    [SerializeField] private GameObject endDoorPos;
    [SerializeField] private GameObject player;

    private float _teleportTime = 0.6f;

    private bool _isDoorRange = false; // door ���� �ȿ� player�� �ִ���
    public bool isControl = true; // �÷��̾� Input�� ����

    private Animator _animator;
    private Animator _endDoorAnimator;
    private Animator _playerAnimator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _endDoorAnimator = endDoorPos.GetComponent<Animator>();
        _playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>();
    }

    public void TelePortMove()
    {
        if (_isDoorRange)
        {
            StartCoroutine(DoorInMoveRoutine());
        }
    }

    IEnumerator DoorInMoveRoutine()
    {
        _animator.SetBool("Door", true);
        _playerAnimator.SetBool("Teleport", true);
        isControl = false; // �÷��̾� ������ ����

        yield return new WaitForSeconds(_teleportTime);

        player.transform.position = endDoorPos.transform.position; // �ڷ���Ʈ

        _animator.SetBool("Door", false);
        _endDoorAnimator.SetBool("Door", true);
        _playerAnimator.SetBool("Teleport", false);

        yield return new WaitForSeconds(_teleportTime);

        _endDoorAnimator.SetBool("Door", false);
        _playerAnimator.SetBool("GoBackIdle", true);
        isControl = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _playerAnimator.SetBool("GoBackIdle", false);
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
