using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsDestroyTest : MonoBehaviour {
    void Start() {
        StartCoroutine(Timer());
    }

    // 생성된 뒤 특정 시간 경과 후 Pool로 반납
    IEnumerator Timer() {
        var wait = new WaitForSeconds(1f);
        int time = 3;

        while (time > 0) {
            time--;
            yield return wait;
        }

        ObstaclePool.instance.ReturnObs(gameObject);
    }
}
