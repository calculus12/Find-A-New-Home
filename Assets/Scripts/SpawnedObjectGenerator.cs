using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class SpawnedObjectGenerator : MonoBehaviour
{
    public static SpawnedObjectGenerator instance {get; private set;}

    // ���� ��ġ
    [SerializeField] float[] spawnedXPoses;
    [SerializeField] float spawnedZPos;

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
    [SerializeField] float coinLowProbability; // ������ �ϴܿ� ������ Ȯ��
    [SerializeField] float coinHighProbability; // ������ ��ܿ� ������ Ȯ��

    [SerializeField] int coinZeroHeightIdx;

    int obsTypeCount;
    int lineCount;
    int[] coinHeightIdx; // �� ���� �� ���� ��ġ
    bool[,] isOccupied; // �� ���� �� �ڸ� ���� ����

    void Awake() {
        // �̱���
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }

        obsTypeCount = Enum.GetNames(typeof(SpawnedObject.ObjType)).Length - 1;
        lineCount = spawnedXPoses.Length;
        coinHeightIdx = Enumerable.Repeat<int>(coinZeroHeightIdx, lineCount).ToArray<int>(); 
        isOccupied = new bool[lineCount, 3];
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
            for (int j = 0; j < 3; j++) {
                isOccupied[i, j] = false;
            }
        }

        SpawnedObject obj = null;
        for (int i = 0; i < lineCount; i++) {
            // ��ֹ��� ���� Ȯ���� ����
            float randomNum = UnityEngine.Random.Range(0f, 1f);
            if (randomNum > obsSpawnProbability) continue;
                
            // ��ֹ� ����
            SpawnedObject.ObjType type = (SpawnedObject.ObjType)UnityEngine.Random.Range(0, obsTypeCount);
            obj = SpawnedObjectPool.instance.GetObs(type);
            Obstacle obs = obj as Obstacle;
            Debug.Assert(obs != null, "SpawningObjectPool���� ������ ������Ʈ�� ��ֹ��� �ƴմϴ�");

            // ��ֹ� ��ġ ����
            obs.transform.position = new Vector3(spawnedXPoses[i], obs.yPos, spawnedZPos);
            if (obs.occupyLow) isOccupied[i, 0] = true;
            if (obs.occupyMid) isOccupied[i, 1] = true;
            if (obs.occupyHigh) isOccupied[i, 2] = true;
        }

        if (!IsValid()) {
            // ������ ������ ��ֹ��� ����
            SpawnedObjectPool.instance.ReturnObs(obj);
            for (int i = 0; i < 3; i++) {
                isOccupied[lineCount - 1, i] = false;
            }
        }
    }

    // ��� ������ ���, �ߴ�, �ϴ��� ��� ��ֹ��� �����ִٸ� return false
    bool IsValid() {
        for (int i = 0; i < lineCount; i++) {
            for (int j = 0; j < 3; j++) {
                if (!isOccupied[i, j]) {
                    return true;
                }
            }
        }
        return false;
    }

    void GenerateCoins(int[] coinCount) {
        for (int i = 0; i < lineCount; i++) {
            // ������ �����ؾ� �� ��
            if (coinCount[i] > 0) {
                // ������ ��ܿ� �����ؾ� �Ѵٸ�
                if (coinHeightIdx[i] > coinZeroHeightIdx) {
                    // ����� ��ֹ��� ���θ��� �ִٸ� ���� ���� �ߴ�
                    if (isOccupied[i, 2]) {
                        StopCoin(i, coinCount);
                        continue;
                    }
                    // �׷��� ������ ��ܿ� ���� ����
                    GenerateCoin(i, coinHeightIdx[i]++);
                }

                // ������ �ϴܿ� �����ؾ� �Ѵٸ�
                else if (coinHeightIdx[i] < coinZeroHeightIdx) {
                    // �ϴ��� ��ֹ��� ���θ��� �ִٸ� ���� ���� �ߴ�
                    if (isOccupied[i, 0]) {
                        StopCoin(i, coinCount);
                        continue;
                    }

                    // �׷��� ������ �ϴܿ� ���� ����
                    GenerateCoin(i, coinHeightIdx[i]--);
                }

                // ������ �ߴܿ� �����ؾ� �Ѵٸ�
                else {
                    // �ߴ��� ��ֹ��� ���θ��� �ִٸ� ���� ���� �ߴ�
                    if (isOccupied[i, 1]) {
                        StopCoin(i, coinCount);
                        continue;
                    }

                    // �׷��� ������ �ߴܿ� ���� ����
                    GenerateCoin(i, coinHeightIdx[i]);

                    float randomNum = UnityEngine.Random.Range(0f, 1f);
                    // ���� Ȯ���� ���� ������ ��ܿ� ����
                    if (randomNum < coinHighProbability) {
                        coinHeightIdx[i]++;
                    }
                    // ���� Ȯ���� ���� ������ �ϴܿ� ����
                    else if (randomNum > 1 - coinLowProbability) {
                        coinHeightIdx[i]--;
                    }
                }
                coinCount[i]--;
            }
            // ���� �� �̾Ƶ��� ��
            else {
                float randomNum = UnityEngine.Random.Range(0f, 1f);
                // ���� Ȯ���� ���� ����
                if (randomNum < coinSpawnProbability) {
                    coinCount[i] = UnityEngine.Random.Range(coinMinCount, coinMaxCount);
                }
            }
        }
    }

    void GenerateCoin(int lineNum, int yPosIdx) {
        SpawnedObject obj = SpawnedObjectPool.instance.GetObs(Obstacle.ObjType.coin);
        Coin coin = obj as Coin;
        Debug.Assert(coin != null, "SpawningObjectPool���� ������ ������Ʈ�� ������ �ƴմϴ�");
        coin.transform.position = new Vector3(spawnedXPoses[lineNum], coin.yPoses[yPosIdx], spawnedZPos);

        if (coinHeightIdx[lineNum] >= coin.yPoses.Length || coinHeightIdx[lineNum] < 0) {
            coinHeightIdx[lineNum] = coinZeroHeightIdx;
        }
    }

    void StopCoin(int lineNum, int[] coinCount) {
        coinCount[lineNum] = 0;
        coinHeightIdx[lineNum] = coinZeroHeightIdx;
    }
}
