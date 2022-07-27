using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]

public enum SIDE
{
    LEFT,
    MID,
    RIGHT
}
public class Movement : MonoBehaviour
{
    public SIDE mSide = SIDE.MID;
    float NewXPos = 0f;
    float XValue = 2f;
    private PlayerControls playerControls;
    private CharacterController mChar;
    private float X;
    private float DodgeSpeed = 10f;
    private float JumpPower = 0.01f;
    private float Y;
    public bool InJump, InFall, InRoll;
    private float ColHeight;
    private float ColCenterY;
    private void Awake()
    {
        playerControls = new PlayerControls();
    }
    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        mChar = GetComponent<CharacterController>();
        ColHeight = mChar.height;
        ColCenterY = mChar.center.y;
        transform.position = new Vector3(0, 0.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControls.Player.Left.triggered)
        {
            if (mSide == SIDE.MID)
            {
                mSide = SIDE.LEFT;
                NewXPos = -XValue;
            }
            else if (mSide == SIDE.RIGHT)
            {
                mSide = SIDE.MID;
                NewXPos = 0f;
            }
        }
        else if (playerControls.Player.Right.triggered)
        {
            if (mSide == SIDE.MID)
            {
                mSide = SIDE.RIGHT;
                NewXPos = XValue;
            }
            else if (mSide == SIDE.LEFT)
            {
                mSide = SIDE.MID;
                NewXPos = 0f;
            }
        }
        Vector3 movement = new Vector3(X - transform.position.x, Y, 0);
        X = Mathf.Lerp(X, NewXPos, Time.deltaTime * DodgeSpeed);
        mChar.Move(movement);
        Jump();
        Roll();
    }
    private void Jump()
    {
        if (mChar.isGrounded)
        {
            if (InFall)
            {
                InJump = false;
                InFall = false;
            }
            if (playerControls.Player.Jump.triggered)
            {
                Y = JumpPower;
                InJump = true;
            }
        }
        else
        {
            Y -= JumpPower * 2 * Time.deltaTime;
            if (mChar.velocity.y < -0.1f)
            {
                InFall = true;
            }
        }
    }
    internal float RollCounter;
    private void Roll()
    {
        RollCounter -= Time.deltaTime;
        if (RollCounter <= 0f)
        {
            RollCounter = 0f;
            mChar.height = ColHeight;
            mChar.center = new Vector3(0, ColCenterY, 0);
            InRoll = false;
        }
        if (playerControls.Player.Roll.triggered)
        {
            RollCounter = 0.2f;
            Y -= 0.015f;
            mChar.height = ColHeight / 2f;
            mChar.center = new Vector3(0, ColCenterY / 2f, 0);
            InRoll = true;
            InJump = false;
        }
    }
}
