using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsDestroyTest : MonoBehaviour {
    void Start() {
        StartCoroutine(Timer());
    }

    // ������ �� Ư�� �ð� ��� �� ������Ʈ Ǯ�� �ݳ�
    IEnumerator Timer() {
        var wait = new WaitForSeconds(1f);
        int time = 3;

        while (time > 0) {
            time--;
            yield return wait;
        }

        ObstacleGenerator.instance.ReturnObs(gameObject);
    }
}
