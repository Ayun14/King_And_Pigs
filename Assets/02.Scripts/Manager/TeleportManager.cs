using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    [SerializeField] private GameObject endDoorPos;
    [SerializeField] private GameObject player;
    [SerializeField] private int camNum; // �� ��° ī�޶� ����� �ϳ�

    private float _teleportTime = 0.6f;

    private bool _isDoorRange; // door ���� �ȿ� player�� �ִ���
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

    private void Update()
    {
        if (_isDoorRange && Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(DoorInMoveRoutine());
            CinemachineSwitcher.Instance.SwitchProiority(camNum);
            Debug.Log(camNum);
        }
    }

    IEnumerator DoorInMoveRoutine()
    {
        Debug.Log("�ڷ�ƾ ����");
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
        isControl = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Enter");
            _isDoorRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Exit");
            _isDoorRange = false;
        }
    }
}
