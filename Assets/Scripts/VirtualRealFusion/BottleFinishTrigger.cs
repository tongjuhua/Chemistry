using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCE;

public class BottleFinishTrigger : FinishTrigger {

    [Header("Tracking Enable")]
    public OptitrackObject trackingObject;

    public override void OnFinished(Collider other) {
        base.OnFinished(other);

        if (TipBoard.Progress(3, 3)) {
            trackingObject.isTracking = true;
        }
    }
}
