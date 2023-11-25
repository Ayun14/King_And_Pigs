using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TeleportManager : MonoBehaviour
{
    private enum Door
    {
        None, BossDoor, BossStart
    }

    [SerializeField] private GameObject endDoorPos;
    [SerializeField] private GameObject player;
    [SerializeField] private Image gameClearPanel;
    [SerializeField] private Image fadePanel;
    [SerializeField] private int camNum; // �� ��° ī�޶� ����� �ϳ�
    [SerializeField] private Door door;
    [SerializeField] private GameObject boss;

    private float _teleportTime = 1.2f;
    private string doorName = "Door-Finish";

    private bool _isDoorRange; // door ���� �ȿ� player�� �ִ���
    public bool isControl = true; // �÷��̾� Input�� ����

    private Animator _animator;
    private Animator _endDoorAnimator;
    private Animator _playerAnimator;

    private void Start()
    {
        boss.SetActive(false);

        _animator = GetComponent<Animator>();
        _endDoorAnimator = endDoorPos.GetComponent<Animator>();
        _playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (door == Door.BossStart)
        {
            AudioManager.Instance.PlayBGM(AudioManager.BGM.Boss);
            return;
        }

        if (_isDoorRange && Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(DoorInMoveRoutine());
        }
    }

    IEnumerator DoorInMoveRoutine()
    {
        _animator.SetBool("Door", true);
        _playerAnimator.SetBool("Teleport", true);
        isControl = false; // �÷��̾� ������ ����

        GameManager.Instance.FadeIn(1f);
        yield return new WaitForSeconds(_teleportTime);

        if (IsStageEndDoor()) yield break;
        if (door == Door.BossDoor) boss.SetActive(true);

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
            gameClearPanel.rectTransform.DOAnchorPosY(0, 1);
            AudioManager.Instance.PlaySFX(AudioManager.Sfx.GameClear);
            return true;
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isDoorRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isDoorRange = false;
        }
    }
}
