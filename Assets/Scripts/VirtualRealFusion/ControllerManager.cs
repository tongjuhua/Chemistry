using UnityEngine;

[RequireComponent(typeof(SteamVR_ControllerManager))]
public class ControllerManager : MonoBehaviour {

    [HideInInspector]
    public GameObject left, right;
    [HideInInspector]
    public SteamVR_TrackedObject leftObject, rightObject;
    [HideInInspector]
    public SteamVR_Controller.Device leftDevice, rightDevice;

    private bool GetTouchDown(SteamVR_Controller.Device device, ulong buttomMask) {
        return device.GetTouchDown(buttomMask);
    }

    private bool GetPressDown(SteamVR_Controller.Device device, ulong buttomMask) {
        return device.GetPressDown(buttomMask);
    }

    private Vector2 GetAxis(SteamVR_Controller.Device device, Valve.VR.EVRButtonId buttomId) {
        return device.GetAxis(buttomId);
    }


    public bool LeftGetTouchDown(ulong buttomMask) {
        if (leftDevice != null) {
            return GetTouchDown(leftDevice, buttomMask);
        }
        else {
            return false;
        }
    }

    public bool LeftGetPressDown(ulong buttomMask) {
        if (leftDevice != null) {
            return GetPressDown(leftDevice, buttomMask);
        }
        else {
            return false;
        }
    }

    public Vector2 LeftGetAxis(Valve.VR.EVRButtonId buttomId) {
        if (leftDevice != null) {
            return GetAxis(leftDevice, buttomId);
        }
        else {
            return new Vector2(0, 0);
        }
    }


    public bool RightGetTouchDown(ulong buttomMask) {
        if (rightDevice != null) {
            return GetTouchDown(rightDevice, buttomMask);
        }
        else {
            return false;
        }
    }

    public bool RightGetPressDown(ulong buttomMask) {
        if (rightDevice != null) {
            return GetPressDown(rightDevice, buttomMask);
        }
        else {
            return false;
        }
    }

    public Vector2 RightGetAxis(Valve.VR.EVRButtonId buttomId) {
        if (rightDevice != null) {
            return GetAxis(rightDevice, buttomId);
        }
        else {
            return new Vector2(0, 0);
        }
    }

    void Awake() {
        left = GetComponent<SteamVR_ControllerManager>().left;
        right = GetComponent<SteamVR_ControllerManager>().right;
        if (left != null) {
            leftObject = left.GetComponent<SteamVR_TrackedObject>();
        }
        if (right != null) {
            rightObject = right.GetComponent<SteamVR_TrackedObject>();
        }
    }

    // Update is called once per frame
    void Update() {

        if (leftObject != null && leftDevice == null) {
            leftDevice = leftObject.index != SteamVR_TrackedObject.EIndex.None ? SteamVR_Controller.Input((int)leftObject.index) : null;
        }
        else {
            leftDevice = null;
        }

        if (rightObject != null && rightDevice == null) {
            rightDevice = rightObject.index != SteamVR_TrackedObject.EIndex.None ? SteamVR_Controller.Input((int)rightObject.index) : null;
        }
        else {
            rightDevice = null;
        }

        /*
        LeftGetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu);
        LeftGetTouchDown(SteamVR_Controller.ButtonMask.Touchpad);
        LeftGetTouchDown(SteamVR_Controller.ButtonMask.System);
        LeftGetTouchDown(SteamVR_Controller.ButtonMask.Trigger);
        LeftGetPressDown(SteamVR_Controller.ButtonMask.Grip);

        LeftGetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);
        LeftGetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);

        RightGetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu);
        RightGetTouchDown(SteamVR_Controller.ButtonMask.Touchpad);
        RightGetTouchDown(SteamVR_Controller.ButtonMask.System);
        RightGetTouchDown(SteamVR_Controller.ButtonMask.Trigger);
        RightGetPressDown(SteamVR_Controller.ButtonMask.Grip);

        RightGetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);
        RightGetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);
        */
    }
}