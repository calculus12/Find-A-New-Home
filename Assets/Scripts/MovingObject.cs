using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public enum ObjType {box, can, reef, seagull, coin};
    public ObjType type;
    public float speed;
    public int count; // �ʱ� ���� ����
    public int additionalCount; // ������ �� �߰��� ������ ����
    public float endZPos; // �Ҹ�� ���� z��ǥ

    protected virtual void FixedUpdate() {
        // �̵�
        transform.Translate(Vector3.back * speed, Space.World);
        
        // �Ҹ�
        if (transform.position.z <= endZPos) {
            MovingObjectPool.instance.ReturnObj(this);
        }
    }
}
