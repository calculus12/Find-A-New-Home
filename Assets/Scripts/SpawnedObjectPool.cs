using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnedObjectPool : MonoBehaviour {
    public static SpawnedObjectPool instance {get; private set;}

    List<List<SpawnedObject>> objs;
    List<List<Queue<SpawnedObject>>> objQueues;
    Dictionary<SpawnedObject, Queue<SpawnedObject>> getQueueByObj;

    void Awake() {
        // �̱���
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }

        objs = new List<List<SpawnedObject>>();
        objQueues = new List<List<Queue<SpawnedObject>>>();
        int objTypeCount = Enum.GetNames(typeof(SpawnedObject.ObjType)).Length;
        for (int i = 0; i < objTypeCount; i++) {
            objs.Add(new List<SpawnedObject>());
            objQueues.Add(new List<Queue<SpawnedObject>>());
        }
        getQueueByObj = new Dictionary<SpawnedObject, Queue<SpawnedObject>>();

        // Resources/SpawnedObjects ������ �ִ� ��� SpawnedObject �ҷ�����
        SpawnedObject[] objArr = Resources.LoadAll<SpawnedObject>("SpawnedObjects");
        foreach (SpawnedObject obj in objArr) {
            objs[(int)obj.type].Add(obj);
            Queue<SpawnedObject> queue = new Queue<SpawnedObject>();
            objQueues[(int)obj.type].Add(queue);
            CreateNewObs(obj, queue, false);
        }
    }

    // �� ������Ʈ �����Ͽ� Pool�� �ֱ�
    void CreateNewObs(SpawnedObject obj, Queue<SpawnedObject> queue, bool isAdditional) {
        for (int i = 0; i < (isAdditional ? obj.additionalCount : obj.count); i++) {
            GameObject newObj = Instantiate(obj.gameObject);
            newObj.SetActive(false);
            SpawnedObject newSpawnedObj = newObj.GetComponent<SpawnedObject>();
            getQueueByObj.Add(newSpawnedObj, queue);
            queue.Enqueue(newSpawnedObj);
        }
    }

    // Pool���� ������Ʈ ������
    public SpawnedObject GetObs(SpawnedObject.ObjType type) {
        // �ش� Ÿ�Կ� �´� ������ ���� ����
        int prefabCount = objs[(int)type].Count;
        int prefabIdx = UnityEngine.Random.Range(0, prefabCount);
        Queue<SpawnedObject> queue = objQueues[(int)type][prefabIdx];

        // Pool�� �����ִ� ������Ʈ�� ���� ��� �߰������� ������Ʈ �����Ͽ� Pool�� �ֱ�
        if (queue.Count == 0) {
            CreateNewObs(objs[(int)type][prefabIdx], queue, true);
        }

        SpawnedObject obj = queue.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    // ������Ʈ�� �ڽ��� ���� Pool�� �ֱ�
    public void ReturnObs(SpawnedObject obj) {
        obj.gameObject.SetActive(false);
        Queue<SpawnedObject> queue = getQueueByObj[obj];
        queue.Enqueue(obj);
    }
}
