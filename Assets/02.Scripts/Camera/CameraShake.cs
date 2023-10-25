using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : Singleton<CameraShake>
{
    public void CameraShaking(CinemachineImpulseSource impulseSource, float globalShakeForce)
    {
        impulseSource.GenerateImpulseWithForce(globalShakeForce);
    }
}
