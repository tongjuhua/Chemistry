using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpsideDownCubeController : MonoBehaviour {
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
			
	}

    void EnableThis() {
        this.gameObject.SetActive(true);
    }

    void DisableThis() {
        this.gameObject.SetActive(false);
    }
}
