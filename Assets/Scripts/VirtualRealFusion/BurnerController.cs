using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCE;

public class BurnerController : MonoBehaviour {

    public GameObject tubeBreak;
    [HideInInspector]
    public bool lidIsOn = true;

    private bool onFire = false;

    void OnTriggerEnter(Collider other) {
        if (!onFire) {
            if (!lidIsOn) {
                MatchController match = other.GetComponent<MatchController>();
                if (match && match.onFire) {
                    SetFire();
                }
            }
        }
    }

    public void SetFire() {
        GetComponent<Fire>().SetFire();
        transform.Find("Flames").GetComponent<UCE_Heatable>().SetFire();
        onFire = true;
        TipBoard.Progress(0, 1);
        TipBoard.Progress(2, 0);
    }

    public void PutOutFire() {
        if (onFire && TipBoard.sn == 4 && TipBoard.ssn == 0) {
            tubeBreak.GetComponent<TubeExplode>().Explode();
        }
        GetComponent<Fire>().PutOutFire();
        transform.Find("Flames").GetComponent<UCE_Heatable>().PutOutFire();
        onFire = false;
        TipBoard.Progress(1, 1);
        TipBoard.Progress(1, 2);
        TipBoard.Progress(4, 1);
    }

    void FixedUpdate() {
        if (onFire && (transform.parent.rotation.x < -0.5 || transform.parent.rotation.x > 0.5 || transform.parent.rotation.z < -0.5
            || transform.parent.rotation.z > 0.5)) {
            // TODO
            GameObject.Find("FlamesParticleEffect").GetComponent<ParticleSystem>().Play();
        }
    }
}
