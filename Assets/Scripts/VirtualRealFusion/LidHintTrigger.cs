using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidHintTrigger : HintTrigger {

    [Header("Snap Drop")]
    public Transform snapDropObject;
    public Transform snapDropPoint;

    [Header("Fire")]
    public BurnerController burner;

    [Header("Hierarchy")]
    public Transform snapOnParent;
    public Transform snapOffParent;

    public void GraspBegin() {
        OpenTrigger();
        burner.lidIsOn = false;
        snapDropObject.parent = snapOffParent;
    }

    public void GraspEnd() {
        if (hintObject.activeInHierarchy) {
            snapDropObject.position = snapDropPoint.position;
            snapDropObject.rotation = snapDropPoint.rotation;
            snapDropObject.parent = snapOnParent;

            burner.PutOutFire();
            burner.lidIsOn = true;
        }

        CloseTrigger();
    }
}
