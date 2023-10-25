using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CinemachineSwitcher : Singleton<CinemachineSwitcher>
{
    [SerializeField] private CinemachineVirtualCamera[] virtualCameras;

    public void SwitchProiority(int cameraNumder)
    {
        // 높을 수록 우선 순위
        virtualCameras[cameraNumder-1].Priority = 2;

        for (int i = 0; i < virtualCameras.Length; i++)
        {
            if (virtualCameras[i] == virtualCameras[cameraNumder-1])
                continue;

            virtualCameras[i].Priority = 0;
        }
    }
}
