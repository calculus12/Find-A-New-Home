using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[System.Serializable]

/// 캐릭터 레일 위치 enum
public enum SIDE
{
    LEFT,
    MID,
    RIGHT
}
public class Movement : MonoBehaviour
{
    public SIDE mSide = SIDE.MID; // 캐릭터 레일 위치
    float NewXPos = 0f;
    float XValue = 2f;
    private PlayerControls playerControls;
    private CharacterController mChar;
    private Animator mAnimator;
    private float X, Y;
    private float DodgeSpeed = 10f;
    [SerializeField] float JumpPower;
    public bool InJump, InFall, InRoll, InRecovery;
    private float PauseDelay, DodgeDelay;
    public SwipeManager swipeManager;
    [SerializeField] private float delayConstant = 0.1f;
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
    void Start()
    {
        mChar = GetComponent<CharacterController>();
        mAnimator = GetComponent<Animator>();
        transform.position = new Vector3(0f, 0f, 0f);
        Y = 0f;
        PauseDelay = 0f;
        DodgeDelay = 0f;
    }

    void FixedUpdate()
    {
        // 일시정지 중이면 입력 무효화
        if (GameManager.Instance.GameState == GameState.pause)
        {
            PauseDelay = 0.2f;
            return;
        }
        else if (PauseDelay > 0)
        {
            PauseDelay -= Time.fixedDeltaTime;
            return;
        }

        // 캐릭터 레일 위치에 따른 컨트롤 및 X값 설정
        if (DodgeDelay > 0)
        {
            DodgeDelay -= Time.fixedDeltaTime;
        }
        else if (playerControls.Player.Left.triggered || swipeManager.swipeDirection == Swipe.Left)
        {
            if (mSide == SIDE.MID)
            {
                mSide = SIDE.LEFT;
                NewXPos = -XValue;
                mAnimator.CrossFadeInFixedTime("Idle_B", 0.1f);
                DodgeDelay = delayConstant;
            }
            else if (mSide == SIDE.RIGHT)
            {
                mSide = SIDE.MID;
                NewXPos = 0f;
                mAnimator.CrossFadeInFixedTime("Idle_B", 0.1f);
                DodgeDelay = delayConstant;
            }
        }
        else if (playerControls.Player.Right.triggered || swipeManager.swipeDirection == Swipe.Right)
        {
            if (mSide == SIDE.MID)
            {
                mSide = SIDE.RIGHT;
                NewXPos = XValue;
                mAnimator.CrossFadeInFixedTime("Idle_C", 0.1f);
                DodgeDelay = delayConstant;
            }
            else if (mSide == SIDE.LEFT)
            {
                mSide = SIDE.MID;
                NewXPos = 0f;
                mAnimator.CrossFadeInFixedTime("Idle_C", 0.1f);
                DodgeDelay = delayConstant;
            }
        }

        // 점프 및 다이빙 로직

        // 바다에 가까우면 y=0 및 상태 초기화 
        if (Mathf.Abs(transform.position.y) < 0.1f)
        {
            if (InFall || InRecovery)
            {
                Y = 0;
                InFall = false;
                InRecovery = false;
                mAnimator.CrossFadeInFixedTime("Roll", 0.1f);
            }
        }
        else
        {
            // 점프 상태에서 움직임 업데이트
            if (InJump || InFall)
            {
                // 만약 바다 위 (y>0)에 있으면 Y (y방향 속도) 감소
                if (transform.position.y > 0.01f)
                {
                    Y -= JumpPower * 2.5f * Time.fixedDeltaTime;
                }
                // 만약 바다 아래 (y<0)에 있으면 다이빙 중 점프이므로 Y 속도 증가
                else
                {
                    Y += JumpPower * Time.fixedDeltaTime;
                }
            }
            // 다이빙 상태에서 움직임 업데이트
            else if (InRoll || InRecovery)
            {
                if (transform.position.y < 0.01f)
                {
                    Y += JumpPower * 2.5f * Time.fixedDeltaTime;
                }
                else
                {
                    Y -= JumpPower * Time.fixedDeltaTime;
                }
            }
        }
        // 점프에서 떨어짐으로 전환
        if (InJump && mChar.velocity.y < -0.1f)
        {
            InJump = false;
            InFall = true;
        }
        // 다이빙에서 복귀상태로 전환
        if (InRoll && mChar.velocity.y > 0.1f)
        {
            InRoll = false;
            InRecovery = true;
        }
        // 현재 점프 중이거나 떨어지는 중이 아니면 입력에 따라 점프 실행
        if (!(InJump || InFall || InRoll) && (playerControls.Player.Jump.triggered || swipeManager.swipeDirection == Swipe.Up))
        {
            Y = JumpPower;
            InJump = true;
            InRoll = false;
            InRecovery = false;
            mAnimator.CrossFadeInFixedTime("Fly", 0.1f);
        }
        // 현재 다이빙 중이거나 복귀 중이 아니면 입력에 따라 다이빙 실행
        else if (!(InRoll || InRecovery || InJump) && (playerControls.Player.Roll.triggered || swipeManager.swipeDirection == Swipe.Down))
        {
            Y = -JumpPower;
            InRoll = true;
            InJump = false;
            InFall = false;
            mAnimator.CrossFadeInFixedTime("Swim", 0.1f);
        }

        // 캐릭터 NewXPos 및 Y에 따른 부드러운 이동
        Vector3 movement = new Vector3(X - transform.position.x, Y, 0f);
        X = Mathf.Lerp(X, NewXPos, Time.fixedDeltaTime * DodgeSpeed);
        mChar.Move(movement);
    }

    // private void Jump()
    // {
    //     if (mChar.isGrounded)
    //     {
    //         if (InFall)
    //         {
    //             InJump = false;
    //             InFall = false;
    //         }
    //         if (playerControls.Player.Jump.triggered || swipeManager.swipeDirection == Swipe.Up)
    //         {
    //             Y = JumpPower;
    //             InJump = true;
    //         }
    //     }
    //     else
    //     {
    //         Y -= JumpPower * 2 * Time.fixedDeltaTime;
    //         if (mChar.velocity.y < -0.1f)
    //         {
    //             InFall = true;
    //         }
    //     }
    // }

    // private void Roll()
    // {
    //     RollCounter -= Time.fixedDeltaTime;
    //     if (RollCounter <= 0f)
    //     {
    //         RollCounter = 0f;
    //         mChar.height = ColHeight;
    //         mChar.center = new Vector3(0, ColCenterY, 0);
    //         InRoll = false;
    //     }
    //     if (playerControls.Player.Roll.triggered || swipeManager.swipeDirection == Swipe.Down)
    //     {
    //         RollCounter = 0.2f;
    //         Y -= 0.015f;
    //         mChar.height = ColHeight / 2f;
    //         mChar.center = new Vector3(0, ColCenterY / 2f, 0);
    //         InRoll = true;
    //         InJump = false;
    //     }
    // }
}
