using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 뒤로 이동한 배경을 앞으로 재배치하는 스크립트 (About z)
public class MapScrolling : MonoBehaviour
{
    [SerializeField] float endPosition;
    [SerializeField] float startPosition;

    [SerializeField] List<Transform> children;

    [SerializeField] float speed;

    private void Awake()
    {
        // 자식 트랜스폼 가져오기
        foreach (Transform child in transform)
        {
            //Debug.Log(child.name);
            children.Add(child);
        }
    }

    private void FixedUpdate()
    {
        // 현재 z위치가 endPosition 이하로 이동했을때 위치를 리셋
        foreach (Transform transform in children)
        {
            transform.Translate(Vector3.back * speed);
            if (transform.position.z <= endPosition)
            {
            Reposition(transform);
            }
        }
    }

    // 위치를 리셋하는 메서드
    private void Reposition(Transform transform)
    {
        // 현재 z위치를 startPosition으로 리셋
        transform.position = new Vector3(transform.position.x, transform.position.y, startPosition);
    }
}