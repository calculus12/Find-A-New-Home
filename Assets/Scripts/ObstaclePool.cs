using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePool : MonoBehaviour {
    public static ObstaclePool instance {get; private set;}

    Obstacle[] obses;
    Queue<GameObject>[] obsQueues;
    Dictionary<GameObject, Queue<GameObject>> getQueueByObj;

    void Awake() {
        // �̱���
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }

        // Assets/Resources/Obstacles �������� ��� Obstacle �ҷ�����
        obses = Resources.LoadAll<Obstacle>("Obstacles");
        for (int i = 0; i < obses.Length; i++) {
            bool isEqual = obses[i].name.ToLower() == obses[i].type.ToString();
            bool isSorted = (int)obses[i].type == i;
            Debug.Assert(isEqual, "Obstacle�� Ÿ���� Obstacle�� �̸��� ��ġ���� ����");

            // �������� ���ĵǾ� ���� ������ GetObs()���� ObsType���� Obs�� ã�� �� ����
            Debug.Assert(!isEqual || isSorted, "Obstacle.ObsType�� ������������ ���ĵǾ� ���� ����");
        }

        // Obstacle�� ������ Pool
        obsQueues = new Queue<GameObject>[obses.Length];
        for (int i = 0; i < obsQueues.Length; i++) {
            obsQueues[i] = new Queue<GameObject>();
        }

        // GameObject Ÿ���� Obstacle�� �ڽ��� ���� Pool�� ã�ư��� ���� Dictionary
        getQueueByObj = new Dictionary<GameObject, Queue<GameObject>>();
        
        // ��� Pool���� �ش��ϴ� Obstacle ä���ֱ� 
        foreach (Obstacle obs in obses) {
            CreateNewObs(obs, false);
        }
    }

    // �� ������Ʈ �����Ͽ� Pool�� �ֱ�
    void CreateNewObs(Obstacle obs, bool isAdditional) {
        Queue<GameObject> queue = obsQueues[(int)obs.type];

        for (int i = 0; i < (isAdditional ? obs.additionalCount : obs.count); i++) {
            GameObject newObs = Instantiate(obs.prefab);
            newObs.SetActive(false);
            queue.Enqueue(newObs);
            getQueueByObj.Add(newObs, queue);
        }
    }

    // Pool���� ������Ʈ ������
    public GameObject GetObs(Obstacle.ObsType type) {
        Queue<GameObject> queue = obsQueues[(int)type];

        // Pool�� �����ִ� ������Ʈ�� ���� ��� �߰������� ������Ʈ �����Ͽ� Pool�� �ֱ�
        if (queue.Count == 0) {
            CreateNewObs(obses[(int)type], true);
        }

        GameObject obs = queue.Dequeue();
        obs.SetActive(true);
        return obs;
    }

    // ������Ʈ�� �ڽ��� ���� Pool�� �ֱ�
    public void ReturnObs(GameObject obs) {
        obs.SetActive(false);
        Queue<GameObject> queue = getQueueByObj[obs];
        queue.Enqueue(obs);
    }
}
