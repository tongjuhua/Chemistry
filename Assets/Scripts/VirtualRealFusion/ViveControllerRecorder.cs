using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveControllerRecorder : MonoBehaviour {

    public enum HandType {
        Left,
        Right
    }

    [Header("Vive")]
    public ControllerManager manager;
    public HandType type;

    [Header("Record Data")]
    public Vector3 currentPosition;
    public List<Vector3> recordPosition;
    public List<float> length;

	// Use this for initialization
	void Start () {
        recordPosition = new List<Vector3>();
        length = new List<float>();
	}
	
	// Update is called once per frame
	void Update () {

        if (type == HandType.Left) {
            currentPosition = manager.left.transform.position;
            if (manager.LeftGetTouchDown(SteamVR_Controller.ButtonMask.Trigger)) {
                recordPosition.Add(currentPosition);
                if (recordPosition.Count > 1) {
                    length.Add((recordPosition[recordPosition.Count - 1] - recordPosition[recordPosition.Count - 2]).magnitude);
                }
            }
        }
        else {
            currentPosition = manager.right.transform.position;
            if (manager.RightGetTouchDown(SteamVR_Controller.ButtonMask.Trigger)) {
                recordPosition.Add(currentPosition);
                if (recordPosition.Count > 1) {
                    length.Add((recordPosition[recordPosition.Count - 1] - recordPosition[recordPosition.Count - 2]).magnitude);
                }
            }
        }


	}
}
