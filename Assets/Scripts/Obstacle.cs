using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Obstacle", menuName = "New Obstacle/obstacle")]
public class Obstacle : ScriptableObject
{
    // ObsType�� �ݵ�� �������� ���ĵǾ� �־�� ��
    // �׷��� ������ ObstaclePool.GetObs()���� ObsType���� Obs�� ã�� �� ����
    public enum ObsType {box, can, reef};
    public ObsType type;
    public GameObject prefab;
    public int count; // �ʱ� ���� ����
    public int additionalCount; // ������ �� �߰��� ������ ����
}
