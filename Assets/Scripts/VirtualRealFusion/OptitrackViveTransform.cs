using UnityEngine;
using System.IO;
using Conversion;
using Matrix;

public class OptitrackViveTransform : MonoBehaviour {
    
    public enum HandType {
        Left,
        Right
    }

    [Header("Optitrack")]
    public int controllerID;
    public OptitrackStreamingClient client;
    public GameObject[] objects;
    [Header("HTC Vive")]
    public HandType type;
    public ControllerManager controller;
    private Transform leftPivot;
    private Transform rightPivot;
    [Header("Debug")]
    public bool isDebug;
    public GameObject debugObject;

    [Header("Read-Only Data [For Debug]")]

    // has not calculated yet
    private bool isCalculated;
    // is it from data file
    private bool fromDataFile;

    // record by htc
    [SerializeField]
    private Vector3[] vivePoints;
    private Quaternion[] viveQuaternions;
    private Vector3 viveRefinePoint;
    // record by optitrack
    [SerializeField]
    private Vector3[] optiPoints;
    private Quaternion[] optiQuaternions;
    private Vector3 optiRefinePoint;
    // record number;
    private int number = 0;

    // vector calculated by points;
    private Vector3 X;
    private Vector3 Xprime;
    private Vector3 Y;
    private Vector3 Yprime;
    [SerializeField]
    private float ratioX;
    [SerializeField]
    private float ratioY;
    private float ratio;

    // all attributes for calculation
    private Vector3 deltaX;
    private Vector3 sumX;
    private Vector3 deltaY;
    private Vector3 sumY;

    private double buttomU23;
    private double buttomU13;
    private double buttomU12;

    private double upU1;
    private double upU2;
    private double upU3;

    private double U1;
    private double U2;
    private double U3;

    // the Transform Matrix
    private Matrix3X3 U;
    [SerializeField]
    private Matrix3X3 R;
    private Vector3 T;

    // the Delta Quaternion
    private Quaternion QR;
    private Quaternion[] Qi;
    private Quaternion Q;

    // store the result
    private Vector3 debugObjectPosition;
    private Quaternion debugObjectRotation;
    private Vector3[] objectsPosition;
    private Quaternion[] objectsRotation;

    // store the tracker
    private OptitrackObject[] trackers;

    // tmp
    private OptitrackPose tmpPose;

    // File
    private static string filePath;
    private static readonly string fileName = "OptitrackViveData.txt";



    // refine T
    public void RefineTransform(Vector3 vec) {
        T += vec;
    }

    public void FindObjects() {
        trackers = GameObject.FindObjectsOfType<OptitrackObject>();

        objectsPosition = new Vector3[trackers.Length];
        objectsRotation = new Quaternion[trackers.Length];
        objects = new GameObject[trackers.Length];

        for (int i = 0; i < objects.Length; i++) {
            objects[i] = trackers[i].gameObject;
        }

        if (isCalculated) {
            for (int i = 0; i < objects.Length; i++) {
                if (trackers[i] != null) {
                    tmpPose = client.GetLatestRigidBodyState(trackers[i].ID).Pose;
                    trackers[i].Q = Quaternion.Inverse(Quaternion.Inverse(trackers[i].transform.rotation) * QR * tmpPose.Orientation);
                }
            }
        }
    }


    // record data
    private void RecordData() {
        if (number >= 3) return;

        tmpPose = client.GetLatestRigidBodyState(controllerID).Pose;
        optiPoints[number] = tmpPose.Position;
        optiQuaternions[number] = tmpPose.Orientation;
        if (type == HandType.Left) {
            vivePoints[number] = leftPivot.position;
            viveQuaternions[number] = leftPivot.rotation;
        }
        else {
            vivePoints[number] = rightPivot.position;
            viveQuaternions[number] = rightPivot.rotation;
        }

        number++;
    }

    // save data in file
    private void SaveData() {
        StreamWriter sw;
        sw = File.CreateText(filePath + "//" + fileName);
        for (int i = 0; i < 3; i++) {
            sw.WriteLine(ConversionUtils.Vector2String(vivePoints[i]));
            sw.WriteLine(ConversionUtils.Vector2String(optiPoints[i]));
            sw.WriteLine(ConversionUtils.Quaternion2String(viveQuaternions[i]));
            sw.WriteLine(ConversionUtils.Quaternion2String(optiQuaternions[i]));
        }
        sw.Close();
        sw.Dispose();
    }

    // load data from file
    private void LoadData() {
        StreamReader sr;
        sr = File.OpenText(filePath + "//" + fileName);
        for (int i = 0; i < 3; i++) {
            vivePoints[i] = ConversionUtils.String2Vector(sr.ReadLine());
            optiPoints[i] = ConversionUtils.String2Vector(sr.ReadLine());
            viveQuaternions[i] = ConversionUtils.String2Quaternion(sr.ReadLine());
            optiQuaternions[i] = ConversionUtils.String2Quaternion(sr.ReadLine());
        }
        sr.Close();
        sr.Dispose();
    }



    // Use this for initialization
    void Start() {
        filePath = Application.streamingAssetsPath;

        leftPivot = controller.left.transform.Find("pivot");
        rightPivot = controller.right.transform.Find("pivot");

        isCalculated = false;
        fromDataFile = false;

        vivePoints = new Vector3[3];
        optiPoints = new Vector3[3];
        viveQuaternions = new Quaternion[3];
        optiQuaternions = new Quaternion[3];
        Qi = new Quaternion[3];
        
        objectsPosition = new Vector3[objects.Length];
        objectsRotation = new Quaternion[objects.Length];
        trackers = new OptitrackObject[objects.Length];

        for (int i = 0; i < objects.Length; i++) {
            trackers[i] = objects[i].GetComponent<OptitrackObject>();
        }
    }

