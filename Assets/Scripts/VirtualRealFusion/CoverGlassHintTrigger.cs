using UnityEngine;
using UCE;

public class CoverGlassHintTrigger : HintTrigger {

    [Header("Snap Drop")]
    public Transform snapDropObject;
    public Transform snapDropPoint;

    [Header("Hierarchy")]
    public Transform snapOnParent;
    public Transform snapOffParent;

    public void GraspBegin() {
        OpenTrigger();
        snapDropObject.parent = snapOffParent;
    }

    public void GraspEnd() {
        if (hintObject.activeInHierarchy) {
            snapDropObject.position = snapDropPoint.position;
            snapDropObject.rotation = snapDropPoint.rotation;
            snapDropObject.parent = snapOnParent;
            TipBoard.Progress(3, 2);
        }

        CloseTrigger();
    }
}
