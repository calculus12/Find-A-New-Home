using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

[System.Serializable]

public enum Swipe { None, Up, Down, Left, Right };
public class SwipeManager : MonoBehaviour
{
    private Touch playerTouch;
    private Vector2 startPos, currentPos, touchDif;
    private float swipeSensitivity = 60f;
    public Swipe swipeDirection;
    private bool movedOnce = false;

    private List<int> boundaryAngles = new List<int> {50, 130, 230, 310};
        private void Awake()
    {
        EnhancedTouchSupport.Enable();
    }
    void Start()
    {
        startPos = Vector2.zero;
        currentPos = Vector2.zero;
    }

    // https://gamedev-resources.com/implementing-touch-with-input-systems-enhanced-touch-api/
    void FixedUpdate()
    {
        if (Touch.activeFingers.Count > 0)
        {
            playerTouch = Touch.activeTouches[0];
            if (playerTouch.phase == TouchPhase.Began)
            {
                startPos = playerTouch.screenPosition;
            }
            else if (playerTouch.phase == TouchPhase.Moved || playerTouch.phase == TouchPhase.Stationary)
            {
                if (!movedOnce) CalculateSwipe();
                else swipeDirection = Swipe.None;
            }
            else if (playerTouch.phase == TouchPhase.Ended)
            {
                movedOnce = false;
            }
        }
        else
        {
            swipeDirection = Swipe.None;
            movedOnce = false;
        }
    }

    void CalculateSwipe()
    {
        currentPos = playerTouch.screenPosition;
        touchDif = (currentPos - startPos);
        var swipeAngle = Mathf.Atan2(currentPos.y - startPos.y, currentPos.x - startPos.x) * 180 / Mathf.PI;
        if (swipeAngle < 0)
        {
            swipeAngle += 360;
        }
        if(touchDif.magnitude > swipeSensitivity)
        {
            if (swipeAngle > boundaryAngles[0] && swipeAngle <= boundaryAngles[1])
            {
                swipeDirection = Swipe.Up;
            }
            else if(swipeAngle > boundaryAngles[1] && swipeAngle <= boundaryAngles[2])
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

            movedOnce = true;
        }
        //터치.
        else
        {
            swipeDirection = Swipe.None;
        }
        //Debug.Log($"Swipe: {swipeDirection}");
    }
    
    
}
