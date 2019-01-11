using UnityEngine;

public class OptitrackObject : MonoBehaviour {

    public int ID;
    public bool isTracking = true;
    [HideInInspector]
    public Quaternion Q;
    [HideInInspector]
    public Quaternion originRotation;

    private void Start() {
        originRotation = transform.rotation;
    }

}
