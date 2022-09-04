using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ڷ� �̵��� ����� ������ ���ġ�ϴ� ��ũ��Ʈ (About z)
public class MapScrolling : MonoBehaviour
{
    [SerializeField] float endPosition;
    [SerializeField] float startPosition;

    [SerializeField] List<Transform> children;

    [SerializeField] float speed;

    private void Awake()
    {
        // �ڽ� Ʈ������ ��������
        foreach (Transform child in transform)
        {
            Debug.Log(child.name);
            children.Add(child);
        }
    }

    private void FixedUpdate()
    {
        // ���� z��ġ�� endPosition ���Ϸ� �̵������� ��ġ�� ����
        foreach (Transform transform in children)
        {
            transform.Translate(Vector3.back * speed);
            if (transform.position.z <= endPosition)
            {
            Reposition(transform);
            }
        }
    }

    // ��ġ�� �����ϴ� �޼���
    private void Reposition(Transform transform)
    {
        // ���� z��ġ�� startPosition���� ����
        transform.position = new Vector3(transform.position.x, transform.position.y, startPosition);
    }
}