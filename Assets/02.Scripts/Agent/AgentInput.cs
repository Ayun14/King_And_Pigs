using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AgentInput : MonoBehaviour
{
    public UnityEvent<Vector2> OnMovementKeyPressed = null;
    public UnityEvent<Vector2> OnPointerPoistionChanged = null; // 마우스 방향에 맞춰서 캐릭터 회전

    private Camera _mainCam;

    void Start()
    {
        _mainCam = Camera.main;
    }

    private void GetMovementInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2(x, y);
        OnMovementKeyPressed?.Invoke(direction.normalized);
    }

    private void GetPointerInput()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;
        Vector2 mousePosInWorld = (Vector2)_mainCam.ScreenToWorldPoint(mousePos);

        OnPointerPoistionChanged?.Invoke(mousePosInWorld);
    }
}
