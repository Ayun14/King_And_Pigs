using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPViewer : MonoBehaviour
{
    private Transform fillTrm;

    private void Start()
    {
        fillTrm = transform.Find("HealthBar-Fill");
        fillTrm.localScale = new Vector3(1f, 1f, 1f);
    }

    public void HpUpdate(float normalizedScale)
    {
        Vector3 scale = fillTrm.localScale;
        scale.x = Mathf.Clamp(normalizedScale, 0, 1f);
        fillTrm.localScale = scale;
    }
}
