using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : SpawnedObject
{
    public float yPos;

    // 자리 차지 여부
    public bool occupyLow;
    public bool occupyMid;
    public bool occupyHigh;
}
