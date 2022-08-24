using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class MovingObjectGenerator : MonoBehaviour
{
    public static MovingObjectGenerator instance {get; private set;}

    // ���� ��ġ
    [SerializeField] float[] XPoses;
    [SerializeField] float ZPos;

    // �������� �����Ǵ� ���� ����
    [SerializeField] int coinMinCount;
    [SerializeField] int coinMaxCount;

    // ���� ����
    [SerializeField] float obsMinInterval;
    [SerializeField] float obsMaxInterval;
    [SerializeField] float coinInterval;

    // ���� Ȯ��
    [SerializeField] float obsSpawnProbability;
    [SerializeField] float coinSpawnProbability;
    [SerializeField] float coinLowHeightProbability; // ������ �ϴܿ� ������ Ȯ��
    [SerializeField] float coinHighHeightProbability; // ������ ��ܿ� ������ Ȯ��

    [SerializeField] int coinZeroHeightIdx; // Coin.yPoses���� ���� 0�� ���� index

    int obsTypeCount;
    int lineCount;
    int[] coinHeightIdx; // �� ���� �� ���� ��ġ
    bool[,] isOccupied; // �� ���� �� �ڸ� ���� ����

    const int SPACE_CNT = 3; // ���, �ߴ�, �ϴ�

    void Awake() {
        // �̱���
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }

        obsTypeCount = Enum.GetNames(typeof(MovingObject.ObjType)).Length - 1;
        lineCount = XPoses.Length;
        coinHeightIdx = Enumerable.Repeat<int>(coinZeroHeightIdx, lineCount).ToArray<int>(); 
        isOccupied = new bool[lineCount, SPACE_CNT];
    }

    void Start() {
        StartCoroutine(GenerateObjs());
    }

    IEnumerator GenerateObjs() {
        var wait = new WaitForSeconds(coinInterval);

        float curInterval = 0f;
        float obsInterval = UnityEngine.Random.Range(obsMinInterval, obsMaxInterval);
        int[] coinCount = new int[lineCount]; // �� ���κ� �� �����ؾ� �� ���� ����

        // coinInterval���� ���� ���� ���� ����, obsInterval���� ��ֹ� ���� ���� ����
        while (true) {
            if (curInterval > obsInterval) {
                GeneratedObses();
                curInterval = 0f;
                obsInterval = UnityEngine.Random.Range(obsMinInterval, obsMaxInterval);
            }
            
            GenerateCoins(coinCount);

            curInterval += coinInterval;
            yield return wait;
        }
    }

    // ��ֹ� ����
    void GeneratedObses() {
        for (int i = 0; i < lineCount; i++) {
            for (int j = 0; j < SPACE_CNT; j++) {
                isOccupied[i, j] = false;
            }
        }

        MovingObject obj = null;
        for (int i = 0; i < lineCount; i++) {
            // ���� Ȯ���� ��ֹ� ���� �ȵ�
            float randomNum = UnityEngine.Random.Range(0f, 1f);
            if (randomNum > obsSpawnProbability) continue;
                
            // ��ֹ� ����
            MovingObject.ObjType type = (MovingObject.ObjType)UnityEngine.Random.Range(0, obsTypeCount);
            obj = MovingObjectPool.instance.GetObj(type);
            Obstacle obs = obj as Obstacle;
            Debug.Assert(obs != null, "SpawningMovingObjectPool���� ������ ������Ʈ�� ��ֹ��� �ƴմϴ�");

            // ��ֹ� ��ġ ����
            obs.transform.position = new Vector3(XPoses[i], obs.yPos, ZPos);
            if (obs.occupyLow) isOccupied[i, 0] = true;
            if (obs.occupyMid) isOccupied[i, 1] = true;
            if (obs.occupyHigh) isOccupied[i, 2] = true;
        }

        // ��� ������ ��ֹ��� �����ִٸ� ������ ������ ��ֹ��� ����
        if (!IsValid()) {
            MovingObjectPool.instance.ReturnObj(obj);
            for (int i = 0; i < SPACE_CNT; i++) {
                isOccupied[lineCount - 1, i] = false;
            }
        }
    }

    // ��� ������ ��ֹ��� �����ִٸ� return false
    bool IsValid() {
        for (int i = 0; i < lineCount; i++) {
            for (int j = 0; j < SPACE_CNT; j++) {
                if (!isOccupied[i, j]) {
                    return true;
                }
            }
        }
        return false;
    }

    void GenerateCoins(int[] coinCount) {
        for (int i = 0; i < lineCount; i++) {
            // �̹� �Ͽ� ������ �������� �ʴ´ٸ�
            if (coinCount[i] <= 0) {
                // ���� Ȯ���� ���� �Ͽ� ���� ����
                float randomNum = UnityEngine.Random.Range(0f, 1f);
                if (randomNum < coinSpawnProbability) {
                    coinCount[i] = UnityEngine.Random.Range(coinMinCount, coinMaxCount);
                }
            }

            // �̹� �Ͽ� ������ ��ܿ� �����ؾ� �Ѵٸ�
            else if (coinHeightIdx[i] > coinZeroHeightIdx) {
                // ����� ��ֹ��� ���θ��� �ִٸ� ���� ���� �ߴ�
                if (isOccupied[i, 2]) {
                    StopCoin(i, coinCount);
                    continue;
                }
                // �׷��� ������ ��ܿ� ���� ����
                GenerateCoin(i, coinHeightIdx[i]++);
            }

            // �̹� �Ͽ� ������ �ϴܿ� �����ؾ� �Ѵٸ�
            else if (coinHeightIdx[i] < coinZeroHeightIdx) {
                // �ϴ��� ��ֹ��� ���θ��� �ִٸ� ���� ���� �ߴ�
                if (isOccupied[i, 0]) {
                    StopCoin(i, coinCount);
                    continue;
                }

                // �׷��� ������ �ϴܿ� ���� ����
                GenerateCoin(i, coinHeightIdx[i]--);
            }

            // �̹� �Ͽ� ������ �ߴܿ� �����ؾ� �Ѵٸ�
            else {
                // �ߴ��� ��ֹ��� ���θ��� �ִٸ� ���� ���� �ߴ�
                if (isOccupied[i, 1]) {
                    StopCoin(i, coinCount);
                    continue;
                }

                // �׷��� ������ �ߴܿ� ���� ����
                GenerateCoin(i, coinHeightIdx[i]);

                float randomNum = UnityEngine.Random.Range(0f, 1f);
                // ���� Ȯ���� ���� �Ͽ� ������ ��ܿ� ����
                if (randomNum < coinHighHeightProbability) {
                    coinHeightIdx[i]++;
                }
                // ���� Ȯ���� ���� �Ͽ� ������ �ϴܿ� ����
                else if (randomNum > 1 - coinLowHeightProbability) {
                    coinHeightIdx[i]--;
                }
            }
            coinCount[i]--;
        }
    }

    void GenerateCoin(int lineNum, int yPosIdx) {
        // ���� ����
        MovingObject obj = MovingObjectPool.instance.GetObj(Obstacle.ObjType.coin);
        Coin coin = obj as Coin;
        Debug.Assert(coin != null, "SpawningMovingObjectPool���� ������ ������Ʈ�� ������ �ƴմϴ�");
        
        // ��ġ ����
        coin.transform.position = new Vector3(XPoses[lineNum], coin.yPoses[yPosIdx], ZPos);
        if (coinHeightIdx[lineNum] >= coin.yPoses.Length || coinHeightIdx[lineNum] < 0) {
            coinHeightIdx[lineNum] = coinZeroHeightIdx;
        }
    }

    void StopCoin(int lineNum, int[] coinCount) {
        coinCount[lineNum] = 0;
        coinHeightIdx[lineNum] = coinZeroHeightIdx;
    }
}
