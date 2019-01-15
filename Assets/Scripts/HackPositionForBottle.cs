using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackPositionForBottle : MonoBehaviour {
    private bool isGrasped = false;
    public GameObject indexEnd;
    public GameObject thumbEnd;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isGrasped) {
            this.transform.position = (indexEnd.transform.position + thumbEnd.transform.position) / 2;
        }
	}

    public void OnGraspBegin() {
        isGrasped = true;
    }

    public void OnGraspEnd() {
        isGrasped = false;
    }
}
