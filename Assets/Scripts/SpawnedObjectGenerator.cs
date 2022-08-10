using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class SpawnedObjectGenerator : MonoBehaviour
{
    public static SpawnedObjectGenerator instance {get; private set;}

    // 생성 위치
    [SerializeField] float[] spawnedXPoses;
    [SerializeField] float spawnedZPos;

    // 연속으로 생성되는 코인 개수
    [SerializeField] int coinMinCount;
    [SerializeField] int coinMaxCount;

    // 생성 간격
    [SerializeField] float obsMinInterval;
    [SerializeField] float obsMaxInterval;
    [SerializeField] float coinInterval;

    // 생성 확률
    [SerializeField] float obsSpawnProbability;
    [SerializeField] float coinSpawnProbability;
    [SerializeField] float coinLowProbability; // 코인이 하단에 생성될 확률
    [SerializeField] float coinHighProbability; // 코인이 상단에 생성될 확률

    [SerializeField] int coinZeroHeightIdx;

    int obsTypeCount;
    int lineCount;
    int[] coinHeightIdx; // 각 라인 별 코인 위치
    bool[,] isOccupied; // 각 라인 별 자리 차지 여부

    void Awake() {
        // 싱글톤
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

        int[] coinCount = new int[lineCount]; // 각 라인별 더 생성해야 할 코인 개수

        // coinInterval마다 코인 생성 로직 실행, obsInterval마다 장애물 생성 로직 실행
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

    // 장애물 생성
    void GeneratedObses() {
        for (int i = 0; i < lineCount; i++) {
            for (int j = 0; j < 3; j++) {
                isOccupied[i, j] = false;
            }
        }

        SpawnedObject obj = null;
        for (int i = 0; i < lineCount; i++) {
            // 장애물은 일정 확률로 생성
            float randomNum = UnityEngine.Random.Range(0f, 1f);
            if (randomNum > obsSpawnProbability) continue;
                
            // 장애물 생성
            SpawnedObject.ObjType type = (SpawnedObject.ObjType)UnityEngine.Random.Range(0, obsTypeCount);
            obj = SpawnedObjectPool.instance.GetObs(type);
            Obstacle obs = obj as Obstacle;
            Debug.Assert(obs != null, "SpawningObjectPool에서 가져온 오브젝트가 장애물이 아닙니다");

            // 장애물 위치 설정
            obs.transform.position = new Vector3(spawnedXPoses[i], obs.yPos, spawnedZPos);
            if (obs.occupyLow) isOccupied[i, 0] = true;
            if (obs.occupyMid) isOccupied[i, 1] = true;
            if (obs.occupyHigh) isOccupied[i, 2] = true;
        }

        if (!IsValid()) {
            // 마지막 라인의 장애물을 없앰
            SpawnedObjectPool.instance.ReturnObs(obj);
            for (int i = 0; i < 3; i++) {
                isOccupied[lineCount - 1, i] = false;
            }
        }
    }

    // 모든 라인의 상단, 중단, 하단이 모두 장애물로 막혀있다면 return false
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
            // 코인을 생성해야 할 때
            if (coinCount[i] > 0) {
                // 코인을 상단에 생성해야 한다면
                if (coinHeightIdx[i] > coinZeroHeightIdx) {
                    // 상단을 장애물이 가로막고 있다면 코인 생성 중단
                    if (isOccupied[i, 2]) {
                        StopCoin(i, coinCount);
                        continue;
                    }
                    // 그렇지 않으면 상단에 코인 생성
                    GenerateCoin(i, coinHeightIdx[i]++);
                }

                // 코인을 하단에 생성해야 한다면
                else if (coinHeightIdx[i] < coinZeroHeightIdx) {
                    // 하단을 장애물이 가로막고 있다면 코인 생성 중단
                    if (isOccupied[i, 0]) {
                        StopCoin(i, coinCount);
                        continue;
                    }

                    // 그렇지 않으면 하단에 코인 생성
                    GenerateCoin(i, coinHeightIdx[i]--);
                }

                // 코인을 중단에 생성해야 한다면
                else {
                    // 중단을 장애물이 가로막고 있다면 코인 생성 중단
                    if (isOccupied[i, 1]) {
                        StopCoin(i, coinCount);
                        continue;
                    }

                    // 그렇지 않으면 중단에 코인 생성
                    GenerateCoin(i, coinHeightIdx[i]);

                    float randomNum = UnityEngine.Random.Range(0f, 1f);
                    // 일정 확률로 다음 코인을 상단에 생성
                    if (randomNum < coinHighProbability) {
                        coinHeightIdx[i]++;
                    }
                    // 일정 확률로 다음 코인을 하단에 생성
                    else if (randomNum > 1 - coinLowProbability) {
                        coinHeightIdx[i]--;
                    }
                }
                coinCount[i]--;
            }
            // 코인 안 뽑아도될 때
            else {
                float randomNum = UnityEngine.Random.Range(0f, 1f);
                // 일정 확률로 코인 생성
                if (randomNum < coinSpawnProbability) {
                    coinCount[i] = UnityEngine.Random.Range(coinMinCount, coinMaxCount);
                }
            }
        }
    }

    void GenerateCoin(int lineNum, int yPosIdx) {
        SpawnedObject obj = SpawnedObjectPool.instance.GetObs(Obstacle.ObjType.coin);
        Coin coin = obj as Coin;
        Debug.Assert(coin != null, "SpawningObjectPool에서 가져온 오브젝트가 코인이 아닙니다");
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
