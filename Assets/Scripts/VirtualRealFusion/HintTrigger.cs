using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintTrigger : MonoBehaviour {

    protected bool openTrigger = true;
    protected float currentTime = 0.0f;

    protected Collider myCollider;
    protected bool ignore;

    public bool initOnPlace = false;
    public float duringTime = 0.5f;
    public string targetTag;
    public GameObject hintObject;

	// Use this for initialization
	void Start () {
        ignore = initOnPlace;
        hintObject.SetActive(false);
	}

    public void CloseTrigger() {
        openTrigger = false;
        hintObject.SetActive(false);
    }

    public void OpenTrigger() {
        ignore = false;
        if (!openTrigger) {
            openTrigger = true;
            currentTime = 0.0f;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag.Equals(targetTag)) {
            openTrigger = true;
            currentTime = 0.0f;
        }
    }

    private void OnTriggerStay(Collider other) {
        myCollider = other;
        if (openTrigger && other.gameObject.tag.Equals(targetTag)) {
            if (ignore) {
                ignore = false;
                openTrigger = false;
                return;
            }

            currentTime += Time.deltaTime;
            if (currentTime > duringTime) {
                hintObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag.Equals(targetTag)) {
            openTrigger = false;
            hintObject.SetActive(false);
        }
    }
}
