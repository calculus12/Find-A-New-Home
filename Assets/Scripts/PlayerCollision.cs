using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 장애물 및 코인과의 충돌을 감지하고 해당 로직을 처리하는 컴포넌트
/// </summary>
public class PlayerCollision : MonoBehaviour
{
    [SerializeField] ScoreManager scoreManager;

    Movement moveScript;
    [SerializeField] GameObject gameoverScreen;

    private void Awake()
    {
        moveScript = GetComponent<Movement>();
    }

    private void OnTriggerEnter(Collider other){
        //player die function
        if(other.tag == "Obstacle"){
            // 게임오버 처리
            GameManager.Instance.SetState(GameState.gameover);
            StartCoroutine(PlayerDieMovement());

            // 현재 점수, 획득 코인 결과 설정 및 UI 활성화
            gameoverScreen.GetComponent<GameoverScreen>().SetResult(scoreManager.GetScore, scoreManager.GetEarnedCoin);
            GameManager.Instance.GameoverAndSave(scoreManager.GetScore, scoreManager.GetEarnedCoin);
            gameoverScreen.SetActive(true);

            Time.timeScale = 0f; // DEBUGGING CODE
            // FADE IN 처리로 게임오버 UI가 나와야 함
        }
        else if (other.tag == "Coin") {
            MovingObjectPool.instance.ReturnObj(other.GetComponent<Coin>());
            scoreManager.IncreaseCoin();
        }
    }
    
    // 플레이어 충돌 후 ragdoll 효과
    IEnumerator PlayerDieMovement()
    {
        moveScript.enabled = false;

        // 플레이어가 앞쪽으로 90도 엎어짐 
        var startAngle = transform.rotation;
        var destAngle = Quaternion.Euler(transform.eulerAngles + Vector3.right * 90f);
        for (float t = 0f; t < 1f; t += Time.deltaTime / 0.1f)
        {
            transform.rotation = Quaternion.Slerp(startAngle, destAngle, t);
            yield return null;
        }

        // 뒤로 10만큼 이동
        var startPos = transform.position;
        while (Vector3.Distance(startPos, transform.position) < 10f)
        {
            transform.Translate(Vector3.down * 20f * Time.deltaTime, Space.Self);
            yield return null;
        }
    }
}
