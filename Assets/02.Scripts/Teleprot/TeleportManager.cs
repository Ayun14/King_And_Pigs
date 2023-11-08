using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    [SerializeField] private GameObject endDoorPos;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject gameClearPanel;
    [SerializeField] private int camNum; // �� ��° ī�޶� ����� �ϳ�

    private float _teleportTime = 1.2f;
    private string doorName = "Door-Finish";

    private bool _isDoorRange; // door ���� �ȿ� player�� �ִ���
    public bool isControl = true; // �÷��̾� Input�� ����

    private Animator _animator;
    private Animator _endDoorAnimator;
    private Animator _playerAnimator;

    private void Start()
    {
        gameClearPanel.SetActive(false);

        _animator = GetComponent<Animator>();
        _endDoorAnimator = endDoorPos.GetComponent<Animator>();
        _playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (_isDoorRange && Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(DoorInMoveRoutine());
            Debug.Log(camNum);
        }
    }

    IEnumerator DoorInMoveRoutine()
    {
        Debug.Log("�ڷ�ƾ ����");
        _animator.SetBool("Door", true);
        _playerAnimator.SetBool("Teleport", true);
        isControl = false; // �÷��̾� ������ ����

        GameManager.Instance.FadeIn(1f);
        yield return new WaitForSeconds(_teleportTime);

        if (IsStageEndDoor()) yield break;
        CinemachineSwitcher.Instance.SwitchProiority(camNum);
        yield return new WaitForSeconds(0.1f);

        player.transform.position = endDoorPos.transform.position; // �ڷ���Ʈ

        GameManager.Instance.FadeOut(1f);
        _animator.SetBool("Door", false);
        _endDoorAnimator.SetBool("Door", true);
        _playerAnimator.SetBool("Teleport", false);

        yield return new WaitForSeconds(0.8f);

        _endDoorAnimator.SetBool("Door", false);
        isControl = true;
    }

    private bool IsStageEndDoor()
    {
        if (gameObject.name == doorName)
        {
            gameClearPanel.SetActive(true);
            Debug.Log("���� ����������");
            return true;
        }
        return false;
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
