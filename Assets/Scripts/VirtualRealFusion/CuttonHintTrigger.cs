using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCE;

public class CuttonHintTrigger : HintTrigger {

    [Header("Snap Drop")]
    public Transform snapDropObject;
    public Transform snapDropPoint;

    public void GraspBegin() {
        OpenTrigger();
    }

    public void GraspEnd() {
        if (hintObject.activeInHierarchy) {
            snapDropObject.position = snapDropPoint.position;
            snapDropObject.rotation = snapDropPoint.rotation;

            TipBoard.Progress(1, 3);
        }

        CloseTrigger();
    }
}