    void FixedUpdate() {
        if (number == 3) {
            if (!isCalculated) {
                X = vivePoints[1] - vivePoints[0];
                Xprime = optiPoints[1] - optiPoints[0];
                ratioX = X.magnitude / Xprime.magnitude;
                Xprime = Xprime * ratioX;
                deltaX = X - Xprime;
                sumX = X + Xprime;

                Y = vivePoints[2] - vivePoints[1];
                Yprime = optiPoints[2] - optiPoints[1];
                ratioY = Y.magnitude / Yprime.magnitude;
                Yprime = Yprime * ratioY;
                ratio = Mathf.Sqrt(ratioX * ratioY);
                deltaY = Y - Yprime;
                sumY = Y + Yprime;

                buttomU12 = sumX.y * 1.0 * sumY.x - sumY.y * 1.0 * sumX.x;
                buttomU13 = sumX.x * 1.0 * sumY.z - sumY.x * 1.0 * sumX.z;
                buttomU23 = sumX.z * 1.0 * sumY.y - sumY.z * 1.0 * sumX.y;

                upU1 = (deltaX.y + deltaX.z) * 1.0 * sumY.x - (deltaY.y + deltaY.z) * 1.0 * sumX.x;
                upU2 = (deltaX.x + deltaX.z) * 1.0 * sumY.y - (deltaY.x + deltaY.z) * 1.0 * sumX.y;
                upU3 = (deltaX.x + deltaX.y) * 1.0 * sumY.z - (deltaY.x + deltaY.y) * 1.0 * sumX.z;

                U1 = upU1 / (buttomU12 + buttomU13);
                U2 = upU2 / (buttomU12 + buttomU23);
                U3 = upU3 / (buttomU13 + buttomU23);

                U = MatrixUtils.Screw(U1, U2, U3);
                R = MatrixUtils.Inverse(Matrix3X3.I - U) * (Matrix3X3.I + U);

                T = Vector3.zero;
                for (int i = 0; i < 3; i++) {
                    T += vivePoints[i] - R * ratio * optiPoints[i];
                    //print(vivePoints[i] - R * ratio * optiPoints[i]);
                }
                T = T / 3;

                QR = MatrixUtils.MatrixToQuaternion(R);
                for (int i = 0; i < 3; i++) {
                    Qi[i] = Quaternion.Inverse(Quaternion.Inverse(viveQuaternions[i]) * QR * optiQuaternions[i]);
                }
                Q = Quaternion.Slerp(Qi[0], Qi[1], 1.0f / 2);
                Q = Quaternion.Slerp(Q, Qi[2], 1.0f / 3);

                for (int i = 0; i < objects.Length; i++) {
                    if (trackers[i] != null) {
                        tmpPose = client.GetLatestRigidBodyState(trackers[i].ID).Pose;
                        trackers[i].Q = Quaternion.Inverse(Quaternion.Inverse(trackers[i].originRotation) * QR * tmpPose.Orientation);
                    }
                }

                if (fromDataFile) {
                    T = viveRefinePoint - R * ratio * optiRefinePoint;
                    fromDataFile = false;
                }

                isCalculated = true;
            }

            if (isDebug) {
                tmpPose = client.GetLatestRigidBodyState(controllerID).Pose;
                debugObjectPosition = R * ratio * tmpPose.Position + T;
                debugObjectRotation = QR * tmpPose.Orientation * Q;
            }

            for (int i = 0; i < objects.Length; i++) {
                if (trackers[i] != null && trackers[i].isTracking) {
                    tmpPose = client.GetLatestRigidBodyState(trackers[i].ID).Pose;
                    objectsPosition[i] = R * ratio * tmpPose.Position + T;
                    objectsRotation[i] = QR * tmpPose.Orientation * trackers[i].Q;
                }
            }
        }
    }

    // Update is called once per frame
    void Update () {

        if (number == 3) {
            if (isDebug) {
                debugObject.SetActive(true);

                debugObject.transform.position = debugObjectPosition;
                debugObject.transform.rotation = debugObjectRotation;
            }
            else {
                debugObject.SetActive(false);
            }

            for (int i = 0; i < objects.Length; i++) {
                if (trackers[i] != null) {
                    objects[i].transform.position = objectsPosition[i];
                    objects[i].transform.rotation = objectsRotation[i];
                }
            }
        }

        if (type == HandType.Left) {
            if (controller.LeftGetTouchDown(SteamVR_Controller.ButtonMask.Trigger)) {
                if (number >= 3) return;
                RecordData();
            }

            if (controller.LeftGetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu)) {
                if (number < 3) return;
                SaveData();
            }

            if (controller.LeftGetPressDown(SteamVR_Controller.ButtonMask.Grip)) {
                LoadData();
                number = 3;
                isCalculated = false;
                fromDataFile = true;

                tmpPose = client.GetLatestRigidBodyState(controllerID).Pose;
                optiRefinePoint = tmpPose.Position;
                viveRefinePoint = leftPivot.position;
            }
        }
        else {
            if (controller.RightGetTouchDown(SteamVR_Controller.ButtonMask.Trigger)) {
                if (number >= 3) return;
                RecordData();
            }

            if (controller.RightGetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu)) {
                if (number < 3) return;
                SaveData();
            }

            if (controller.RightGetPressDown(SteamVR_Controller.ButtonMask.Grip)) {
                LoadData();
                number = 3;
                isCalculated = false;
                fromDataFile = true;

                tmpPose = client.GetLatestRigidBodyState(controllerID).Pose;
                optiRefinePoint = tmpPose.Position;
                viveRefinePoint = rightPivot.position;
            }
        }
	}
}