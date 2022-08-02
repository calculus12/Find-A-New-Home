using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour {
    public static ObstacleGenerator instance {get; private set;}

    public enum ObsType {reef, box, can};

    [SerializeField] GameObject reefPrefab;
    [SerializeField] GameObject boxPrefab;
    [SerializeField] GameObject canPrefab;

    // 초기에 생성할 장애물의 개수
    [SerializeField] int reefCount;
    [SerializeField] int boxCount;
    [SerializeField] int canCount;

    // 장애물이 모자랄 때 추가로 생성할 장애물의 개수
    [SerializeField] int additionalObsCount;

    Queue<GameObject> reefQueue;
    Queue<GameObject> boxQueue;
    Queue<GameObject> canQueue;

    Dictionary<GameObject, Queue<GameObject>> objToQueue;

    void Awake() {
        // 싱글톤
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }

        reefQueue = new Queue<GameObject>();
        boxQueue = new Queue<GameObject>();
        canQueue = new Queue<GameObject>();

        objToQueue = new Dictionary<GameObject, Queue<GameObject>>();
    }

    void Start() {
        CreateNewObs(ObsType.reef, reefCount);
        CreateNewObs(ObsType.box, boxCount);
        CreateNewObs(ObsType.can, canCount);
    }

    void CreateNewObs(ObsType type, int count) {
        switch (type) {
            case ObsType.reef:
                for (int i = 0; i < count; i++) {
                    GameObject newReef = Instantiate(reefPrefab);
                    newReef.SetActive(false);
                    reefQueue.Enqueue(newReef);
                    objToQueue.Add(newReef, reefQueue);
                }
                break;
            case ObsType.box:
                for (int i = 0; i < count; i++) {
                    GameObject newBox = Instantiate(boxPrefab);
                    newBox.SetActive(false);
                    boxQueue.Enqueue(newBox);
                    objToQueue.Add(newBox, boxQueue);
                }
                break;
            case ObsType.can:
                for (int i = 0; i < count; i++) {
                    GameObject newCan = Instantiate(canPrefab);
                    newCan.SetActive(false);
                    canQueue.Enqueue(newCan);
                    objToQueue.Add(newCan, canQueue);
                }
                break;
        }
    }

    // 오브젝트 풀에서 오브젝트 꺼내기
    public GameObject GetObs(ObsType type) {
        Queue<GameObject> queue = null;
        switch (type) {
            case ObsType.reef:
                queue = reefQueue;
                break;
            case ObsType.box:
                queue = boxQueue;
                break;
            case ObsType.can:
                queue = canQueue;
                break;
        }

        // 오브젝트 풀에 남아있는 오브젝트가 없을 경우 추가적으로 오브젝트 생성
        if (queue.Count == 0) {
            CreateNewObs(type, additionalObsCount);
        }

        GameObject obs = queue.Dequeue();
        obs.SetActive(true);
        return obs;
    }

    // 오브젝트를 자신이 속한 오브젝트 풀에 반납
    public void ReturnObs(GameObject prefab) {
        prefab.SetActive(false);
        Queue<GameObject> queue = objToQueue[prefab];
        queue.Enqueue(prefab);
    }
}
