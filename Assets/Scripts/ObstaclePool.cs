using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePool : MonoBehaviour {
    public static ObstaclePool instance {get; private set;}

    Obstacle[] obses;
    Queue<GameObject>[] obsQueues;
    Dictionary<GameObject, Queue<GameObject>> getQueueByObj;

    void Awake() {
        // 싱글톤
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }

        // Assets/Resources/Obstacles 폴더에서 모든 Obstacle 불러오기
        obses = Resources.LoadAll<Obstacle>("Obstacles");
        for (int i = 0; i < obses.Length; i++) {
            bool isEqual = obses[i].name.ToLower() == obses[i].type.ToString();
            bool isSorted = (int)obses[i].type == i;
            Debug.Assert(isEqual, "Obstacle의 타입이 Obstacle의 이름과 일치하지 않음");

            // 오름차순 정렬되어 있지 않으면 GetObs()에서 ObsType으로 Obs를 찾을 수 없음
            Debug.Assert(!isEqual || isSorted, "Obstacle.ObsType이 오름차순으로 정렬되어 있지 않음");
        }

        // Obstacle을 보관할 Pool
        obsQueues = new Queue<GameObject>[obses.Length];
        for (int i = 0; i < obsQueues.Length; i++) {
            obsQueues[i] = new Queue<GameObject>();
        }

        // GameObject 타입의 Obstacle이 자신이 속한 Pool을 찾아가기 위한 Dictionary
        getQueueByObj = new Dictionary<GameObject, Queue<GameObject>>();
        
        // 모든 Pool마다 해당하는 Obstacle 채워넣기 
        foreach (Obstacle obs in obses) {
            CreateNewObs(obs, false);
        }
    }

    // 새 오브젝트 생성하여 Pool에 넣기
    void CreateNewObs(Obstacle obs, bool isAdditional) {
        Queue<GameObject> queue = obsQueues[(int)obs.type];

        for (int i = 0; i < (isAdditional ? obs.additionalCount : obs.count); i++) {
            GameObject newObs = Instantiate(obs.prefab);
            newObs.SetActive(false);
            queue.Enqueue(newObs);
            getQueueByObj.Add(newObs, queue);
        }
    }

    // Pool에서 오브젝트 꺼내기
    public GameObject GetObs(Obstacle.ObsType type) {
        Queue<GameObject> queue = obsQueues[(int)type];

        // Pool에 남아있는 오브젝트가 없을 경우 추가적으로 오브젝트 생성하여 Pool에 넣기
        if (queue.Count == 0) {
            CreateNewObs(obses[(int)type], true);
        }

        GameObject obs = queue.Dequeue();
        obs.SetActive(true);
        return obs;
    }

    // 오브젝트를 자신이 속한 Pool에 넣기
    public void ReturnObs(GameObject obs) {
        obs.SetActive(false);
        Queue<GameObject> queue = getQueueByObj[obs];
        queue.Enqueue(obs);
    }
}
