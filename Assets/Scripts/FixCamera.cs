using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixCamera : MonoBehaviour {
	// Use this for initialization
	void Start () {
       FixHead();
	}

    void FixHead() {
        GameObject eye = GameObject.Find("Camera (eye)");
        Vector3 delta_pos = transform.position - eye.transform.position;
        GameObject real = GameObject.Find("RealView");
        real.transform.Translate(delta_pos);
    }
}
