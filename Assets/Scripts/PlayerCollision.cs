using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ֹ� �� ���ΰ��� �浹�� �����ϰ� �ش� ������ ó���ϴ� ������Ʈ
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
            // ���ӿ��� ó��
            GameManager.Instance.SetState(GameState.gameover);
            StartCoroutine(PlayerDieMovement());

            // ���� ����, ȹ�� ���� ��� ���� �� UI Ȱ��ȭ
            gameoverScreen.GetComponent<GameoverScreen>().SetResult(scoreManager.GetScore, scoreManager.GetEarnedCoin);
            GameManager.Instance.GameoverAndSave(scoreManager.GetScore, scoreManager.GetEarnedCoin);
            gameoverScreen.SetActive(true);

            Time.timeScale = 0f; // DEBUGGING CODE
            // FADE IN ó���� ���ӿ��� UI�� ���;� ��
        }
        else if (other.tag == "Coin") {
            MovingObjectPool.instance.ReturnObj(other.GetComponent<Coin>());
            scoreManager.IncreaseCoin();
        }
    }
    
    // �÷��̾� �浹 �� ragdoll ȿ��
    IEnumerator PlayerDieMovement()
    {
        moveScript.enabled = false;

        // �÷��̾ �������� 90�� ������ 
        var startAngle = transform.rotation;
        var destAngle = Quaternion.Euler(transform.eulerAngles + Vector3.right * 90f);
        for (float t = 0f; t < 1f; t += Time.deltaTime / 0.1f)
        {
            transform.rotation = Quaternion.Slerp(startAngle, destAngle, t);
            yield return null;
        }

        // �ڷ� 10��ŭ �̵�
        var startPos = transform.position;
        while (Vector3.Distance(startPos, transform.position) < 10f)
        {
            transform.Translate(Vector3.down * 20f * Time.deltaTime, Space.Self);
            yield return null;
        }
    }
}
