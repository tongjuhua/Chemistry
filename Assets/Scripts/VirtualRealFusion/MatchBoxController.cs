using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchBoxController : MonoBehaviour {

    public float sensitivity = 0.4f;

    private float enterTime;
    private Vector3 enterPos;

    void OnTriggerEnter(Collider other) {
        MatchController match = other.GetComponent<MatchController>();
        if (match) {
            enterTime = Time.time;
            enterPos = other.transform.position;
        }
    }

    void OnTriggerStay(Collider other) {
        MatchController match = other.GetComponent<MatchController>();
        if (match) {
            float dx = (other.transform.position - enterPos).magnitude;
            float dt = Time.time - enterTime;
            if (dx / dt > sensitivity) {
                match.SetFire();
            }
        }
    }

    void OnTriggerExit(Collider other) {
        MatchController match = other.GetComponent<MatchController>();
        if (match) {
            float dx = (other.transform.position - enterPos).magnitude;
            float dt = Time.time - enterTime;
            if (dx / dt > sensitivity) {
                match.SetFire();
            }
        }
    }
}
