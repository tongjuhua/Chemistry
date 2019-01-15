using System;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour {

    public string portname = "5";
    public int preCount = 200;
    public double ignoreThreshold = 0.01;
    public float posTransformer = 100;

    public Vector3 velocity;

    private int baudRate = 115200;
    public Vector3 accuPreNum;
    public Vector3 averStart;


    SerialPort sp = null;

    private void Start() {

        foreach (var item in SerialPort.GetPortNames()) {
            Debug.Log(item);
        }


        OpenPort();
        //InvokeRepeating("GetNumOfData", 0.02f, 0.02f);
        StartCoroutine("GetNumOfData");
        srcCount = preCount;
    }


    private void OpenPort() {
        sp = new SerialPort(@"\\.\COM" + portname, baudRate);
        sp.ReadTimeout = 400;
        try {
            sp.Open();
        }
        catch (System.Exception ex) {
            Debug.Log(ex.Message);
        }
    }


    private void ClosePort() {
        try {
            sp.Close();
        }
        catch (System.Exception ex) {
            Debug.Log(ex.Message);
        }
    }

    byte[] RxBuffer = new byte[1000];
    UInt16 usRxLength = 0;
    //delegate void UpdateData(byte[] byteData);


    private double[] LastTime = new double[10];
    short sRightPack = 0;
    short[] ChipTime = new short[7];
    double[] a = new double[4], w = new double[4], h = new double[4], Angle = new double[4], Port = new double[4];
    double[] q = new double[4];
    double Temperature, Pressure, Altitude, GroundVelocity, GPSYaw, GPSHeight;
    long Longitude, Latitude;
    private DateTime TimeStart = DateTime.Now;
    private Int32 Baund = 115200;
    double TimeElapse = 0;
    private int srcCount;
    private void DecodeData(byte[] byteTemp) {
        double[] Data = new double[4];
        DateTime now = DateTime.Now;
        TimeElapse = (now - TimeStart).TotalMilliseconds / 1000;
        TimeStart = now;
        Data[0] = BitConverter.ToInt16(byteTemp, 2);
        Data[1] = BitConverter.ToInt16(byteTemp, 4);
        Data[2] = BitConverter.ToInt16(byteTemp, 6);
        Data[3] = BitConverter.ToInt16(byteTemp, 8);
        sRightPack++;
        switch (byteTemp[1]) {
            case 0x50:
                //Data[3] = Data[3] / 32768 * double.Parse(textBox9.Text) + double.Parse(textBox8.Text);
                ChipTime[0] = (short)(2000 + byteTemp[2]);
                ChipTime[1] = byteTemp[3];
                ChipTime[2] = byteTemp[4];
                ChipTime[3] = byteTemp[5];
                ChipTime[4] = byteTemp[6];
                ChipTime[5] = byteTemp[7];
                ChipTime[6] = BitConverter.ToInt16(byteTemp, 8);


                break;
            case 0x51:
                //Data[3] = Data[3] / 32768 * double.Parse(textBox9.Text) + double.Parse(textBox8.Text);
                Temperature = Data[3] / 100.0;
                Data[0] = Data[0] / 32768.0 * 16;
                Data[1] = Data[1] / 32768.0 * 16;
                Data[2] = Data[2] / 32768.0 * 16;

                a[0] = Data[0];
                a[1] = Data[1];
                a[2] = Data[2];
                a[3] = Data[3];
                if ((TimeElapse - LastTime[1]) < 0.1) return;
                LastTime[1] = TimeElapse;

                break;
            case 0x52:
                //Data[3] = Data[3] / 32768 * double.Parse(textBox9.Text) + double.Parse(textBox8.Text);
                Temperature = Data[3] / 100.0;
                Data[0] = Data[0] / 32768.0 * 2000;
                Data[1] = Data[1] / 32768.0 * 2000;
                Data[2] = Data[2] / 32768.0 * 2000;
                w[0] = Data[0];
                w[1] = Data[1];
                w[2] = Data[2];
                w[3] = Data[3];

                if ((TimeElapse - LastTime[2]) < 0.1) return;
                LastTime[2] = TimeElapse;
                break;
            case 0x53:
                //Data[3] = Data[3] / 32768 * double.Parse(textBox9.Text) + double.Parse(textBox8.Text);
                Temperature = Data[3] / 100.0;
                Data[0] = Data[0] / 32768.0 * 180;
                Data[1] = Data[1] / 32768.0 * 180;
                Data[2] = Data[2] / 32768.0 * 180;
                Angle[0] = Data[0];
                Angle[1] = Data[1];
                Angle[2] = Data[2];
                Angle[3] = Data[3];
                if ((TimeElapse - LastTime[3]) < 0.1) return;
                LastTime[3] = TimeElapse;
                break;
            case 0x54:
                //Data[3] = Data[3] / 32768 * double.Parse(textBox9.Text) + double.Parse(textBox8.Text);
                Temperature = Data[3] / 100.0;
                h[0] = Data[0];
                h[1] = Data[1];
                h[2] = Data[2];
                h[3] = Data[3];
                if ((TimeElapse - LastTime[4]) < 0.1) return;
                LastTime[4] = TimeElapse;
                break;
            case 0x55:
                Port[0] = Data[0];
                Port[1] = Data[1];
                Port[2] = Data[2];
                Port[3] = Data[3];

                break;

            case 0x56:
                Pressure = BitConverter.ToInt32(byteTemp, 2);
                Altitude = (double)BitConverter.ToInt32(byteTemp, 6) / 100.0;

                break;

            case 0x57:
                Longitude = BitConverter.ToInt32(byteTemp, 2);
                Latitude = BitConverter.ToInt32(byteTemp, 6);

                break;

            case 0x58:
                GPSHeight = (double)BitConverter.ToInt16(byteTemp, 2) / 10.0;
                GPSYaw = (double)BitConverter.ToInt16(byteTemp, 4) / 10.0;
                GroundVelocity = BitConverter.ToInt16(byteTemp, 6) / 1e3;

                break;
            case 0x59:
                q[0] = Data[0] / 32768.0;
                q[1] = Data[1] / 32768.0;
                q[2] = Data[2] / 32768.0;
                q[3] = Data[3] / 32768.0;
                //Debug.Log(Data[0] + " " + Data[1] + " " + Data[2] + " " + Data[3]);
                break;
            default:
                break;
        }
        //Debug.Log(a[0] + " " + a[1] + " " + a[2]);
        MoveObj();
    }
    int iaef = 0;
    int[][] data = {
        new int[4]{ 0, 1, 2, 3},
        new int[4]{ 0, 1, 3, 2},
        new int[4]{ 0, 2, 1, 3},
        new int[4]{ 0, 2, 3, 1},
        new int[4]{ 0, 3, 1, 2},
        new int[4]{ 0, 3, 2, 1},
        new int[4]{ 1, 0, 2, 3},
        new int[4]{ 1, 0, 3, 2},
        new int[4]{ 1, 2, 0, 3},
        new int[4]{ 1, 2, 3, 0},
        new int[4]{ 1, 3, 2, 0},
        new int[4]{ 1, 3, 0, 2},
        new int[4]{ 2, 0, 1, 3},
        new int[4]{ 2, 0, 3, 1},
        new int[4]{ 2, 1, 0, 3},
        new int[4]{ 2, 1, 3, 0},
        new int[4]{ 2, 3, 1, 0},
        new int[4]{ 2, 3, 0, 1},
        new int[4]{ 3, 0, 1, 2},
        new int[4]{ 3, 0, 2, 1},
        new int[4]{ 3, 1, 2, 0},
        new int[4]{ 3, 1, 0, 2},
        new int[4]{ 3, 2, 1, 0},
        new int[4]{ 3, 2, 0, 1}};
    void Update() {
        //MoveObj();


        if (Input.GetKeyDown(KeyCode.Q)) {
            iaef++;
            Debug.Log(iaef);
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            iaef--;
            Debug.Log(iaef);
        }
    }

    public Vector3 globalAccu;
    public Vector3 globalRotation;

    private void MoveObj() {
        //this.transform.rotation = Quaternion.Euler((float)Angle[0] + 38.661f, -(float)Angle[2] + 31.16499f, (float)Angle[1] - 99.069f);
        //Debug.Log((float)Angle[0] + " " + (float)Angle[1] + " " + (float)Angle[2]);
        //Debug.Log(Angle[0] + " " + Angle[1] + " " + Angle[2]);
        //Never use EULER in UNITY!!!!
        //this.transform.localRotation = new Quaternion((float)q[data[iaef][0]]
        //    , (float)q[data[iaef][1]]
        //    , (float)q[data[iaef][2]]
        //    , (float)q[data[iaef][3]]);
        this.transform.localRotation = new Quaternion((float)q[1], (float)q[0], (float)q[2], (float)q[3]);
        //this.transform.localRotation = new Quaternion((float)q[0], -(float)q[1], (float)q[2], (float)q[3]);
        globalRotation = this.transform.rotation.eulerAngles;

        if (preCount > 100) {
            preCount--;
        }
        else if (preCount > 0) {
            // globalAccu = this.transform.rotation * new Vector3((float)a[0], (float)a[1], (float)a[2]);
            globalAccu = this.transform.rotation * new Vector3((float)a[1], (float)a[2], -(float)a[0]);
            //globalAccu = new Vector3((float)a[0], (float)a[1], (float)a[2]);
            //accuPreNum += globalAccu;
            accuPreNum += new Vector3((float)a[0], (float)a[1], (float)a[2] - 1);
            preCount--;
        }
        else if (preCount == 0) {
            averStart = accuPreNum / (float)(srcCount - 100);

            preCount--;
            Debug.Log("Finished");
        }
        else {
            globalAccu = this.transform.rotation * new Vector3((float)a[0], (float)a[1], (float)a[2]);
            //Vector3 atmp = new Vector3((float)a[0], (float)a[1], (float)a[2]) - averStart;
            //globalAccu = this.transform.rotation * new Vector3(atmp.y, atmp.z, -atmp.x);
            //globalAccu = this.transform.rotation * new Vector3((float)a[1], (float)a[2], -(float)a[0]);
            //velocity.x += (float)((Mathf.Abs(globalAccu.x - (float)averStart.x) > ignoreThreshold) ?
            //    ((globalAccu.x - (float)averStart.x) * TimeElapse) : 0);
            //velocity.y += (float)((Mathf.Abs(globalAccu.y - (float)averStart.y) > ignoreThreshold) ?
            //    ((globalAccu.y - (float)averStart.y) * TimeElapse) : 0);
            //velocity.z += (float)((Mathf.Abs(globalAccu.z - (float)averStart.z) > ignoreThreshold) ?
            //    ((globalAccu.z - (float)averStart.z) * TimeElapse) : 0);
            velocity += globalAccu - Vector3.up;
            this.transform.position += this.transform.rotation
                * new Vector3(
                (float)velocity.x * (float)TimeElapse / posTransformer,
                (float)velocity.y * (float)TimeElapse / posTransformer,
                (float)velocity.z * (float)TimeElapse / posTransformer);
            
        }

    }

    UInt16 index = 0;
    private IEnumerator GetNumOfData() {
        while (true) {
            UInt16 usLength = 0;
            byte[] byteTemp = new byte[11];
            if (sp != null && sp.IsOpen) {
                //try {
                usLength = (UInt16)sp.Read(RxBuffer, index, 700);
                index = 0;
                while ((usLength - index) > 11) {
                    //RxBuffer.CopyTo(byteTemp, index);
                    for (int i = index; i < index + 11; i++)
                        byteTemp[i - index] = RxBuffer[i];
                    if (!((byteTemp[0] == 0x55) & ((byteTemp[1] & 0x50) == 0x50))) {
                        index++;
                        continue;
                    }
                    if (((byteTemp[0] + byteTemp[1] + byteTemp[2] + byteTemp[3] + byteTemp[4] + byteTemp[5] + byteTemp[6] + byteTemp[7] + byteTemp[8] + byteTemp[9]) & 0xff) == byteTemp[10])
                        DecodeData(byteTemp);
                    index += 11;
                    //Debug.Log
                }
                for (int i = index; i < usLength; i++)
                    RxBuffer[i - index] = RxBuffer[i];
                //Debug.Log(index + " " + usLength);
                index = (UInt16)(usLength - index);


                //    usLength = (UInt16)sp.Read(RxBuffer, usRxLength, 700);
                //    usRxLength += usLength;
                //    //print(sp.ReadByte());
                //    while (usRxLength >= 11) {
                //        //UpdateData Update = new UpdateData(DecodeData);
                //        RxBuffer.CopyTo(byteTemp, 0);
                //        if (!((byteTemp[0] == 0x55) & ((byteTemp[1] & 0x50) == 0x50))) {
                //            for (int i = 1; i < usRxLength; i++) RxBuffer[i - 1] = RxBuffer[i];
                //            usRxLength--;
                //            continue;
                //        }
                //        if (((byteTemp[0] + byteTemp[1] + byteTemp[2] + byteTemp[3] + byteTemp[4] + byteTemp[5] + byteTemp[6] + byteTemp[7] + byteTemp[8] + byteTemp[9]) & 0xff) == byteTemp[10])
                //            //this.Control.Invoke(Update, byteTemp);
                //            //this.Invoke("")
                //            DecodeData(byteTemp);
                //        for (int i = 11; i < usRxLength; i++) RxBuffer[i - 11] = RxBuffer[i];
                //        usRxLength -= 11;
                //    }
                //}
                //catch (System.Exception ex) {

                //    Debug.Log(ex.Message);
                //}
            }
            yield return new WaitForSeconds(0.02f);
        }
    }

    private void OnDisable() {
        print("OnDisable");
        ClosePort();
    }

    //void FixedUpdate()
    //{

    //}
}
