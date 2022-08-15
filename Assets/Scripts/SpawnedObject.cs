using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedObject : MonoBehaviour
{
    public enum ObjType {box, can, reef, coin};
    public ObjType type;
    public float speed;
    public int count; // �ʱ� ���� ����
    public int additionalCount; // ������ �� �߰��� ������ ����
    public float endZPos; // �Ҹ�� ���� z��ǥ

    void FixedUpdate() {
        // �̵�
        transform.Translate(Vector3.back * speed, Space.World);
        
        // �Ҹ�
        if (transform.position.z <= endZPos) {
            SpawnedObjectPool.instance.ReturnObs(this);
        }
    }
}
