using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObsGenTest : MonoBehaviour {
    int obsTypeCount = Enum.GetNames(typeof(ObstacleGenerator.ObsType)).Length;

    void Start() {
        StartCoroutine(GenerateObs());
    }

    // 랜덤한 장애물을 특정 범위 내에 생성
    IEnumerator GenerateObs() {
        var wait = new WaitForSeconds(1f);
        
        while (true) {
            GameObject newObs = ObstacleGenerator.instance.GetObs((ObstacleGenerator.ObsType)UnityEngine.Random.Range(0, obsTypeCount));
            newObs.transform.position = Vector3.one * UnityEngine.Random.Range(-3f, 3f);
            yield return wait;
        }
    }
}
