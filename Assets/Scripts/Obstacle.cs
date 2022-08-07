using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Obstacle", menuName = "New Obstacle/obstacle")]
public class Obstacle : ScriptableObject
{
    // ObsType은 반드시 오름차순 정렬되어 있어야 함
    // 그렇지 않으면 ObstaclePool.GetObs()에서 ObsType으로 Obs를 찾을 수 없음
    public enum ObsType {box, can, reef};
    public ObsType type;
    public GameObject prefab;
    public int count; // 초기 생성 개수
    public int additionalCount; // 부족할 시 추가로 생성할 개수
}
