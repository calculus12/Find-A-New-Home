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

    private List<int> boundaryAngles = new List<int> {60, 150, 210, 300};
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
    void FixedUpdate()
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
        if (swipeAngle > boundaryAngles[0] && swipeAngle <= boundaryAngles[1])
        {
            swipeDirection = Swipe.Up;
        }
        else if (swipeAngle > boundaryAngles[1] && swipeAngle <= boundaryAngles[2])
        {
            swipeDirection = Swipe.Left;
        }
        else if (swipeAngle > boundaryAngles[2] && swipeAngle <= boundaryAngles[3])
        {
            swipeDirection = Swipe.Down;
        }
        else
        {
            swipeDirection = Swipe.Right;
        }
        //Debug.Log($"Swipe: {swipeDirection}");
    }
}
