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
    [SerializeField] private int camNum; // 몇 번째 카메라가 비춰야 하냐
    [SerializeField] private Door door;
    [SerializeField] private GameObject boss;

    private float _teleportTime = 1.2f;
    private string doorName = "Door-Finish";

    private bool _isDoorRange; // door 범위 안에 player가 있는지
    public bool isControl = true; // 플레이어 Input을 제어

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
        isControl = false; // 플레이어 움직임 제어

        GameManager.Instance.FadeIn(1f);
        yield return new WaitForSeconds(_teleportTime);

        if (IsStageEndDoor()) yield break;
        if (door == Door.BossDoor) boss.SetActive(true);

        CinemachineSwitcher.Instance.SwitchProiority(camNum);
        yield return new WaitForSeconds(0.1f);

        player.transform.position = endDoorPos.transform.position; // 텔레포트

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
