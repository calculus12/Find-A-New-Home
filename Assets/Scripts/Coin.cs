using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MovingObject
{
    public float[] yPoses;
    public float rotSpeed;

    protected override void FixedUpdate() {
        base.FixedUpdate();
        transform.Rotate(Vector3.right * rotSpeed);
    }
}
