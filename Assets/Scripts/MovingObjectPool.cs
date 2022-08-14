using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovingObjectPool : MonoBehaviour {
    public static MovingObjectPool instance {get; private set;}

    List<List<MovingObject>> objs;
    List<List<Queue<MovingObject>>> objQueues;
    Dictionary<MovingObject, Queue<MovingObject>> getQueueByObj;

    void Awake() {
        // 싱글톤
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }

        // objs, objQueues 구조
        // [0][]: ObjType1 --- [0][0]: prefab1, [0][1]: prefab2, [0][2]: prefab3
        // [1][]: ObjType2 --- [1][0]: prefab1, [1][1]: prefab2, [1][2]: prefab3
        // [2][]: ObjType3 --- [2][0]: prefab1, [2][1]: prefab2, [2][2]: prefab3
        objs = new List<List<MovingObject>>();
        objQueues = new List<List<Queue<MovingObject>>>();
        int objTypeCount = Enum.GetNames(typeof(MovingObject.ObjType)).Length;
        for (int i = 0; i < objTypeCount; i++) {
            objs.Add(new List<MovingObject>());
            objQueues.Add(new List<Queue<MovingObject>>());
        }
        getQueueByObj = new Dictionary<MovingObject, Queue<MovingObject>>();

        // Resources/MovingObjects에 있는 모든 MovingObject 불러와서 오브젝트 풀 생성
        MovingObject[] objArr = Resources.LoadAll<MovingObject>("MovingObjects");
        foreach (MovingObject obj in objArr) {
            objs[(int)obj.type].Add(obj);
            Queue<MovingObject> queue = new Queue<MovingObject>();
            objQueues[(int)obj.type].Add(queue);
            CreateNewObj(obj, queue, false);
        }
    }

    void CreateNewObj(MovingObject obj, Queue<MovingObject> queue, bool isAdditional) {
        // 새 오브젝트를 지정된 수 만큼 생성하여 Pool에 넣기
        for (int i = 0; i < (isAdditional ? obj.additionalCount : obj.count); i++) {
            GameObject newObj = Instantiate(obj.gameObject);
            newObj.SetActive(false);
            MovingObject newMovingObj = newObj.GetComponent<MovingObject>();
            getQueueByObj.Add(newMovingObj, queue);
            queue.Enqueue(newMovingObj);
        }
    }

    public MovingObject GetObj(MovingObject.ObjType type) {
        // 해당 타입에 맞는 프리팹 랜덤 선택
        int prefabCount = objs[(int)type].Count;
        int prefabIdx = UnityEngine.Random.Range(0, prefabCount);
        Queue<MovingObject> queue = objQueues[(int)type][prefabIdx];

        // 해당 프리팹이 Pool에 남아있지 않다면 추가적으로 오브젝트 생성하여 Pool에 넣기
        if (queue.Count == 0) {
            CreateNewObj(objs[(int)type][prefabIdx], queue, true);
        }

        // Pool에서 오브젝트 꺼내기
        MovingObject obj = queue.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void ReturnObj(MovingObject obj) {
        // 오브젝트를 자신이 속한 Pool에 넣기
        obj.gameObject.SetActive(false);
        Queue<MovingObject> queue = getQueueByObj[obj];
        queue.Enqueue(obj);
    }
}
