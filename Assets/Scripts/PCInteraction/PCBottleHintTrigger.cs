using UnityEngine;
using UCE;

public class PCBottleHintTrigger : HintTrigger {

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

            TipBoard.Progress(3, 3);
        }

        CloseTrigger();
    }
}
