using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] ScoreManager scoreManager;
    
    private void OnTriggerEnter(Collider other){
        //player die function
        if(other.tag == "Obstacle"){
            // 게임오버 처리
            GameManager.Instance.SetState(GameState.gameover);

            //Debug.Log("충돌발생");
        }
        else if (other.tag == "Coin") {
            MovingObjectPool.instance.ReturnObj(other.GetComponent<Coin>());
            scoreManager.IncreaseCoin();
        }
    }
}
