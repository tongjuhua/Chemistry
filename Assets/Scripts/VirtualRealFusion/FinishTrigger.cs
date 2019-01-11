using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTrigger : MonoBehaviour {

    protected bool openTrigger = true;
    protected float currentTime = 0.0f;

    public float duringTime = 3.0f;
    public string targetTag;
    public HintTrigger hintTrigger;

    // Use this for initialization
    void Start() {
        
    }

    public virtual void OnFinished(Collider other) {
        openTrigger = false;
        if (hintTrigger != null) {
            hintTrigger.CloseTrigger();
        }
    }

    public virtual void OnRestarted(Collider collider) {
        openTrigger = true;
        currentTime = 0.0f;
        if (hintTrigger != null) {
            hintTrigger.OpenTrigger();
        }
    }

    /*
    public void CloseTrigger() {
        OnFinished();
    }

    public void OpenTrigger() {
        if (!openTrigger) {
            OnRestarted();
        }
    }*/

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag.Equals(targetTag)) {
            if (!openTrigger) {
                OnRestarted(other);
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if (openTrigger && other.gameObject.tag.Equals(targetTag)) {
            currentTime += Time.deltaTime;
            if (currentTime > duringTime) {
                OnFinished(other);
            } 
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag.Equals(targetTag)) {
            if (!openTrigger) {
                OnRestarted(other);
            }
        }
    }
}
