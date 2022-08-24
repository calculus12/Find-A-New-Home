using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

[System.Serializable]

public enum Swipe { None, Up, Down, Left, Right };
public class SwipeManager : MonoBehaviour
{
    private Touch playerTouch;
    private Vector2 startPos, currentPos;
    private float swipeAngle;
    public Swipe swipeDirection;
    private void Awake()
    {
        EnhancedTouchSupport.Enable();
    }
    void Start()
    {
        startPos = Vector2.zero;
        currentPos = Vector2.zero;
        swipeAngle = 0f;
    }

    // https://gamedev-resources.com/implementing-touch-with-input-systems-enhanced-touch-api/
    void Update()
    {
        if (Touch.activeFingers.Count > 0)
        {
            playerTouch = Touch.activeFingers[0].currentTouch;
            if (playerTouch.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                startPos = playerTouch.screenPosition;
            }
            // else if (playerTouch.phase == UnityEngine.InputSystem.TouchPhase.Moved)
            // {

            //     if ((currentPos - startPos).magnitude; > 100f)
            //     {
            //         CalculateSwipe();
            //     }
            // }
            else if (playerTouch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
            {
                CalculateSwipe();
            }
        }
        else
        {
            swipeDirection = Swipe.None;
        }
    }

    void CalculateSwipe()
    {
        currentPos = playerTouch.screenPosition;
        swipeAngle = Mathf.Atan2(currentPos.y - startPos.y, currentPos.x - startPos.x) * 180 / Mathf.PI;
        if (swipeAngle < 0)
        {
            swipeAngle += 360;
        }
        if (swipeAngle > 45 && swipeAngle <= 135)
        {
            swipeDirection = Swipe.Up;
        }
        else if (swipeAngle > 135 && swipeAngle <= 225)
        {
            swipeDirection = Swipe.Left;
        }
        else if (swipeAngle > 225 && swipeAngle <= 315)
        {
            swipeDirection = Swipe.Down;
        }
        else
        {
            swipeDirection = Swipe.Right;
        }
        Debug.Log($"Swipe: {swipeDirection}");
    }
}
