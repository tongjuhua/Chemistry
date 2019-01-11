using UnityEngine;
using System.IO;
using Conversion;

public class RealSceneAlignment : MonoBehaviour {

    public enum HandType {
        Left,
        Right
    }

    [Header("Vive")]
    public ControllerManager manager;
    private Transform leftBase;
    private Transform leftBaseAttach;
    private Transform leftBody;
    private Transform leftBodyAttach;
    private Transform rightBase;
    private Transform rightBaseAttach;
    private Transform rightBody;
    private Transform rightBodyAttach;
    private Vector3 realTransform;
    public HandType type;

    [Header("Real Scene")]
    public Transform viewing;
    public Transform comparedObject;

    [Header("Correct Trasform")]
    public OptitrackViveTransform ovTransform;

    // File
    private static string filePath;
    private static readonly string fileName = "RealSceneData.txt";

    // save data in file
    private void SaveData() {
        StreamWriter sw;
        sw = File.CreateText(filePath + "//" + fileName);
        sw.WriteLine(ConversionUtils.Vector2String(realTransform - viewing.position));
        sw.Close();
        sw.Dispose();
    }

    // load data from file
    private void LoadData() {
        StreamReader sr;
        sr = File.OpenText(filePath + "//" + fileName);
        realTransform = ConversionUtils.String2Vector(sr.ReadLine()) + viewing.position;
        sr.Close();
        sr.Dispose();
    }



    // Use this for initialization
    void Start () {
        filePath = Application.streamingAssetsPath;
    }

    // Update is called once per frame
    void Update() {

        if (leftBaseAttach == null) {
            leftBase = manager.left.transform.Find("Model").Find("base");
            if (leftBase != null) {
                leftBaseAttach = leftBase.Find("attach");
            }
        }

        if (leftBodyAttach == null) {
            leftBody = manager.left.transform.Find("Model").Find("body");
            if (leftBody != null) {
                leftBodyAttach = leftBody.Find("attach");
            }
        }

        if (rightBaseAttach == null) {
            rightBase = manager.right.transform.Find("Model").Find("base");
            if (rightBase != null) {
                rightBaseAttach = rightBase.Find("attach");
            }
        }

        if (rightBodyAttach == null) {
            rightBody = manager.right.transform.Find("Model").Find("body");
            if (rightBody != null) {
                rightBodyAttach = rightBody.Find("attach");
            }
        }


        if (type == HandType.Left) {
            if (manager.LeftGetTouchDown(SteamVR_Controller.ButtonMask.Trigger)) {
                if (leftBaseAttach != null && leftBodyAttach != null) {
                    realTransform.x = leftBaseAttach.position.x;
                    realTransform.z = leftBaseAttach.position.z;
                    realTransform.y = leftBodyAttach.position.y;

                    viewing.position -= (realTransform - comparedObject.position);

                    if (ovTransform != null) {
                        ovTransform.RefineTransform(comparedObject.position - realTransform);
                    }
                }
            }

            if (manager.LeftGetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu)) {
                SaveData();
            }

            if (manager.LeftGetPressDown(SteamVR_Controller.ButtonMask.Grip)) {
                LoadData();

                viewing.position -= (realTransform - comparedObject.position);
                print(realTransform);

                if (ovTransform != null) {
                    ovTransform.RefineTransform(comparedObject.position - realTransform);
                }
            }
        }
        else {
            if (manager.RightGetTouchDown(SteamVR_Controller.ButtonMask.Trigger)) {
                if (rightBaseAttach != null && rightBodyAttach != null) {
                    realTransform.x = rightBaseAttach.position.x;
                    realTransform.z = rightBaseAttach.position.z;
                    realTransform.y = rightBodyAttach.position.y;

                    viewing.position -= (realTransform - comparedObject.position);

                    if (ovTransform != null) {
                        ovTransform.RefineTransform(comparedObject.position - realTransform);
                    }
                }
            }

            if (manager.RightGetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu)) {
                SaveData();
            }

            if (manager.RightGetPressDown(SteamVR_Controller.ButtonMask.Grip)) {
                LoadData();

                viewing.position -= (realTransform - comparedObject.position);
                print(realTransform);

                if (ovTransform != null) {
                    ovTransform.RefineTransform(comparedObject.position - realTransform);
                }
            }
        }
    }
}
