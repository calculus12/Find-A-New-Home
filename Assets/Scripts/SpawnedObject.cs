using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedObject : MonoBehaviour
{
    public enum ObjType {box, can, reef, coin};
    public ObjType type;
    public float speed;
    public int count; // 초기 생성 개수
    public int additionalCount; // 부족할 시 추가로 생성할 개수
    public float endZPos; // 소멸될 때의 z좌표

    void FixedUpdate() {
        // 이동
        transform.Translate(Vector3.back * speed, Space.World);
        
        // 소멸
        if (transform.position.z <= endZPos) {
            SpawnedObjectPool.instance.ReturnObs(this);
        }
    }
}
