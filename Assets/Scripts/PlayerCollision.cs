using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] ScoreManager scoreManager;
    
    private void OnTriggerEnter(Collider other){
        //player die function
        if(other.tag == "Obstacle"){
            //Debug.Log("�浹�߻�");
        }
        else if (other.tag == "Coin") {
            MovingObjectPool.instance.ReturnObj(other.GetComponent<Coin>());
            scoreManager.IncreaseCoin();
        }
    }
}
