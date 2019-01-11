using UnityEngine;
using System.IO;
using Conversion;

public class HandPositionAlignment : MonoBehaviour {

    public enum HandType {
        LeftMainRightSupported,
        RightMainLeftSupported
    }

    [Header("Vive")]
    public ControllerManager manager;
    public HandType type;
    private Vector2 axis;

    [Header("Leap Motion")]
    public Transform leapMotionRig;
    private Vector3 origin;

    // for hand offset
    [Header("Hand Offest")]
    public float touchSpeed = 0.01f;
    public Vector3 offset;

    // File
    private static string filePath;
    private static string fileName = "HandPositionData.txt";


    // save data in file
    private void SaveData() {
        StreamWriter sw;
        sw = File.CreateText(filePath + "//" + fileName);
        sw.WriteLine(ConversionUtils.Vector2String(offset));
        sw.Close();
        sw.Dispose();
    }

    // load data from file
    private void LoadData() {
        StreamReader sr;
        sr = File.OpenText(filePath + "//" + fileName);
        offset = ConversionUtils.String2Vector(sr.ReadLine());
        sr.Close();
        sr.Dispose();
    }



    // Use this for initialization
    void Start () {
        filePath = Application.streamingAssetsPath;
        origin = leapMotionRig.localPosition;
        leapMotionRig.localPosition = origin + offset;
    }

    // Update is called once per frame
    void Update() {
        if (type == HandType.LeftMainRightSupported) {
            if (manager.LeftGetTouchDown(SteamVR_Controller.ButtonMask.Touchpad)) {
                axis = manager.LeftGetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0) * touchSpeed;
                offset.x += axis.x;
                offset.y += axis.y;
                leapMotionRig.localPosition = origin + offset;
            }

            if (manager.RightGetTouchDown(SteamVR_Controller.ButtonMask.Touchpad)) {
                axis = manager.RightGetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0) * touchSpeed;
                offset.z += axis.y;
                leapMotionRig.localPosition = origin + offset;
            }

            if (manager.LeftGetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu)) {
                SaveData();
            }

            if (manager.LeftGetPressDown(SteamVR_Controller.ButtonMask.Grip)) {
                LoadData();
                leapMotionRig.localPosition = origin + offset;
            }
        }
        else {
            if (manager.RightGetTouchDown(SteamVR_Controller.ButtonMask.Touchpad)) {
                axis = manager.RightGetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0) * touchSpeed;
                offset.x += axis.x;
                offset.y += axis.y;
                leapMotionRig.localPosition = origin + offset;
            }

            if (manager.LeftGetTouchDown(SteamVR_Controller.ButtonMask.Touchpad)) {
                axis = manager.LeftGetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0) * touchSpeed;
                offset.z += axis.y;
                leapMotionRig.localPosition = origin + offset;
            }

            if (manager.RightGetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu)) {
                SaveData();
            }

            if (manager.RightGetPressDown(SteamVR_Controller.ButtonMask.Grip)) {
                LoadData();
                leapMotionRig.localPosition = origin + offset;
            }
        }
    }
}
